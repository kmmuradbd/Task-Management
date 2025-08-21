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
    public class MemberTaskController : Controller
    {
        protected readonly IMemberTaskService AppMemberTask;
        public MemberTaskController(IMemberTaskService memberTask)
        {
            this.AppMemberTask = memberTask;
        }
        public ActionResult Index()
        {
            try
            {
                IEnumerable<MemberTaskViewModel> memberTasks = AppMemberTask.GetAll();
                return View(memberTasks);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JavaScriptResult Create(MemberTaskViewModel memberTask)
        {
            try
            {
                AppMemberTask.Add(memberTask);
                var logPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/Login-.txt");

                var log = new LoggerConfiguration()
                    .WriteTo.File(
                        logPath,
                        rollingInterval: RollingInterval.Day, // creates new file per day
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                    .CreateLogger();

                log.Information("Task {Task} Created By {UserName} at {Time}", memberTask.Id, Session["userName"].ToString(), DateTime.Now);

                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')",
                   "Data saved successfully.", "success", "redirect", "/MemberTask"));
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
        public ActionResult Edit(int id)
        {
            var task = AppMemberTask.Get(id);
            return View(task);
        }

        [HttpPost]
        public JavaScriptResult Edit(MemberTaskViewModel memberTask)
        {
            try
            {
                AppMemberTask.Update(memberTask);

                var logPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/Login-.txt");

                var log = new LoggerConfiguration()
                    .WriteTo.File(
                        logPath,
                        rollingInterval: RollingInterval.Day, // creates new file per day
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                    .CreateLogger();

                log.Information("Task {Task} Updated By {UserName} at {Time}", memberTask.Id, Session["userName"].ToString(), DateTime.Now);

                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')",
                   "Data saved successfully.", "success", "redirect", "/MemberTask"));
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