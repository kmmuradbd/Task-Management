using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Core;

namespace TM.Domain.DomainObject
{
    public class User : Entity
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public int UserRoleMasterId { get; set; }
    }
}
