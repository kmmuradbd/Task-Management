using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Domain.DomainObject;
using TM.Domain.RepositoryContract;

namespace TM.Infrastructure.Repository
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
    }
}
