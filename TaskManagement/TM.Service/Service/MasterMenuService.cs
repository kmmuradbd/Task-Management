using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Domain.RepositoryContract;
using TM.Service.Interface;
using TM.Service.ViewModel;

namespace TM.Service.Service
{
    public class MasterMenuService : IMasterMenuService
    {
        protected readonly IMasterMenuRepository RepoMasterMenu;
        public MasterMenuService(IMasterMenuRepository masterMenu)
        {
            RepoMasterMenu = masterMenu;
        }
        public void Add(MasterMenuViewModel masterMenu)
        {
            throw new NotImplementedException();
        }
        public void Update(MasterMenuViewModel masterMenu)
        {
            throw new NotImplementedException();
        }

        public MasterMenuViewModel Get(string id)
        {
            throw new NotImplementedException();
        }

        public List<MasterMenuViewModel> GetAll()
        {
            var masterMenus = (from mastermenu in RepoMasterMenu.GetAll(r => !r.IsArchived)
                               select new MasterMenuViewModel()
                               {
                                   Id = mastermenu.Id,
                                   Name = mastermenu.Name,
                                   URL = mastermenu.URL,
                                   ParentId = mastermenu.ParentId,
                                   SortNo = mastermenu.SortNo,
                                   Icon = mastermenu.Icon,
                                   IsActive = mastermenu.IsActive,
                                   CreatedDate = mastermenu.CreatedDate,
                                   CreatedBy = mastermenu.CreatedBy,
                                   UpdatedBy = mastermenu.UpdatedBy,
                                   UpdatedDate = mastermenu.UpdatedDate,
                                   IsArchived = mastermenu.IsArchived
                               }).ToList();
            return masterMenus;
        }
        public List<MasterMenuViewModel> GetAll(string userId)
        {
            try
            {
                SqlParameter usernameParam = new SqlParameter("@userId", userId.ToString() ?? (object)DBNull.Value);

                return RepoMasterMenu.SqlQuery<MasterMenuViewModel>("Sp_GetMenues @userId", usernameParam).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
