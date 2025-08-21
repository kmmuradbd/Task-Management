using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Service.ViewModel;

namespace TM.Service.Interface
{
    public interface IProjectService
    {
        void Add(ProjectViewModel project);
        void Update(ProjectViewModel project);
        ProjectViewModel Get(int id);
        IEnumerable<ProjectViewModel> GetAll();
        IEnumerable<object> GetProjectList();
    }
}
