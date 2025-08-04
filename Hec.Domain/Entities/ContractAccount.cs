using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hec.Integrations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hec.Entities
{
    public class ContractAccount : Entity
    {
        [StringLength(15)]
        public string AccountNo { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public bool IsFromSsp { get; set; }
        public bool IsDefault { get; set; }

        //-------------------------------------------------------------------------------- 
        // BCRM data

        public string AccountStatus { get; set; }
        public string Name { get; set; }
        public string RateCategory { get; set; }
        public string RateCategoryDesc { get; set; }
        public string DisconnectionStatus { get; set; }
        public string PremiseType { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string TelephoneNo { get; set; }
        
        public decimal LatestBillAmount { get; set; }
        public int LatestBillMonthYear { get; set; }

        //-------------------------------------------------------------------------------- 

        public HouseType HouseType { get; set; }
        private House house = null;

        [NotMapped]
        public House House
        {
            get
            {
                if (house == null)
                {
                    if (String.IsNullOrEmpty(HouseData))
                        house = new House();
                    else
                        house = JsonConvert.DeserializeObject<House>(this.HouseData);
                }
                return house;
            }

            set
            {
                house = value;
                house.AccountNo = this.AccountNo;
                HouseData = JsonConvert.SerializeObject(value, jsonSerializerSettings);
            }
        }

        public void CopyBcrmInfo(HecContext db, AccountInfo accountInfo, bool copyHouseType)
        {
            // Update properties
            //
            this.AccountStatus = accountInfo.AccountStatus;
            this.Name = accountInfo.Name;
            this.RateCategory = accountInfo.RateCategory;
            this.RateCategoryDesc = accountInfo.RateCategoryDesc;
            this.DisconnectionStatus = accountInfo.DisconnectionStatus;
            this.PremiseType = accountInfo.PremiseType;
            this.Email = accountInfo.Email;
            this.MobileNo = accountInfo.MobileNo;
            this.TelephoneNo = accountInfo.TelephoneNo;

            //
            // Copy house type
            //
            if (copyHouseType)
            {
                var houseType = db.HouseTypes.FirstOrDefault(x => x.PremiseCode == accountInfo.PremiseType);
                if (this.HouseType == null && houseType != null)
                {
                    this.HouseType = houseType;
                    this.House.HouseType = houseType.HouseTypeCode;
                    this.SerializeData();
                }
            }

            //
            // Update bills
            //
            var existingBills = db.Bills.OrderByDescending(x => x.PrintDate)
                                    .Where(x => x.ContractAccountId == this.Id)
                                    .ToList();
            var latestBill = existingBills.FirstOrDefault();

            foreach (var billInfo in accountInfo.Bills.OrderByDescending(x => x.PrintDate))
            {
                if (!existingBills.Any(x => x.PrintDate == billInfo.PrintDate))
                {
                    var bill = new Bill
                    {
                        ContractAccountId = this.Id,
                    };

                    bill.CopyBrcmInfo(billInfo);
                    db.Bills.Add(bill);
                    
                    if (latestBill == null || billInfo.PrintDate > latestBill.PrintDate)
                        latestBill = bill;
                }
            }

            if (latestBill != null)
            {
                this.LatestBillAmount = latestBill.Amount;
                this.LatestBillMonthYear = latestBill.MonthYear;
            }
        }

        private JsonSerializerSettings jsonSerializerSettings
        {
            get
            {
                var settings = new JsonSerializerSettings();
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Error;
                settings.Formatting = Formatting.Indented;
                settings.Converters.Add(new StringEnumConverter());
                return settings;
            }
        }

        public string HouseData { get; set; }

        /// <summary>
        /// Always call this before saving ContractAccount.
        /// </summary>
        public void SerializeData()
        {
            HouseData = JsonConvert.SerializeObject(House, jsonSerializerSettings);
        }

        //-------------------------------------------------------------------------------- 
        
        public void SetHouseType(HouseType houseType)
        {
            // Set house type
            this.HouseType = houseType;
            this.House.HouseType = houseType.HouseTypeCode;
        }

        public IEnumerable<Bill> GetBills(HecContext db, IBcrmService bcrmService)
        {
            var bills = db.Bills.OrderByDescending(x => x.MonthYear)
                            .Where(x => x.ContractAccountId == this.Id);

            if (bills.Count() == 0 || bills.First().MonthYear < Bill.BuildMonthYear(DateTime.Now))
            {
                var accountInfo = bcrmService.GetAccountInfo(this.AccountNo);
                if (accountInfo != null)
                    this.CopyBcrmInfo(db, accountInfo, true);

                db.SaveChanges();
            }

            return db.Bills.OrderByDescending(x => x.MonthYear)
                            .Where(x => x.ContractAccountId == this.Id)
                            .ToList();
        }
    }
}
