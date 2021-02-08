using MySql.Data.MySqlClient;
using System.Data.Common;

namespace SysSped.Infra.Data
{
    public abstract class BaseRepository
    {
        public DbConnection _conn;
        public BaseRepository()
        {
            //_conn = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SysSped"].ConnectionString);
            _conn = new MySqlConnection(@"server=localhost;user id=admin;password=abc123;persistsecurityinfo=True;database=sysspeddb;SslMode=none;");
            _conn.Open();
        }

        public BaseRepository(bool coisa)
        {
            _conn = new MySqlConnection(@"server=localhost;user id=admin;password=abc123;persistsecurityinfo=True;database=sys;SslMode=none;");
            _conn.Open();
        }
    }
}
