using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Bet
    {
        public int Id { get; set; }
        public string BetType { get; set; } // e.g., "number", "color"
        public string BetValue { get; set; } // e.g., "17", "red"
        public decimal Amount { get; set; }
        public decimal? Payout { get; set; }
        public bool IsWinningBet { get; set; }
        public int? SpinId { get; set; }
        public bool WasPayedout { get; set; }
    }
}
