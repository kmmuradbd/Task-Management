using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Service.ViewModel
{
    public class UserRoleViewModel
    {
        public int Id { get; set; }
        public int MasterMenuId { get; set; }
        public int UserRoleMasterId { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }


    }
}
