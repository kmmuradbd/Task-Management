using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Core;

namespace TM.Domain.DomainObject
{
    public class UserRoleMaster : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
