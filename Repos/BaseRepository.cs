using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Repos
{
    public class BaseRepository
    {
        private readonly IConfiguration _configuration;
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqliteConnection GetDb()
        {
            return new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));            
        }
    }
}
