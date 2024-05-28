using Infrastructure;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using Repos.Contracts;
using Services.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class BetService : IBetService
    {
        private readonly IBetRepo _betRepo;
        private readonly ISpinService _spinService;
        private static readonly Random _random = new();

        public BetService(IBetRepo betRepo, ISpinService spinService)
        {
            _betRepo = betRepo;
            _spinService = spinService;
        }

        public async Task<int> PlaceBet(Bet bet)
        {
            return await _betRepo.PlaceBet(bet);
        }

        public async Task<SpinResult> Spin()
        {
            int number = _random.Next(0, 37);
            string color = (number == 0) ? "green" : (number % 2 == 0) ? "black" : "red";

            var bets = (await _betRepo.GetAllNonCompletedBets()).ToList();

            if (!bets.Any())
            {
                throw new NoBetPlacedException();
            }

            var spinResult = new SpinResult { Number = number, Colour = color };
            var spinId = await _spinService.SaveSpin(spinResult);

            foreach (var bet in bets)
            {
                bet.IsWinningBet = IsWinningBet(bet, spinResult);
                bet.Payout = bet.IsWinningBet ? CalculatePayout(bet) : 0;

                await _betRepo.UpdateBetPayout(bet, spinId);
            }

            return spinResult;
        }

        private bool IsWinningBet(Bet bet, SpinResult spinResult)
        {
            return bet.BetType switch
            {
                "number" => bet.BetValue == spinResult.Number.ToString(),
                "colour" => bet.BetValue == spinResult.Colour,
                _ => false
            };
        }

        private decimal CalculatePayout(Bet bet)
        {
            return bet.BetType switch
            {
                "number" => bet.Amount * 35,
                "colour" => bet.Amount * 2,
                _ => 0
            };
        }

        public async Task PayoutBets()
        {
            await _betRepo.PayoutBets();
        }
    }
}
