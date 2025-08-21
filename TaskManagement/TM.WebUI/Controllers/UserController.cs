using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TM.Service.Interface;
using TM.Service.ViewModel;

namespace TM.WebUI.Controllers
{
    public class UserController : Controller
    {
        protected readonly IUserService AppUser;
        public UserController(IUserService userService)
        {
            this.AppUser = userService;
        }
        // GET: User
        public ActionResult Index()
        {
            try
            {
                IEnumerable<UserViewModel> users = AppUser.GetAll();
                return View(users);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region ddl
        public JsonResult GetUserRoleMasterList()
        {
            return Json(new SelectList(AppUser.GetUserRoleMasterList(), "Value", "Text"), JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JavaScriptResult Create(UserViewModel user)
        {
            try
            {
                AppUser.Add(user);
                var logPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/Login-.txt");

                var log = new LoggerConfiguration()
                    .WriteTo.File(
                        logPath,
                        rollingInterval: RollingInterval.Day, // creates new file per day
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                    .CreateLogger();

                log.Information("User {User} Created By {UserName} at {Time}", user.Id, Session["userName"].ToString(), DateTime.Now);

                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')",
                   "Data saved successfully.", "success", "redirect", "/User"));
            }
            catch (System.Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                  ex.Message, "failure"));
            }
        }
        #endregion

        #region Update
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var user = AppUser.Get(id);
            return View(user);
        }

        [HttpPost]
        public JavaScriptResult Edit(UserViewModel user)
        {
            try
            {
                AppUser.Update(user);

                var logPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/Login-.txt");

                var log = new LoggerConfiguration()
                    .WriteTo.File(
                        logPath,
                        rollingInterval: RollingInterval.Day, // creates new file per day
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                    .CreateLogger();

                log.Information("User {User} Updated By {UserName} at {Time}", user.Id, Session["userName"].ToString(), DateTime.Now);

                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')",
                   "Data saved successfully.", "success", "redirect", "/User"));
            }
            catch (System.Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                  ex.Message, "failure"));
            }
        }
        #endregion

    }
}