using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Service.ViewModel;

namespace TM.Service.Interface
{
    public interface IMemberTaskService
    {
        void Add(MemberTaskViewModel memberTask);
        void Update(MemberTaskViewModel memberTask);
        MemberTaskViewModel Get(int id);
        IEnumerable<MemberTaskViewModel> GetAll();
    }
}
