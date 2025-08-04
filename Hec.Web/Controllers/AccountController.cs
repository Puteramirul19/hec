using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Hec.Entities;
using System.Linq;
using System.Security.Claims;
using Hec.Auth;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Hec.Notifications;
using Hec.Settings;
using System.Collections.Generic;
using System.Configuration;
using Autofac;

namespace Hec.Web.Controllers
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class AccountController : Controller
    {
        private const string MSG_LOGIN_ERROR = "Username / Staff number or password is invalid. Please try again.";
        private const string MSG_INPUT_EMPTY = "Username/Staff number and password cannot be empty.";
        private const string MSG_USER_INACTIVE = "User is inactive.";

        //private const string MSG_LOGIN_ERROR = "Name pengguna / nombor pekerja atau kata laluan tidak sah. Sila cuba semula.";
        //private const string MSG_INPUT_EMPTY = "Name pengguna / nombor pekerja dan kata laluan tidak boleh kosong.";
        //private const string MSG_USER_INACTIVE = "Pengguna tidak aktif.";

        private HecContext db;
        private IDirectoryService directoryService;
        private IAuthenticationManager authenticationManager;
        private UserManager<Entities.User> userManager;
        private SignInManager<Entities.User, string> signInManager;
        private SettingsStore settingsStore;
        private IEmailSender emailSender;

        public AccountController(HecContext db, IDirectoryService directoryService,
                                 IAuthenticationManager authenticationManager, UserManager<User> userManager,
                                 SignInManager<User, string> signInManager, SettingsStore settingsStore, IEmailSender emailSender)
            : base()
        {
            this.db = db;
            this.directoryService = directoryService;
            this.authenticationManager = authenticationManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.settingsStore = settingsStore;
            this.emailSender = emailSender;
        }

        public ActionResult Login(string msg)
        {
            var model = new LoginViewModel
            {
                UserName = TempData["UserName"] as string

            };

            var tmpError = TempData["ErrorMessage"] as string;

            if (!String.IsNullOrEmpty(tmpError))
            {
                ViewBag.ErrorMessage = tmpError;
            }
            else if (!String.IsNullOrEmpty(msg))
                ViewBag.ErrorMessage = msg;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            using (var s = new HecContext())
            {
                bool succeed = false;

                if (String.IsNullOrEmpty(model.UserName) || String.IsNullOrEmpty(model.Password))
                {
                    ViewBag.ErrorMessage = MSG_INPUT_EMPTY;
                    return View(model);
                }

                var user = userManager.FindByName(model.UserName);

                if (user == null)
                {
                    ViewBag.ErrorMessage = MSG_LOGIN_ERROR;
                    return View(model);
                }
                else
                {
                    if (!user.IsActive)
                    {
                        ViewBag.ErrorMessage = MSG_USER_INACTIVE;
                        return View(model);
                    }

                    if (user.LoginType == LoginType.ActiveDirectory)
                    {
                        var dirUser = directoryService.Authenticate(model.UserName, model.Password);
                        if (dirUser != null)
                        {
                            succeed = true;

                            // Update user
                            //TEMP HACK: Don't update user for time being
                            //user.FullName = dirUser.FullName;
                            //user.Email = dirUser.Email;
                            //user.PhoneNumber = dirUser.PhoneNumber;
                            //user.Photo = dirUser.Photo;                        
                        }
                    }
                    else
                    {
                        succeed = await userManager.CheckPasswordAsync(user, model.Password);
                    }
                }

                if (succeed)
                {
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    identity.AddClaim(new Claim("NameAndRole", String.Format("{0} ({1})", user.FullName, user.Role.Name)));

                    var authProp = new AuthenticationProperties()
                    {
                        IsPersistent = false
                    };
                    authenticationManager.SignIn(authProp, identity);

                    // Check if user first time login
                    var FirstTimeLogin = false;
                    if (user.LastLogin.ToString() == "01/01/0001 00:00:00")
                    {
                        FirstTimeLogin = true;
                    }
                    TempData["FirstTimeLogin"] = FirstTimeLogin;

                    user.LastLogin = DateTime.Now;

                    await db.SaveChangesAsync();

                    userManager.Update(user);

                    if (user.Role.Name == "Administrator")
                    {
                        return RedirectToAction("Index", new { area = "Admin", controller = "Home" });
                    }
                    else
                    {
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", new { area = "Public", controller = "Home" });
                        }
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(ViewBag.ErrorMessage))
                        ViewBag.ErrorMessage = MSG_LOGIN_ERROR;
                }

                return View(model);
            }
        }

        public ActionResult Logout()
        {
            authenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult PublicLogout()
        {
            authenticationManager.SignOut();
            return RedirectToAction("End", "Session");
        }

        public ActionResult CheckComplaint()
        {
            return View();
        }
    }
}
