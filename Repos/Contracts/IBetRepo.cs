using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Contracts
{
    public interface IBetRepo
    {
        Task<int> PlaceBet(Bet bet);
        Task<IEnumerable<Bet>> GetAllNonCompletedBets();
        Task UpdateBetPayout(Bet bet, int spinId);
        Task PayoutBets();
    }
}
