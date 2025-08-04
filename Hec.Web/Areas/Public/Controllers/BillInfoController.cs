using Hec.Entities;
using Hec.Integrations;
using Hec.Web.Controllers;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Hec.Web.Areas.Public.Controllers
{
    [Authorize]
    public class BillInfoController : BaseController
    {
        private IBcrmService bcrmService;

        public BillInfoController(HecContext db, IBcrmService bcrmService) : base(db)
        {
            this.bcrmService = bcrmService;
        }

        // GET: BillInfo
        public ActionResult Index()
        {
            var userId = GetCurrentUser().Id;
            ViewBag.ContractAccounts = db.ContractAccounts.Where(x => x.UserId == userId).ToList();
            return View();
        }

        public ActionResult AddAccount()
        {
            ViewBag.HouseCategories = db.HouseCategories.OrderBy(x => x.Sequence).ToList();
            ViewBag.HouseTypes = db.HouseTypes.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult AddAccount(string houseName, string accountNo, string houseType)
        {
            var userId = GetCurrentUser().Id;
            var ca = db.ContractAccounts.Where(x => x.AccountNo == accountNo && x.UserId == userId).FirstOrDefault();
            if (ca != null)
                throw new Exception($"Account No '{accountNo}' already exist.");

            var account = new ContractAccount
            {
                UserId = userId,
                AccountNo = accountNo,
                HouseType = db.HouseTypes.Where(x => x.HouseTypeCode == houseType).FirstOrDefault()
            };

            account.House = new House
            {
                HouseName = houseName,
                HouseType = houseType
            };

            var accountInfo = bcrmService.GetAccountInfo(accountNo);
            if (accountInfo == null)
                return Status(System.Net.HttpStatusCode.NotFound);

            account.CopyBcrmInfo(db, accountInfo, false);

            db.ContractAccounts.Add(account);
            db.SaveChanges();
                
            return Json("");
        }

        public ActionResult DetailsAccount(Guid id)
        {
            var account = db.ContractAccounts.Find(id);
            var premise = db.HouseTypes.Where(x => x.PremiseCode.Equals(account.PremiseType)).FirstOrDefault();
            var model = new AccountDetailModel
            {
                Account = account,
                HouseType = premise,
                Bills = account.GetBills(db, bcrmService),
                HouseName = account.House.HouseName
            };
           
            return View(model);
        }
      

        public ActionResult EditAccount(string houseName, string accountNo)
        {
            var userId = GetCurrentUser().Id;
            var ca = db.ContractAccounts.Where(x => x.AccountNo == accountNo && x.UserId == userId).FirstOrDefault();
            if (ca == null)
                throw new Exception($"No house data found for User ID '{accountNo}' and Account No ");

            ViewBag.ContractAccount = ca;

            //var accountInfo = bcrmService.GetAccountInfo(accountNo);

            return View(ca);
        }


        [HttpPost]
        public ActionResult Update(Guid id, AccountDetailModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = db.ContractAccounts.Find(id);
                entity.House.HouseName = model.HouseName;
                entity.Name = model.Name;
                entity.TelephoneNo = model.TelephoneNo;
                entity.MobileNo = model.MobileNo;
                entity.SerializeData();
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

     
        // GET: BillInfo
        public ActionResult Update()
        {
            return View();
        }


        //
        // Set Default House
        //
        [HttpPost]
        public ActionResult SetDefaultHouse(Guid id)
        {
            if (ModelState.IsValid)
            {
                // Set all user house default false
                var userId = GetCurrentUser().Id;
                var conAccounts = db.ContractAccounts.Where(x => x.UserId == userId).ToList();
                foreach(var caccount in conAccounts)
                {
                    caccount.IsDefault = false;
                }
                db.SaveChanges();

                // Set default for the selected house
                var ca = db.ContractAccounts.Where(x => x.Id == id).FirstOrDefault();
                ca.IsDefault = true;
                db.SaveChanges();
            }
            return Json("success");
        }


        //
        // Remove account
        //
        [HttpPost]
        public ActionResult RemoveAccount(Guid id)
        {
            var account = db.ContractAccounts.Find(id);
            var billings = db.Bills.Where(x => x.ContractAccountId == account.Id).ToList();

            if (billings != null)
            {
                foreach (var bill in billings)
                {
                    db.Bills.Remove(bill);
                }
                db.SaveChanges();
            }

            db.ContractAccounts.Remove(account);
            db.SaveChanges();

            return Json("success");
        }
    }

    public class AccountDetailModel
    {
        public ContractAccount Account { get; set; }
        public HouseType HouseType { get; set; }
        public IEnumerable<Bill> Bills { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string TelephoneNo { get; set; }

        public string HouseName { get; set; }
    }
}