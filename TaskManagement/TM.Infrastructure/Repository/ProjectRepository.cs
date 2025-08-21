using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Repository
{
    public class ProjectRepository:Repository<TM.Domain.DomainObject.Project>, TM.Domain.RepositoryContract.IProjectRepository
    {
    }
}
