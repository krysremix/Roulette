using Microsoft.Extensions.Configuration;
using Repos.Contracts;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Repos.Implementations
{
    public class InitializerRepo : IInitializerRepo
    {
        private readonly IConfiguration _configuration;
        public InitializerRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Initialize()
        {
            var dbName = _configuration.GetSection("DBName").Value;
            if (!File.Exists(dbName))
            {
                File.Create(dbName).Close();
            }

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                //Creating tables if not exist
                var sQuery = @"CREATE TABLE IF NOT EXISTS Bets (
                            Id           INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                            BetType      TEXT    NOT NULL,
                            BetValue     TEXT    NOT NULL,
                            Amount       REAL    NOT NULL,
                            Payout       REAL,
                            IsWinningBet INTEGER NOT NULL,
                            SpinId       INTEGER NULL,
                            WasPayedout  INTEGER NOT NULL
                        );";

                try
                {
                    await connection.ExecuteAsync(sQuery);
                }
                catch(Exception ex)
                {
                    throw ex;
                }

                sQuery = @"CREATE TABLE IF NOT EXISTS Spins (
                            Id           INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                            Number       INTEGER NOT NULL,
                            Colour       TEXT    NOT NULL,
                            Time         TEXT    NOT NULL
                        );";

                try
                {
                    await connection.ExecuteAsync(sQuery);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
