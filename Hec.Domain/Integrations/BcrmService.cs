using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hec.BcrmServices;
using System.ServiceModel;
using NLog;

namespace Hec.Integrations
{
    public interface IBcrmService
    {
        /// <summary>
        /// Get account information from BCRM based on Account No.
        /// </summary>
        /// <param name="accountNo">Contract Account No.</param>
        /// <returns>Returns null if not found.</returns>
        AccountInfo GetAccountInfo(string accountNo);
    }

    public class FakeBcrmService : IBcrmService
    {
        public AccountInfo GetAccountInfo(string accountNo)
        {
            var acc = new AccountInfo
            {
                AccountNo = accountNo,
                AccountStatus = "Active",
                Name = "SAMPLE",
                RateCategory = "SAMPLE",
                RateCategoryDesc = "SAMPLE",
                DisconnectionStatus = "Not Disconnected",
                PremiseType = "DOM03",
                PremiseTypeDesc = "SAMPLE",
                Email = "SAMPLE",
                MobileNo = "SAMPLE",
                TelephoneNo = "SAMPLE"
            };

            var random = new Random();
            var today = DateTime.Today;

            acc.Bills = Enumerable.Range(1, 6).Select(i => {
                var billDate = today.AddMonths(i - 6);
                var r = random.Next(400);
                return new BillInfo
                {
                    PrintDocument = "BILL " + i,
                    PrintDate = billDate,
                    PostDate = billDate,
                    Amount = (decimal)r,
                    Consumption = (decimal)r / 10
                };
            });
            
            return acc;
        }
    }

    public class BcrmService : IBcrmService
    {
        private string password;
        private string username;
        private EndpointAddress endpoint;
        private BasicHttpBinding binding;

        private static Logger logger = LogManager.GetLogger("integration");

        public BcrmService(string url, string username, string password)
        {
            this.username = username;
            this.password = password;
            this.endpoint = new EndpointAddress(url);

            this.binding = new BasicHttpBinding();
            this.binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            this.binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
        }

        public AccountInfo GetAccountInfo(string accountNo)
        {
            var client = new AccountInformationReq_OutClient(this.binding, this.endpoint);
            
            if (!String.IsNullOrEmpty(this.username))
            {
                client.ClientCredentials.UserName.UserName = username;
                client.ClientCredentials.UserName.Password = password;
            }

            AccountInformationResp resp = null;

            try
            {
                var req = new AccountInformationReq
                {
                    CADetails = new AccountInformationReqCADetails
                    {
                        ContractAccount = accountNo
                    }
                };

                logger.Trace("[BCRM Request] " + req.Dump());
                resp = client.AccountInformationReq_Out(req);
                logger.Trace("[BCRM Response] " + resp.Dump());
            }
            catch (Exception ex)
            {
                throw new Exception("Some error occured, please try again later.", ex);
            }

            if (resp.StatusMessage != null) // Successful calls do not have StatusMessage
                return null;

            var acc = new AccountInfo(resp);
            return acc;
        }
    }

    public class AccountInfo
    {
        public AccountInfo()
        {
            this.Bills = new List<BillInfo>();
        }

        public AccountInfo(AccountInformationResp resp)
        {
            var ca = resp.CADetails;
            this.AccountNo = ca.ContractAccount;
            this.AccountStatus = ca.AccountStatus;
            this.Name = ca.Name;
            this.RateCategory = ca.RateCategory;
            this.RateCategoryDesc = ca.RateCategoryDesc;
            this.DisconnectionStatus = ca.DisconnectionStatus;
            this.PremiseType = ca.PremiseType;
            this.PremiseTypeDesc = ca.PremiseTypeDesc;
            
            if (resp.AddressDetails != null && resp.AddressDetails.Count() > 0)
            {
                var addressDetail = resp.AddressDetails.First();
                this.Email = addressDetail.EmailID;
                this.MobileNo = addressDetail.MobileNo;
                this.TelephoneNo = addressDetail.Telephone;
            }

            if (resp.BillingHistory != null)
            {
                this.Bills = resp.BillingHistory.OrderByDescending(x => x.PrintDate).Select(x => new BillInfo
                {
                    PrintDocument = x.PrintDocument,
                    PrintDate = ParseDate(x.PrintDate),
                    PostDate = ParseDate(x.PostDate),
                    Amount = String.IsNullOrEmpty(x.Amount) ? 0 : Convert.ToDecimal(x.Amount),
                    Consumption = String.IsNullOrEmpty(x.Consumption) ? 0 : Convert.ToDecimal(x.Consumption)
                });
            }
        }

        public DateTime ParseDate(string dateString)
        {
            var dt = DateTime.MinValue;
            DateTime.TryParseExact(dateString, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dt);
            return dt;
        }

        public string AccountNo { get; set; }
        public string AccountStatus { get; set; }
        public string Name { get; set; }
        public string RateCategory { get; set; }
        public string RateCategoryDesc { get; set; }
        public string DisconnectionStatus { get; set; }
        public string PremiseType { get; set; }
        public string PremiseTypeDesc { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string TelephoneNo { get; set; }
        public IEnumerable<BillInfo> Bills { get; set; }
    }

    public class BillInfo
    {
        public string PrintDocument { get; set; }
        public DateTime PrintDate { get; set; }
        public DateTime PostDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Consumption { get; set; }
    }
}
