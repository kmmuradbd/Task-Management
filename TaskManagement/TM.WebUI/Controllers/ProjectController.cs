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
    public class ProjectController : Controller
    {
        protected readonly IProjectService AppProject;
        public ProjectController(IProjectService project)
        {
            this.AppProject = project;
        }
        // GET: Project
        public ActionResult Index()
        {
            try
            {
                IEnumerable<ProjectViewModel> projects = AppProject.GetAll();
                return View(projects);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region ddl
        public JsonResult GetProjectList()
        {
            return Json(new SelectList(AppProject.GetProjectList(), "Value", "Text"), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JavaScriptResult Create(ProjectViewModel project)
        {
            try
            {
                AppProject.Add(project);
                var logPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/Login-.txt");

                var log = new LoggerConfiguration()
                    .WriteTo.File(
                        logPath,
                        rollingInterval: RollingInterval.Day, // creates new file per day
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                    .CreateLogger();

                log.Information("Project {Project} Created By {UserName} at {Time}", project.Id, Session["userName"].ToString(), DateTime.Now);

                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')",
                   "Data saved successfully.", "success", "redirect", "/Project"));
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
            var user = AppProject.Get(id);
            return View(user);
        }

        [HttpPost]
        public JavaScriptResult Edit(ProjectViewModel user)
        {
            try
            {
                AppProject.Update(user);

                var logPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/Login-.txt");

                var log = new LoggerConfiguration()
                    .WriteTo.File(
                        logPath,
                        rollingInterval: RollingInterval.Day, // creates new file per day
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                    .CreateLogger();

                log.Information("Project {Project} Updated By {UserName} at {Time}", user.Id, Session["userName"].ToString(), DateTime.Now);

                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')",
                   "Data saved successfully.", "success", "redirect", "/Project"));
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