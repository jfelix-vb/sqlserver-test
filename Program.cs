// RAD database connection tester
//
// Opens a connection to an remote SQL Server database and executes a single query.
// Check if any trouble was found on connection handshake.
//

using System.Net;
using Microsoft.Data.SqlClient;

using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;

namespace rad;

class RADDbTest
{
    // For reference: sample connection data to be used in the tests:

    // Without server encryption enabled:
    // "Server=10.200.10.52,1433; Integrated Security=False; Initial Catalog=RADServer; User Id=sysgls; Password=22500m; Encrypt=False;";

    // With server encryption enabled:
    // "Server=10.200.10.52,1433; Integrated Security=False; Initial Catalog=master; User Id=sa; Password=22500m; Encrypt=True; TrustServerCertificate=True;";

    static async Task Main(string[] args)
    {
        Option<string> host = new (name: "--host", description: "SQL Server host name or IP.");
        Option<string> port = new (name: "--port", description: "The TCP port for connection (default 1433).");
        Option<string> database = new (name: "--database", description: "The initial database to use.");
        Option<string> uid = new (name: "--uid", description: "User login.");
        Option<string> password = new (name: "--password", description: "User password.");

        RootCommand rootCommand = new RootCommand(
            description: "Tests a connection with a legacy SQL Server and prints the remote server version banner.")
            {
                host, port, database, uid, password
            };

        rootCommand.SetHandler(
            (string host, string port, string database, string uid, string password) =>
            {
                if (port == null)
                {
                    port = "1433";
                }

                string sqlConnectionData = 
                    $"Server={host},{port}; Integrated Security=False; " + 
                    $"Initial Catalog={database}; User Id={uid}; Password={password}; " + 
                    $"Encrypt=False;";

                try
                {
                    using (SqlConnection conn = new SqlConnection(sqlConnectionData))
                    {
                        conn.Open();

                        using (SqlCommand command = new SqlCommand("SELECT @@version", conn))
                        {
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                Console.WriteLine("{0}", reader.GetString(0));
                            }
                        }

                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                    Console.WriteLine(ex.InnerException);
                    Console.WriteLine();
                    Console.WriteLine(ex.StackTrace);
                }            },
            host,
            port,
            database,
            uid,
            password);
        await rootCommand.InvokeAsync(args);
    }
}
