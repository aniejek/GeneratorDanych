using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorDanych
{
    class Program
    {
        static void Main(string[] args)
        {
            var sqlConnection = new SqlConnection()
            {
                ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;" +
                "Initial Catalog=hd;" +
                "Integrated Security=True;" +
                "Connect Timeout=30;" +
                "Encrypt=False;" +
                "TrustServerCertificate=False;" +
                "ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
            };
            var generator = new Generator(sqlConnection);
            generator.Generate();

            generator.CloseGenerator();
        }
    }
}
