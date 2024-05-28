using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Contracts
{
    public interface ISpinRepo
    {
        Task<int> SaveSpin(SpinResult spinResult);
        Task<IEnumerable<Spin>> ShowPreviousSpins();
    }
}
