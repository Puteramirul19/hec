using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Hec.Entities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using Hec.Integrations;
using NLog;

namespace Hec.Web.Controllers
{
    public class SessionController : BaseController
    {
        private IAuthenticationManager authenticationManager;
        private UserManager<Entities.User> userManager;
        private SignInManager<Entities.User, string> signInManager;
        private IBcrmService bcrmService;
        private ISspService sspService;

        public SessionController(HecContext db, IAuthenticationManager authenticationManager, 
                                UserManager<User> userManager, SignInManager<User, string> signInManager,
                                IBcrmService bcrmService,
                                ISspService sspService) 
            : base(db)
        {
            this.authenticationManager = authenticationManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.bcrmService = bcrmService;
            this.sspService = sspService;
        }

        public static Logger logger = LogManager.GetLogger("integration");
        
        /// <summary>
        /// Entry point for SSP integration.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Start(string USERTOKEN, string USERID, string USERNAME, 
                                              string FULLNAME, string EMAIL, string CA_NO_LIST)
        {
            logger.Trace($"[SSP Login] USERID: '{USERID}', USERNAME: '{USERNAME}', USERTOKEN: '{USERTOKEN}', FULLNAME: '{FULLNAME}', EMAIL: '{EMAIL}', CA_NO_LIST: '{CA_NO_LIST}'");

            //
            // Verify token back against SSP to make sure we really got here from SSP.
            //
            if (!sspService.VerifyToken(USERNAME, USERTOKEN))
                throw new Exception("You must login from MyTNB (invalid MyTNB credentials).");

            //
            // Create user or update
            //
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == USERNAME);
            if (user == null)
            {
                var newid = Guid.NewGuid().ToString();
                user = new User
                {
                    Id = USERNAME,
                    SecurityStamp = USERID,
                    PasswordHash = new Microsoft.AspNet.Identity.PasswordHasher().HashPassword(newid),
                    UserName = USERNAME,
                    FullName = FULLNAME,
                    Email = EMAIL,
                    LoginType = LoginType.Ssp,
                    IsActive = true,
                    RoleId = db.Roles.First(x => x.Name == RoleNames.Public).Id,
                    SspUserId = USERID,
                    SspUserToken = USERTOKEN
                };

                db.Users.Add(user);
            }
            else
            {
                user.FullName = FULLNAME;
                user.Email = EMAIL;
                user.SspUserToken = USERTOKEN;
                db.SetModified(user);
            }
            await db.SaveChangesAsync();

            string firstNo = null;

            var houseTypes = db.HouseTypes.ToList();

            //
            // Save contract accounts
            //
            foreach (var ca in CA_NO_LIST
                                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(x => x.Trim()))
            {
                var splitted = ca.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                var accountNo = splitted[0].Trim();
                var accountName = (splitted.Length > 1) ? splitted[1].Trim() : null;
                
                if (firstNo == null) firstNo = accountNo;

                var accountInfo = bcrmService.GetAccountInfo(accountNo);
                if (accountInfo == null)
                    continue;

                var houseType = houseTypes.FirstOrDefault(x => x.PremiseCode == accountInfo.PremiseType);
                if (houseType == null)
                    continue; // If house type not found (non-domestic), we dont add the account.
                
                var contractAccount = db.ContractAccounts.FirstOrDefault(x => x.UserId == user.Id && x.AccountNo == accountNo);

                if (contractAccount == null)
                {
                    // New account
                    contractAccount = new ContractAccount
                    {
                        UserId = user.Id,
                        AccountNo = accountNo,
                        IsFromSsp = true
                    };

                    // Save it
                    db.ContractAccounts.Add(contractAccount);
                }
                else
                {
                    db.SetModified(contractAccount);
                }

                // Set house type
                contractAccount.SetHouseType(houseType);

                // Copy BCRM info
                contractAccount.CopyBcrmInfo(db, accountInfo, false);
                                
                // Set or Update house name
                if (accountName != null)
                {
                    contractAccount.House.HouseName = accountName;
                }

                // Remember to serialize before save
                contractAccount.SerializeData();
            }
            await db.SaveChangesAsync();
            
            var userAccounts = db.ContractAccounts
                                .Include(x => x.HouseType)
                                .Where(x => x.UserId == user.Id)
                                .ToList();

            if (userAccounts.Count() > 0)
            {
                //
                // If there's no default account, set fist one we get from SSP
                // REVIEW: What happens if existing user dont have default account? Is it possible?
                if (!userAccounts.Any(x => x.IsDefault == true))
                {
                    var firstAccount = userAccounts.FirstOrDefault(x => x.AccountNo == firstNo);
                    if (firstAccount != null)
                    {
                        firstAccount.IsDefault = true;
                        db.SetModified(firstAccount);
                        await db.SaveChangesAsync();
                    }
                }
            }

            //
            // Log in user
            //
            authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim("NameAndRole", String.Format("{0} ({1})", user.FullName, user.Role.Name)));
            identity.AddClaim(new Claim("Token", USERTOKEN));

            var authProp = new AuthenticationProperties()
            {
                IsPersistent = false
            };
            authenticationManager.SignIn(authProp, identity);

            // Add cookie to track public user
            // Check this cookie in Global.Application_AuthenticateRequest
            Response.Cookies.Add(new HttpCookie("PublicUser")
            {
                Expires = DateTime.MaxValue
            });

            // Check if user first time login
            var FirstTimeLogin = false;
            if (user.LastLogin.ToString() == "01/01/0001 00:00:00")
            {
                FirstTimeLogin = true;
            }
            TempData["FirstTimeLogin"] = FirstTimeLogin;
            Session["FullName"] = user.FullName;

            user.LastLogin = DateTime.Now;
            user.LoginCount += 1;

            await db.SaveChangesAsync();

            // Redirect to public home
            return RedirectToAction("Index", new { area = "Public", controller = "Home" });
        }

        public ActionResult End(string t)
        {
            ViewBag.Timeout = !String.IsNullOrEmpty(t);
            return View();
        }
    }
}