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
    public class UserService : IUserService
    {
        protected readonly IUserRepository RepoUser;
        protected readonly IUserRoleRepository RepoUserRole;
        protected readonly IUserRoleMasterRepository RepoUserRoleMaster;

        public UserService(IUserRepository user, IUserRoleRepository userRole, IUserRoleMasterRepository userRoleMaster)
        {
            this.RepoUser = user;
            this.RepoUserRole = userRole;
            this.RepoUserRoleMaster = userRoleMaster;
        }
        public void Add(UserViewModel user)
        {
            try
            {
                TMIdentity identity = (TMIdentity)Thread.CurrentPrincipal.Identity;
               // bool isExistsUser = RepoUser.GetAll(a => a.Id.ToLower().Trim() == user.Id.ToLower().Trim()).Any();
                user.Password = Common.HashCode(user.Password);
                //if (!isExistsUser)
               // {
                    user.CreatedDate = DateTime.Now;
                    user.CreatedBy = identity.Name;
                    user.IsActive = true;
                    RepoUser.Add(user.ToEntity());
                //}
            }
            catch (Exception ex)
            {

                if (ex.InnerException.InnerException.Message.Contains("PK_Users"))
                {
                    throw new Exception("This Name(" + user.Id + ") is already exists");
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public void Update(UserViewModel user)
        {
            try
            {
                TMIdentity identity = (TMIdentity)Thread.CurrentPrincipal.Identity;
                UserViewModel oldUser = Get(user.Id);
                oldUser.Password = Common.HashCode(user.Password);
                oldUser.UserRoleMasterId = user.UserRoleMasterId;
                oldUser.FullName = user.FullName;
                oldUser.UpdatedBy = identity.Name;
                oldUser.UpdatedDate = DateTime.Now;
                RepoUser.Update(oldUser.ToEntity());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void ChangePassword(UserViewModel user)
        {
            TMIdentity identity = (TMIdentity)Thread.CurrentPrincipal.Identity;
            UserViewModel userOld = Get(user.Id);
            user.Password = Common.HashCode(user.Password);
            user.OldPassword = Common.HashCode(user.OldPassword);
            if (userOld.Password == user.OldPassword)
            {
                userOld.Password = user.Password;
                userOld.UpdatedDate = DateTime.Now;
                userOld.UpdatedBy = identity.Name;
                RepoUser.Update(userOld.ToEntity());
            }
            else
            {
                throw new Exception("Password does not match.");
            }
        }

        public UserViewModel Get(string id)
        {
            var user = RepoUser.Get(r => r.Id == id && !r.IsArchived);
            return new UserViewModel
            {
                Id = user.Id,
                IsActive = user.IsActive,
                FullName = user.FullName,
                Password = user.Password,
                IsArchived = user.IsArchived,
                CreatedBy = user.CreatedBy,
                CreatedDate = user.CreatedDate,
                UpdatedBy = user.UpdatedBy,
                UpdatedDate = user.UpdatedDate,
                UserRoleMasterId = user.UserRoleMasterId
            };
        }

        public List<UserViewModel> GetAll()
        {
            var us = (from user in RepoUser.GetAll(r => !r.IsArchived)
                      select new UserViewModel()
                      {
                          Id = user.Id,
                          IsActive = user.IsActive,
                          FullName = user.FullName,
                          Password = user.Password,
                          CreatedDate = user.CreatedDate,
                          CreatedBy = user.CreatedBy,
                          UpdatedBy = user.UpdatedBy,
                          UpdatedDate = user.UpdatedDate,
                          UserRoleMasterId = user.UserRoleMasterId,
                          RoleName = RepoUserRoleMaster.Get(r => r.Id == user.UserRoleMasterId).Name,
                          IsArchived = user.IsArchived
                      }).OrderByDescending(o=>o.CreatedDate).ToList();
            return us;
        }

        public bool Login(string username, string password)
        {
            password = Common.HashCode(password);
            return RepoUser.GetAny(r => r.Id == username && r.Password == password && r.IsActive);
        }



        public List<UserRoleViewModel> GetAllUserRoles(int userRoleMasterId)
        {
            var userList = (from user in RepoUserRole.GetAll(r => r.UserRoleMaster.Id == userRoleMasterId && !r.IsArchived)
                            select new UserRoleViewModel()
                            {
                                Id = user.Id,
                                IsActive = user.IsActive,
                                IsArchived = user.IsArchived,
                                MasterMenuId = user.MasterMenuId,
                                UserRoleMasterId = user.UserRoleMasterId,
                                CreatedBy = user.CreatedBy,
                                CreatedDate = user.CreatedDate,
                                UpdatedBy = user.UpdatedBy,
                                UpdatedDate = user.UpdatedDate,

                            }).ToList();
            return userList;
        }

        public string[] GetUserRoles(int userRoleMasterId)
        {
            // Ensure MasterMenuId is cast to a string if necessary
            string[] roles = (from ur in RepoUserRole.GetAll(x => x.UserRoleMaster.Id == userRoleMasterId)
                              select ur.MasterMenuId.ToString()).ToArray();
            return roles;
        }

        public IEnumerable<object> GetUserRoleMasterList()
        {
            return from r in RepoUserRoleMaster.GetAll()
                   select new { Text = r.Name, Value = r.Id };
        }

    }
}
