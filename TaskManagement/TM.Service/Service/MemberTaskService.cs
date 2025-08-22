using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TM.Domain.RepositoryContract;
using TM.Infrastructure;
using TM.Infrastructure.Repository;
using TM.Service.Interface;
using TM.Service.ViewModel;

namespace TM.Service.Service
{
    public class MemberTaskService : IMemberTaskService
    {
        protected readonly IMemberTaskRepository RepoMemberTask;
        protected readonly IUserRepository RepoUser;
        protected readonly IProjectRepository RepoProject;
        public MemberTaskService(IMemberTaskRepository memberTask, IUserRepository repoUser, IProjectRepository repoProject)
        {
            this.RepoMemberTask = memberTask;
            RepoUser = repoUser;
            RepoProject = repoProject;
        }
        public void Add(MemberTaskViewModel memberTask)
        {
            try
            {
                TMIdentity identity = (TMIdentity)Thread.CurrentPrincipal.Identity;
                memberTask.Id = RepoMemberTask.GetAutoNumber();
                memberTask.CreatedDate = DateTime.Now;
                memberTask.CreatedBy = identity.Id;
                memberTask.AssignDate = DateTime.Now;
                memberTask.Status = "Pending";
                RepoMemberTask.Add(memberTask.ToEntity());
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("IX_MemberTaskName"))
                {
                    throw new Exception("This Name(" + memberTask.Name + ") is already exists");
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public void Update(MemberTaskViewModel memberTask)
        {
            try
            {
                TMIdentity identity = (TMIdentity)Thread.CurrentPrincipal.Identity;
                MemberTaskViewModel oldMemberTask = Get(memberTask.Id);
                oldMemberTask.IsActive = identity.RoleId == 2 ? memberTask.IsActive : oldMemberTask.IsActive;
                oldMemberTask.Name = identity.RoleId == 2 ? memberTask.Name : oldMemberTask.Name;
                oldMemberTask.ProjectId = identity.RoleId == 2 ? memberTask.ProjectId : oldMemberTask.ProjectId;
                oldMemberTask.MemberId = identity.RoleId == 2 ? memberTask.MemberId : oldMemberTask.MemberId;
                oldMemberTask.Status = memberTask.Status;
                if (memberTask.Status == "Doing")
                    oldMemberTask.WorkStartDate = DateTime.Now;
                if (memberTask.Status == "Complte")
                {
                    oldMemberTask.WorkEndDate = DateTime.Now;
                    if (oldMemberTask.WorkStartDate.HasValue && oldMemberTask.WorkEndDate.HasValue)
                    {
                        TimeSpan timeSpan = oldMemberTask.WorkEndDate.Value - oldMemberTask.WorkStartDate.Value;
                        oldMemberTask.Duration = timeSpan.ToString();
                    }

                }

                oldMemberTask.Comments = memberTask.Comments;
                oldMemberTask.Remarks = identity.RoleId == 2 ? memberTask.Remarks : oldMemberTask.Remarks;
                oldMemberTask.UpdatedBy = identity.Id;
                oldMemberTask.UpdatedDate = DateTime.Now;
                RepoMemberTask.Update(oldMemberTask.ToEntity());
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("IX_MemberTaskName"))
                {
                    throw new Exception("This Name(" + memberTask.Name + ") is already exists");
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public MemberTaskViewModel Get(int id)
        {
            var memberTask = RepoMemberTask.Get(r => r.Id == id && !r.IsArchived);
            return new MemberTaskViewModel
            {
                Id = memberTask.Id,
                IsActive = memberTask.IsActive,
                Name = memberTask.Name,
                ProjectId = memberTask.ProjectId,
                MemberId = memberTask.MemberId,
                Status = memberTask.Status,
                AssignDate = memberTask.AssignDate,
                WorkStartDate = memberTask.WorkStartDate,
                WorkEndDate = memberTask.WorkEndDate,
                Duration = memberTask.Duration,
                Comments = memberTask.Comments,
                Remarks = memberTask.Remarks,
                IsArchived = memberTask.IsArchived,
                CreatedBy = memberTask.CreatedBy,
                CreatedDate = memberTask.CreatedDate,
                UpdatedBy = memberTask.UpdatedBy,
                UpdatedDate = memberTask.UpdatedDate,
            };
        }

        public IEnumerable<MemberTaskViewModel> GetAll()
        {
            TMIdentity identity = (TMIdentity)Thread.CurrentPrincipal.Identity;
            var memberTaskList = (from memberTask in RepoMemberTask.GetAll(r => (identity.IsSysAdmin) || (r.CreatedBy == identity.Id) || (r.MemberId == identity.Id) && !r.IsArchived)
                                  orderby memberTask.Name
                                  select new MemberTaskViewModel()
                                  {
                                      Id = memberTask.Id,
                                      IsActive = memberTask.IsActive,
                                      Name = memberTask.Name,
                                      ProjectId = memberTask.ProjectId,
                                      ProjectName = RepoProject.Get(memberTask.ProjectId).Name,
                                      MemberId = memberTask.MemberId,
                                      MemberName = RepoUser.Get(r => r.Id == memberTask.MemberId).FullName,
                                      Status = memberTask.Status,
                                      AssignDate = memberTask.AssignDate,
                                      WorkStartDate = memberTask.WorkStartDate,
                                      WorkEndDate = memberTask.WorkEndDate,
                                      Duration = memberTask.Duration,
                                      Comments = memberTask.Comments,
                                      Remarks = memberTask.Remarks,
                                      IsArchived = memberTask.IsArchived,
                                      CreatedBy = memberTask.CreatedBy,
                                      CreatedDate = memberTask.CreatedDate,
                                      UpdatedBy = memberTask.UpdatedBy,
                                      UpdatedDate = memberTask.UpdatedDate,
                                  }).OrderByDescending(o=> o.CreatedDate).ToList();
            return memberTaskList;
        }

        public IEnumerable<MemberTaskViewModel> GetAll(DateTime createDate)
        {
            TMIdentity identity = (TMIdentity)Thread.CurrentPrincipal.Identity;
            var memberTaskList = (from memberTask in RepoMemberTask.GetAll(r => r.MemberId==identity.Id && r.CreatedDate> createDate && !r.IsArchived)
                                  orderby memberTask.Name
                                  select new MemberTaskViewModel()
                                  {
                                      Id = memberTask.Id,
                                      IsActive = memberTask.IsActive,
                                      Name = memberTask.Name,
                                     // ProjectId = memberTask.ProjectId,
                                     // ProjectName = RepoProject.Get(memberTask.ProjectId).Name,
                                     // MemberId = memberTask.MemberId,
                                     // MemberName = RepoUser.Get(r => r.Id == memberTask.MemberId).FullName,
                                      Status = memberTask.Status,
                                      //AssignDate = memberTask.AssignDate,
                                     // WorkStartDate = memberTask.WorkStartDate,
                                     // WorkEndDate = memberTask.WorkEndDate,
                                      // Duration = memberTask.Duration,
                                      //Comments = memberTask.Comments,
                                     // Remarks = memberTask.Remarks,
                                     // IsArchived = memberTask.IsArchived,
                                     // CreatedBy = memberTask.CreatedBy,
                                     // CreatedDate = memberTask.CreatedDate,
                                    //  UpdatedBy = memberTask.UpdatedBy,
                                    //  UpdatedDate = memberTask.UpdatedDate,
                                  }).OrderByDescending(a => a.CreatedDate).ToList();
            return memberTaskList;
        }

    }
}
