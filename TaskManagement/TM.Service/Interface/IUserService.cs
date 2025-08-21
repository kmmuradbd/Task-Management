using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Service.ViewModel;

namespace TM.Service.Interface
{
    public interface IUserService
    {
        void Add(UserViewModel user);
        void Update(UserViewModel user);
        void ChangePassword(UserViewModel user);
        UserViewModel Get(string id);
        List<UserViewModel> GetAll();
        bool Login(string username, string password);
        List<UserRoleViewModel> GetAllUserRoles(int userRoleMasterId);
        string[] GetUserRoles(int userRoleMasterId);
        IEnumerable<object> GetUserRoleMasterList();
    }
}
