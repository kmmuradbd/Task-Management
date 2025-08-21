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

namespace TM.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        protected readonly IUserService AppUser;
        protected readonly IMasterMenuService AppMasterMenu;
        public HomeController(IUserService user, IMasterMenuService masterMenu)
        {
            this.AppUser = user;
            this.AppMasterMenu = masterMenu;
        }
       
        public ActionResult Index()
        {
            var data = AppMasterMenu.GetAll(Session["userName"].ToString());
            Session["menu"] = data;
            return View();
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