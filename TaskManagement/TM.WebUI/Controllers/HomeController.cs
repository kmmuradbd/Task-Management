using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TM.Infrastructure;
using TM.Service.Interface;
using TM.WebUI.SignalR;

namespace TM.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        protected readonly IUserService AppUser;
        protected readonly IMasterMenuService AppMasterMenu;
        protected readonly IMemberTaskService AppTask;
        public HomeController(IUserService user, IMasterMenuService masterMenu, IMemberTaskService memberTask)
        {
            this.AppUser = user;
            this.AppMasterMenu = masterMenu;
            this.AppTask = memberTask;
        }
       
        public ActionResult Index()
        {
            var data = AppMasterMenu.GetAll(Session["userName"].ToString());
            Session["menu"] = data;
            // Get currently online users
            var onlineUsers = MvcApplication.GetOnlineUsers();
            return View(onlineUsers);
        }

        public JsonResult GetNotificationContacts()
        {
            var noticiationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            var list = AppTask.GetAll(noticiationRegisterTime);
            // Update session to get new added contacts only notification)
            Session["LastUpdated"] = DateTime.Now;
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}