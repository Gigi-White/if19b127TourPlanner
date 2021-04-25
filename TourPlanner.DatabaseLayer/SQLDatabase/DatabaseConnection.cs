using Npgsql;
using System;
using System.Configuration;

namespace TourPlanner.DataAccessLayer
{
    internal class DatabaseConnection:IDatabaseConnection
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string accessData { get; set; }

        public DatabaseConnection()
        {
            accessData = ConfigurationManager.AppSettings["DatabaseAccess"].ToString();
        }



        public NpgsqlDataReader getDatabaseData(NpgsqlCommand sqlCommand)
        {

            log.Debug("Maintenance: getDatabaseData SQLCommand: " + sqlCommand.CommandText.ToString());

            try
            {

                NpgsqlDataReader rdr = sqlCommand.ExecuteReader();
                return rdr;
            }
            catch (Exception ex)
            {
                log.Error("Maintenance: Database did not return Data", ex);
                return null;
            }

        }

        public bool updateDatabaseData(NpgsqlCommand sqlCommand)
        {           
            log.Debug("Maintenance: updateDatabaseData SQLCommand: " + sqlCommand.CommandText.ToString());

            try
            {
                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                log.Error("Maintenance: Database did not execute the SQL Command", ex);
                return false;
            }
        }

    }
}
