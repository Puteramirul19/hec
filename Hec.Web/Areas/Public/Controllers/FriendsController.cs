using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Hec.Entities;
using Hec.Integrations;
using System.Threading.Tasks;
using Hangfire;
using Hec.Notifications;
using Hec.Settings;

namespace Hec.Web.Areas.Public.Controllers
{
    public class FriendIndexModel
    {
        public User CurrUser { get; set; }
        public int CurrentRank { get; set; }
        public int PrevRank { get; set; }

        public bool RankUp {  get {  return CurrentRank < PrevRank; } }
        public bool RankDown {  get {  return CurrentRank > PrevRank; } }

        public int FriendInviteCount { get; set; }
    }

    public class FriendInvitationsModel
    {
        public User CurrUser { get; set; }
        public IEnumerable<Friendship> Friendships { get; set; }
    }

    public class FriendsController : Web.Controllers.BaseController
    {
        private IBcrmService bcrmService;
        private IBackgroundJobClient backgroundJob;

        public FriendsController(HecContext db, IBcrmService bcrmService, IBackgroundJobClient backgroundJob) : base(db)
        {
            this.bcrmService = bcrmService;
            this.backgroundJob = backgroundJob;
        }

        public ActionResult Index()
        {
            var now = DateTime.Now;

            var currUser = GetCurrentUser();
            var thisMonthRanking = GetAccountRanks(now).FirstOrDefault(x => x.UserId == currUser.Id);
            var lastMonthRanking = GetAccountRanks(now.AddMonths(-1)).FirstOrDefault(x => x.UserId == currUser.Id);

            var friendInviteCount = db.Friendships.Count(x => x.IsAccepted == false && x.InviteeId == currUser.Id);

            return View(new FriendIndexModel
            {
                CurrUser = currUser,
                CurrentRank = (thisMonthRanking == null) ? 0 : thisMonthRanking.Rank,
                PrevRank = (lastMonthRanking == null) ? 0 : lastMonthRanking.Rank,
                FriendInviteCount = friendInviteCount
            });
        }

        //-------------------------------------------------------------------------------- 
        //  Invitations
        //--------------------------------------------------------------------------------
        
        [HttpPost]
        public ActionResult InviteFriend(string email, string name, string accountNo, string icNo)
        {
            if (String.IsNullOrEmpty(email)
                || String.IsNullOrEmpty(name)
                || String.IsNullOrEmpty(accountNo))
                return Json(new { status = false, message = "Email, Name, and Account Number cannot be empty." });

            email = email.Trim();
            accountNo = accountNo.Trim().Replace(" ", "");

            var invitee = db.Users.FirstOrDefault(x => x.Id == email); // SSP users use email as ID
            if (invitee == null)
                return Json(new { status = false, message = "No user found with email " + email });

            var currUser = GetCurrentUser();

            // Check if there's existing invite. Just return if any.
            if (db.Friendships.Any(x => x.InviterId == currUser.Id && x.InviteeId == invitee.Id))
                return Json(new { status = true, message = "" });

            // Check if the other way around. If Accepted, just return.
            if (db.Friendships.Any(x => x.InviteeId == currUser.Id && x.InviterId == invitee.Id && x.IsAccepted == true))
                return Json(new { status = true, message ="" });

            var defaultAccount = invitee.GetDefaultAccount(db);
            if (defaultAccount == null || defaultAccount.AccountNo != accountNo)
                return Json(new { status = false, message = "No user found with account number " + accountNo });

            // No accepted invite found.
            var invitation = new Friendship
            {
                InviterId = currUser.Id,
                InviteeId = invitee.Id,
                IsAccepted = false
            };
            db.Friendships.Add(invitation);
            db.SaveChanges();

            // Email notification
            backgroundJob.Enqueue<Notifier>(x => x.FriendInviteSubmitted(invitation.Id));

            //return Json("");
            return Json(new { status = true, message = "" });
        }

        public ActionResult Invitations()
        {
            var currUser = GetCurrentUser();
            var friendships = db.Friendships
                                .Include(x => x.Inviter)
                                .Include(x => x.Invitee)
                                .Where(x => x.InviteeId == currUser.Id || x.InviterId == currUser.Id)
                                .ToList();
            return View(new FriendInvitationsModel
            {
                CurrUser = currUser,
                Friendships = friendships
            });
        }

        public ActionResult AcceptInvitation(Guid id /*Friendship.Id*/)
        {
            var friendship = db.Friendships
                            .Include(x => x.Invitee)
                            .Include(x => x.Inviter)
                            .FirstOrDefault(x => x.Id == id);

            // If there are other accepted friendship between these 2, just delete this invite.
            if (db.Friendships.Any(x =>
                x.IsAccepted == true &&
                (x.InviterId == friendship.InviterId && x.InviteeId == friendship.InviteeId)
                || (x.InviteeId == friendship.InviterId && x.InviterId == friendship.InviteeId)
                ))
            {
                db.SetDeleted(friendship);
            }
            else
            {
                friendship.Invitee.SetBaseAmount(db, bcrmService);
                friendship.Inviter.SetBaseAmount(db, bcrmService);
                friendship.IsAccepted = true;
                db.SetModified(friendship);
            }

            db.SaveChanges();

            // Email notification
            backgroundJob.Enqueue<Notifier>(x => x.FriendInviteAccepted(friendship.Id));

            TempData["InfoMessage"] = "You have accepted the invitation.";
            return RedirectToAction("Invitations");
        }

        [HttpPost]
        public async Task<ActionResult> RemoveFriend(string userId)
        {
            var currUser = GetCurrentUser();
            var friendships = db.Friendships.Where(x => (x.InviterId == currUser.Id && x.InviteeId == userId)
                                                     || (x.InviteeId == currUser.Id && x.InviterId == userId));
            foreach (var f in await friendships.ToListAsync())
                db.SetDeleted(f);

            await db.SaveChangesAsync();

            return Json("");
        }
        
        //-------------------------------------------------------------------------------- 
        //  Friends and Bills
        //--------------------------------------------------------------------------------
        
        public ActionResult GetFriends()
        {
            var friends = GetFriendAccounts();
            return Json(friends);
        }

        private IEnumerable<FriendAccount> GetFriendAccounts(User user = null)
        {
            var currUser = user ?? GetCurrentUser();
            var userId = currUser.Id;

            var friendships = db.Friendships
                                .Include(x => x.Invitee)
                                .Include(x => x.Inviter) 
                                .Where(x => x.IsAccepted == true && (x.InviteeId == userId || x.InviterId == userId))
                                .ToList();

            var friendAccounts = new List<FriendAccount>();

            Action<User> addFriendAccount = (friendUser) =>
            {
                var defAccount = friendUser.GetDefaultAccount(db);
                if (defAccount != null)
                {
                    friendAccounts.Add(new FriendAccount
                    {
                        ContractAccountId = defAccount.Id,
                        AccountNo = defAccount.AccountNo,
                        UserId = friendUser.Id,
                        FullName = friendUser.FullName,
                        HouseType = db.HouseTypes.Where(x => x.HouseTypeCode == defAccount.House.HouseType).FirstOrDefault(),
                        Bill = defAccount.GetBills(db, bcrmService).ToArray()
                    });

                }
            };

            foreach (var fs in friendships)
            {
                if (fs.InviteeId != userId)
                    addFriendAccount(fs.Invitee);

                if (fs.InviterId != userId)
                    addFriendAccount(fs.Inviter);
            }

            return friendAccounts.Distinct();
        }

        public ActionResult GetCurrAccount()
        {
            var currUser = GetCurrentUser();
            var account = currUser.GetDefaultAccount(db);
            if (account == null)
                throw new IdNotFoundException<ContractAccount>(currUser.Id);

            var bills = account.GetBills(db, bcrmService);
            var billQuarterly = bills.OrderByDescending(x => x.MonthYear).Take(3);
            var billHalfYear = bills.OrderByDescending(x => x.MonthYear).Take(6);
            var billPerYear = bills.OrderByDescending(x => x.MonthYear).Take(12);

            // If current month same with last bill date, then take last 3 previous bill
            var billPrev3Month = bills.OrderByDescending(x => x.MonthYear).Take(4);
            var lastBillDate = billPrev3Month.First().PrintDate;
            var isCurrentMonth = DateTime.Now.Month == lastBillDate.Month && DateTime.Now.Year == lastBillDate.Year;
            if (isCurrentMonth)
                billPrev3Month = billPrev3Month.Skip(1);
            else
                billPrev3Month = billPrev3Month.Take(billPrev3Month.Count() - 1);

            return Json(new {
                account = account,
                bills = billHalfYear,
                billQuarterly = billQuarterly,
                billPerYear = billPerYear,
                billPrev3Month = billPrev3Month
            });
        }

        public ActionResult GetBills(Guid contractAccountId)
        {
            var contractAccount = db.ContractAccounts.FirstOrDefault(x => x.Id == contractAccountId);
            var account = contractAccount.User.GetDefaultAccount(db);
            if (account == null)
                throw new IdNotFoundException<ContractAccount>(contractAccountId);

            var billHalfYear = account.GetBills(db, bcrmService).OrderByDescending(x => x.MonthYear).Take(6);

            // If current month same with last bill date, then take last 3 previous bill
            var billPrevHalfYear = account.GetBills(db, bcrmService).OrderByDescending(x => x.MonthYear).Take(7);
            var lastBillDate = billPrevHalfYear.First().PrintDate;
            var isCurrentMonth = DateTime.Now.Month == lastBillDate.Month && DateTime.Now.Year == lastBillDate.Year;
            if (isCurrentMonth)
                billPrevHalfYear = billPrevHalfYear.Skip(1);
            else
                billPrevHalfYear = billPrevHalfYear.Take(billPrevHalfYear.Count() - 1);

            return Json(new { account = account, bills = billHalfYear, billPrevHalfYear = billPrevHalfYear });
        }

        public ActionResult GetAverageHouseType(string houseTypeCode)
        {
            var houseType = db.HouseTypes.FirstOrDefault(x => x.HouseTypeCode == houseTypeCode);
            if (houseType == null)
                throw new IdNotFoundException<HouseType>(houseTypeCode);

            return Json(houseType);
        }

        //-------------------------------------------------------------------------------- 
        //  Ranking
        //-------------------------------------------------------------------------------- 

        public ActionResult GetRankings(int month)
        {
            var currUser = GetCurrentUser();
            var today = DateTime.Today;
            var dt = new DateTime((today.Month < month) ? today.Year - 1 : today.Year, month, 1);
            var thisMonthRankings = GetAccountRanks(dt);
            var lastMonthRankings = GetAccountRanks(dt.AddMonths(-1));

            var myRankingThisMonth = thisMonthRankings.FirstOrDefault(x => x.UserId == currUser.Id);
            var myRankingLastMonth = lastMonthRankings.FirstOrDefault(x => x.UserId == currUser.Id);

            var myRankThisMonth = (myRankingThisMonth == null) ? 0 : myRankingThisMonth.Rank;
            var myRankLastMonth = (myRankingLastMonth == null) ? 0 : myRankingLastMonth.Rank;

            if (myRankingThisMonth != null)
                myRankingThisMonth.Difference = myRankLastMonth - myRankThisMonth;

            return Json(thisMonthRankings);
        }

        private IEnumerable<AccountRank> GetAccountRanks(DateTime dt)
        {
            var currUser = GetCurrentUser();
            var userId = currUser.Id;

            var friends = GetFriendAccounts(currUser);

            var ranks = new List<AccountRank>();

            Action<ContractAccount> addRank = (account) =>
            {
                if (account != null)
                {
                    var bills = account.GetBills(db, bcrmService);
                    var bill = bills.FirstOrDefault(x => x.MonthYear == Bill.BuildMonthYear(dt));
                    if (bill != null)
                    {
                        ranks.Add(new AccountRank
                        {
                            ContractAccountId = account.Id,
                            AccountNo = account.AccountNo,
                            UserId = account.UserId,
                            FullName = account.User.FullName,
                            Photo = account.User.Photo,
                            BaseConsumption = account.User.BaseConsumption ?? 0,
                            BillConsumption = bill.Consumption
                        });
                    }
                }
            };

            // Calculate savings for friends
            foreach (var friend in friends)
            {
                addRank(db.ContractAccounts
                            .Include(x => x.User)
                            .FirstOrDefault(x => x.Id == friend.ContractAccountId));
            }

            addRank(currUser.GetDefaultAccount(db));

            if (ranks.Count() > 0)
            {
                //ranks = ranks.OrderByDescending(x => x.SavingPerDay).ToList();
                ranks = ranks.OrderByDescending(x => x.Saving).ToList();

                var order = 1;
                foreach (var rank in ranks)
                {
                    rank.Rank = order;
                    order++;
                }
            }

            return ranks;
        }
    }

    public class FriendAccount
    {
        public Guid ContractAccountId { get; set; }
        public string UserId { get; set; }
        public string AccountNo { get; set; }
        public string FullName { get; set; }
        public HouseType HouseType { get; set; }
        public Array Bill { get; set; }
    }

    public class AccountRank
    {
        public Guid ContractAccountId { get; set; }
        public string AccountNo { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Photo { get; set; }
        public decimal BaseConsumption { get; set; }
        public decimal BillConsumption { get; set; }
        public decimal Saving { get { return BillConsumption - BaseConsumption; } }
        public decimal SavingPerDay { get { return Saving / 30; } }
        public decimal SavingPercent { get { return (BaseConsumption <= 0) ? 0 : Saving / BaseConsumption; } }
        public int Rank { get; set; }
        public int Difference { get; set; }
    }
}