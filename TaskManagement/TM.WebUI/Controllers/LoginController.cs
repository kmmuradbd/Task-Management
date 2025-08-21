using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TM.Infrastructure;
using TM.Service.Interface;
using TM.Service.ViewModel;

namespace TM.WebUI.Controllers
{
    public class LoginController : Controller
    {
        protected readonly IUserService AppUser;
        public LoginController(IUserService user)
        {
            this.AppUser = user;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            bool isRemember = false;
           
            if (AppUser.Login(username, password))
            {

                var userInfo = AppUser.Get(username);
                string[] roles = AppUser.GetUserRoles(userInfo.UserRoleMasterId);
                var wsIP = Common.GetWorkstationIP();
                string basicTicket = TMIdentity.CreateBasicTicket(
                                                                    username,
                                                                    userInfo.FullName
                                                                 );
                string roleTicket = TMIdentity.CreateRoleTicket(roles);
                int timeOut = Convert.ToInt32(new AppSettingsReader().GetValue("COOKIE_TIMEOUT", typeof(string)));
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, FormsAuthentication.FormsCookieName, DateTime.Now, DateTime.Now.AddMinutes(timeOut), isRemember, basicTicket);
                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                HttpContext.Application["BasicTicket" + username] = basicTicket;
                HttpContext.Application["RoleTicket" + username] = roleTicket;
                Session["userName"] = username;
                Session["name"] = userInfo.FullName;

                var logPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/Login-.txt");

                var log = new LoggerConfiguration()
                    .WriteTo.File(
                        logPath,
                        rollingInterval: RollingInterval.Day, // creates new file per day
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                    .CreateLogger();

                log.Information("User {User} logged in from IP {IP} at {Time}", username, wsIP, DateTime.Now);


                return Redirect("/Home");

            }
            else
            {
                ViewBag.ErrorText = "Invalid username of password.";
            }
            return View();
        }

        #region Logout
        public ActionResult Logout()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null && authCookie.Value != "")
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                TMIdentity identity = new TMIdentity(ticket.UserData);
                authCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Response.Cookies.Add(authCookie);
                HttpContext.Application["BasicTicket" + identity.Id] = null;
                HttpContext.Application["RoleTicket" + identity.Id] = null;
            }
            return Redirect("/");
        }
        #endregion

        #region ChangePassword
        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public JavaScriptResult ChangePassword(UserViewModel user)
        {
            try
            {
                TMIdentity identity = (TMIdentity)Thread.CurrentPrincipal.Identity;
                user.Id = identity.Name;
                AppUser.ChangePassword(user);
                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')", "Password change successfull.", "success", "redirect", "/Admin/Home"));
            }
            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')", ex.Message, "failure"));
            }
        }
        #endregion

    }
}