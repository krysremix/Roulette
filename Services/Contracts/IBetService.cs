using Infrastructure;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IBetService
    {
        Task<int> PlaceBet(Bet bet);
        Task<SpinResult> Spin();
        Task PayoutBets();
    }
}
