using Infrastructure;
using Microsoft.Extensions.Configuration;
using Repos.Contracts;
using System.Threading.Tasks;
using Dapper;
using System.Collections.Generic;

namespace Repos.Implementations
{
    public class BetRepo : BaseRepository, IBetRepo
    {
        public BetRepo(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<Bet>> GetAllNonCompletedBets()
        {
            using (var connection = GetDb())
            {
                var sQuery = @"
                                SELECT 
                                    Id,
                                    BetType,
                                    BetValue,
                                    Amount
                                FROM
                                    Bets
                                WHERE
                                    Payout is null
                              ";

                return await connection.QueryAsync<Bet>(sQuery);
            }
        }

        public async Task PayoutBets()
        {
            using (var connection = GetDb())
            {
                var sQuery = @"
                            UPDATE 
                                Bets
                            SET
                                WasPayedout = 1
                            WHERE
                                IsWinningBet = 1
                                AND WasPayedout = 0;";
                connection.Open();
                await connection.ExecuteAsync(sQuery);
            }
        }

        public async Task<int> PlaceBet(Bet bet)
        {
            using (var connection = GetDb())
            {
                var sQuery = @"
                            INSERT INTO Bets
                            (
                                BetType, 
                                BetValue, 
                                Amount, 
                                IsWinningBet,
                                WasPayedout
                            )
                            VALUES
                            (
                                @BetType,
                                @BetValue,
                                @Amount,
                                0,
                                0
                            )
                            RETURNING Id";
                connection.Open();
                return await connection.ExecuteScalarAsync<int>(sQuery, new { bet.BetType, bet.BetValue, bet.Amount });
            }
        }

        public async Task UpdateBetPayout(Bet bet, int spinId)
        {
            using (var connection = GetDb())
            {
                var sQuery = @"
                            UPDATE 
                                Bets
                            SET
                                Payout = @Payout,
                                IsWinningBet = @IsWinningBet,
                                SpinId = @SpinId
                            WHERE
                                Id = @Id";
                connection.Open();
                await connection.ExecuteAsync(sQuery, new { bet.Payout, bet.IsWinningBet, SpinId = spinId, bet.Id });
            }
        }
    }
}
