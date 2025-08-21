using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Core;
using TM.Domain.DomainObject;

namespace TM.Domain.RepositoryContract
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
