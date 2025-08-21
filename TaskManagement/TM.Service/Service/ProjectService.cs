using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TM.Domain.RepositoryContract;
using TM.Infrastructure;
using TM.Service.Interface;
using TM.Service.ViewModel;

namespace TM.Service.Service
{
    public class ProjectService : IProjectService
    {
        protected readonly IProjectRepository RepoProject;
        protected readonly IUserRepository RepoUser;
        public ProjectService(IProjectRepository project, IUserRepository repoUser)
        {
            this.RepoProject = project;
            RepoUser = repoUser;
        }
        public void Add(ProjectViewModel project)
        {
            try
            {
                TMIdentity identity = (TMIdentity)Thread.CurrentPrincipal.Identity;
                project.Id = RepoProject.GetAutoNumber();
                project.CreatedDate = DateTime.Now;
                project.CreatedBy = identity.Name;
                RepoProject.Add(project.ToEntity());
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("IX_ProjectName"))
                {
                    throw new Exception("This Name(" + project.Name + ") is already exists");
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public void Update(ProjectViewModel project)
        {
            try
            {
                TMIdentity identity = (TMIdentity)Thread.CurrentPrincipal.Identity;
                ProjectViewModel oldProject = Get(project.Id);
                oldProject.IsActive = project.IsActive;
                oldProject.Name = project.Name;
                oldProject.StartDate = project.StartDate;
                oldProject.EndDate = project.EndDate;
                oldProject.ManagerId = project.ManagerId;
                oldProject.Remarks = project.Remarks;
                oldProject.UpdatedBy = identity.Name;
                oldProject.UpdatedDate = DateTime.Now;
                RepoProject.Update(oldProject.ToEntity());
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("IX_ProjectName"))
                {
                    throw new Exception("This Name(" + project.Name + ") is already exists");
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public ProjectViewModel Get(int id)
        {
            var project = RepoProject.Get(r => r.Id == id && !r.IsArchived);
            return new ProjectViewModel
            {
                Id = project.Id,
                IsActive = project.IsActive,
                Name = project.Name,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ManagerId = project.ManagerId,
                Remarks = project.Remarks,
                IsArchived = project.IsArchived,
                CreatedBy = project.CreatedBy,
                CreatedDate = project.CreatedDate,
                UpdatedBy = project.UpdatedBy,
                UpdatedDate = project.UpdatedDate,
            };
        }

        public IEnumerable<ProjectViewModel> GetAll()
        {
            var projectList = (from project in RepoProject.GetAll(r => !r.IsArchived)
                               orderby project.Name
                               select new ProjectViewModel()
                               {
                                   Id = project.Id,
                                   IsActive = project.IsActive,
                                   Name = project.Name,
                                   StartDate = project.StartDate,
                                   EndDate = project.EndDate,
                                   ManagerId = project.ManagerId,
                                   ManagerName = RepoUser.Get(r => r.Id == project.ManagerId).FullName,
                                   Remarks = project.Remarks,
                                   IsArchived = project.IsArchived,
                                   CreatedBy = project.CreatedBy,
                                   CreatedDate = project.CreatedDate,
                                   UpdatedBy = project.UpdatedBy,
                                   UpdatedDate = project.UpdatedDate
                               }).ToList();
            return projectList;
        }

        public IEnumerable<object> GetProjectList()
        {
            return from r in RepoProject.GetAll()
                   select new { Text = r.Name, Value = r.Id };
        }

    }
}
