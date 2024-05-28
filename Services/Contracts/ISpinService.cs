using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface ISpinService
    {
        Task<int> SaveSpin(SpinResult spin);
        Task<List<Spin>> ShowPreviousSpins();
    }
}
