using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Core;

namespace TM.Domain.DomainObject
{
    public class MasterMenu : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public string URL { get; set; }
        public int SortNo { get; set; }
        public string Icon { get; set; }
    }
}
