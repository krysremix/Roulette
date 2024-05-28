using Dapper;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Repos.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Implementations
{
    public class SpinRepo : BaseRepository, ISpinRepo
    {
        public SpinRepo(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<int> SaveSpin(SpinResult spinResult)
        {
            using (var connection = GetDb())
            {
                var sQuery = @"
                            INSERT INTO Spins
                            (
                                Number, 
                                Colour, 
                                Time
                            )
                            VALUES
                            (
                                @Number,
                                @Colour,
                                @Time
                            )
                            returning Id;";
                connection.Open();
                return await connection.ExecuteScalarAsync<int>(sQuery, new { spinResult.Number, spinResult.Colour, Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
            }
        }

        public async Task<IEnumerable<Spin>> ShowPreviousSpins()
        {
            using (var connection = GetDb())
            {
                var sQuery = @"
                                SELECT 
                                    Id,
                                    Number,
                                    Colour,
                                    Time
                                FROM
                                    Spins
                              ";

                return await connection.QueryAsync<Spin>(sQuery);
            }
        }
    }
}
