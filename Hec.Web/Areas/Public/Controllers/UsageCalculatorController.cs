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
        /// Read Tariff Block - UPDATED FOR NEW TARIFF STRUCTURE JULY 2025
        /// OLD: Read Tariff Block system - kept for backward compatibility
        /// NEW: Returns new component-based tariff structure
        /// </summary>
        /// <returns>TariffBlock</returns>
        public ActionResult ReadTariff()
        {
            // OLD CODE - COMMENTED OUT BUT KEPT FOR REFERENCE
            // var list = db.Tariffs.OrderBy(x => x.Sequence).ToList();
            // var count = list.Count();
            // return Json(new
            // {
            //     tiers = list.Take(count - 1).Select(x => new { boundary = x.BoundryTier, rate = x.TariffPerKWh }),
            //     remaining = list[count - 1].TariffPerKWh
            // });

            // NEW TARIFF STRUCTURE EFFECTIVE JULY 1, 2025
            // Component-based billing instead of tier system
            return Json(new
            {
                // NEW STRUCTURE COMPONENTS
                generationChargeUnder1500 = 0.2703m, // 27.03 sen/kWh
                generationChargeOver1500 = 0.3703m,  // 37.03 sen/kWh
                capacityCharge = 0.0455m,  // 4.55 sen/kWh
                networkCharge = 0.1285m,   // 12.85 sen/kWh
                retailCharge = 10.00m,     // RM10/month
                retailChargeExemptionThreshold = 600m, // Exempted for usage under 600 kWh
                energyEfficiencyThreshold = 1000m,     // For usage under 1000 kWh
                maxEnergyEfficiencyIncentive = 0.25m,  // Up to 25 sen/kWh discount
                generationChargeThreshold = 1500m,     // Different generation charge above 1500 kWh
                averageCostUnder1500 = 0.4443m, // 27.03 + 4.55 + 12.85 = 44.43 sen/kWh
                averageCostOver1500 = 0.5443m,  // 37.03 + 4.55 + 12.85 = 54.43 sen/kWh
                useNewTariffStructure = true,

                // OLD STRUCTURE - KEPT FOR BACKWARD COMPATIBILITY
                tiers = new[] {
                    new { boundary = 200, rate = 0.218 },
                    new { boundary = 100, rate = 0.334 },
                    new { boundary = 300, rate = 0.516 },
                    new { boundary = 300, rate = 0.546 }
                },
                remaining = 0.571
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