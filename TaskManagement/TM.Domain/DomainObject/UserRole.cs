using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Core;

namespace TM.Domain.DomainObject
{
    public class UserRole : Entity
    {
        public int Id { get; set; }
        public int MasterMenuId { get; set; }
        public virtual UserRoleMaster UserRoleMaster { get; set; }
        public int UserRoleMasterId { get; set; }
    }
}
