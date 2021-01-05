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
            _conn = new MySqlConnection(@"server=localhost;user id=admin;password=abc123;persistsecurityinfo=True;database=sysspeddb");
            _conn.Open();
        }

        public BaseRepository(bool coisa)
        {
            _conn = new MySqlConnection(@"server=localhost;user id=admin;password=abc123;persistsecurityinfo=True;database=sys");
            _conn.Open();
        }

        public void VoltaConexaoComBDSelecionado()
        {
            _conn.Close();
            _conn.Dispose();
            _conn = new MySqlConnection(@"server=localhost;user id=admin;password=abc123;persistsecurityinfo=True;database=sysspeddb");
            _conn.Open();
        }
    }
}
