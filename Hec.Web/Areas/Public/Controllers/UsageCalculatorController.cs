using Hec.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using Microsoft.Ajax.Utilities;

namespace Hec.Web.Areas.Public.Controllers
{
    public class TipsList
    {
        public Guid Id { get; set; }
        public string ApplianceName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DoneCount { get; set; }
        public UserTipStatus? Status { get; set; }
        public decimal Watt { get; set; }
    }

    public class Top5Appliance
    {
        public string category { get; set; }
        public decimal value { get; set; }
    }

    public class UsageCalculatorController : Web.Controllers.BaseController
    {
        public UsageCalculatorController(HecContext db) : base(db)
        {
        }

        public ActionResult Index()
        {
            ViewBag.HouseCategories = db.HouseCategories.OrderBy(x => x.Sequence).ToList();
            ViewBag.HouseTypes = db.HouseTypes.OrderBy(x => x.Sequence).ToList();
            ViewBag.Appliances = db.Appliances.ToList();
            ViewBag.AccountNo = "";

            return View();
        }

        public ActionResult Account(string ca)
        {
            ViewBag.HouseCategories = db.HouseCategories.OrderBy(x => x.Sequence).ToList();
            ViewBag.HouseTypes = db.HouseTypes.ToList();
            ViewBag.Appliances = db.Appliances.ToList();
            ViewBag.AccountNo = ca;

            return View("Index");
        }

        /// <summary>
        /// Get House data from Contract Account
        /// </summary>
        /// <param name="id">id is ContractAccount.AccountNo</param>
        /// <returns>House data</returns>
        public async Task<ActionResult> ReadHouseForAccountNo(string userId, string accountNo)
        {
            var ca = await db.ContractAccounts.FirstOrDefaultAsync(x => x.UserId == userId && x.AccountNo == accountNo);
            if (ca == null)
                throw new Exception($"No house data found for User ID '{userId}' and Account No '{accountNo}'");

            return Content(ca.HouseData, "application/json");
            //return Json(ca.House);
        }

        /// <summary>
        /// Save House data into Contract Account
        /// </summary>
        /// <param name="id">id is ContractAccount.AccountNo</param>
        /// <param name="house">House data</param>
        /// <returns>Nothing</returns>
        public async Task<ActionResult> UpdateHouseForAccountNo(string userId, string accountNo, House house)
        {
            var ca = await db.ContractAccounts.FirstOrDefaultAsync(x => x.UserId == userId && x.AccountNo == accountNo);
            if (ca == null)
                throw new Exception($"No house data found for User ID '{userId}' and Account No '{accountNo}'");

            ca.House = house;
            ca.SerializeData();
            await db.SaveChangesAsync();

            return Json("");
        }

        /// <summary>
        /// Get random usage energy tips
        /// </summary>
        /// <param name="house">house is houseData</param>
        /// <returns>Energy tips</returns>
        public async Task<ActionResult> ReadEnergyTips(House house, List<Top5Appliance> top5appliance)
        {
            // Get random appliance tips
            List<TipsList> energyTips = new List<TipsList>();
            if (top5appliance == null)
            {
                return Json(energyTips); ;
            }
            foreach (var appl in top5appliance)
            {
                var app = await db.Appliances.Where(x => x.Name == appl.category).FirstOrDefaultAsync();
                var tip = db.Tips.Where(t => t.TipCategoryId == app.TipCategoryId).OrderBy(x => Guid.NewGuid()).FirstOrDefault();

                if (tip != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        var user = GetCurrentUser();
                        var userTip = db.UserTips.Where(ut => ut.TipId == tip.Id && ut.UserId == user.Id).FirstOrDefault();
                        energyTips.Add(new TipsList()
                        {
                            Id = tip.Id,
                            ApplianceName = app.Name,
                            Title = tip.Title,
                            Description = tip.Description,
                            DoneCount = tip.DoneCount,
                            Status = (userTip == null) ? (UserTipStatus?)null : userTip.Status,
                            Watt = appl.value
                        });
                    }
                    else
                    {
                        energyTips.Add(new TipsList()
                        {
                            Id = tip.Id,
                            ApplianceName = app.Name,
                            Title = tip.Title,
                            Description = tip.Description,
                            DoneCount = tip.DoneCount,
                            Status = (UserTipStatus?)null,
                            Watt = appl.value
                        });
                    }
                }
            }

            // Sort by highest watt
            var usageTips = energyTips.OrderByDescending(o => o.Watt).ToList();

            return Json(usageTips);
        }


        /// <summary>
        /// Read Tariff Block
        /// </summary>
        /// <returns>TariffBlock</returns>
        public ActionResult ReadTariff()
        {
            var list = db.Tariffs.OrderBy(x => x.Sequence).ToList();
            var count = list.Count();

            return Json(new
            {
                tiers = list.Take(count - 1).Select(x => new { boundary = x.BoundryTier, rate = x.TariffPerKWh }),
                remaining = list[count - 1].TariffPerKWh
            });
        }

        /// <summary>
        /// Read House Type
        /// </summary>
        /// <returns>PremiseType</returns>
        public ActionResult GetHouseType(string houseType)
        {
            var houseTypes = db.HouseTypes.Where(x => x.HouseTypeCode == houseType).FirstOrDefault();
            var houseCategories = db.HouseCategories.Where(x => x.Id == houseTypes.HouseCategoryId).FirstOrDefault();

            return Json(new { houseTypes = houseTypes, houseCategories = houseCategories });
        }
    }
}