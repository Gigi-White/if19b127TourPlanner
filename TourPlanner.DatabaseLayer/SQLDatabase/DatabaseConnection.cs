using Npgsql;
using System;
using System.Configuration;

namespace TourPlanner.DataAccessLayer
{
    internal class DatabaseConnection:IDatabaseConnection
    {
        private string accessData { get; set; }

        public DatabaseConnection()
        {
            accessData = ConfigurationManager.AppSettings["DatabaseAccess"].ToString();
        }



        public NpgsqlDataReader getDatabaseData(NpgsqlCommand sqlCommand)
        {

            try
            {
                //NpgsqlConnection con = new NpgsqlConnection(accessData);
                //con.Open();

                //NpgsqlCommand cmd = new NpgsqlCommand(sqlCommand, con);

                //cmd.Prepare();

                NpgsqlDataReader rdr = sqlCommand.ExecuteReader();
                return rdr;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public bool updateDatabaseData(NpgsqlCommand sqlCommand)
        {
            //try
            //{
                sqlCommand.ExecuteNonQuery();

                return true;
            //}
            /*catch (Exception)
            {
                return false;
            }*/
        }

    }
}
