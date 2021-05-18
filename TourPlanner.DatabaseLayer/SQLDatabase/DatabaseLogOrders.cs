using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.SQLDatabase
{
    internal class DatabaseLogOrders : IDatabaseLogOrders
    {


        private static string getLogsOfTourSql = "SELECT* FROM tourlog WHERE tourname = @tourname";

        public bool createLog(Log newLog)
        {
            throw new NotImplementedException();
        }

 

        public List<Log> getLogsofTour(string tourname)
        {
            var sql = getLogsOfTourSql;

            //NpgsqlDataReader rdr = DataConnectionFactory.GetDatabaseConnectionInstance().getDatabaseData(sql);
            NpgsqlConnection con = DataConnectionFactory.GetCon();
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("tourname", tourname);
            cmd.Prepare();

            NpgsqlDataReader rdr = DataConnectionFactory.GetDatabaseConnectionInstance().getDatabaseData(cmd);
            if (rdr == null)
            {
                return null;
            }

            List<Log> myLogs = new List<Log>();
            while (rdr.Read())
            {
                myLogs.Add(new Log
                {
                    name = rdr.GetString(2),
                    date = rdr.GetString(3),
                    reportfile = rdr.GetString(4),
                    distance = rdr.GetInt32(5),
                    totalTime = rdr.GetString(6),               
                    rating = rdr.GetInt32(7),
                    travelBy = rdr.GetString(8),
                    averageSpeed = rdr.GetString(9),
                    recommandRestaurant = rdr.GetString(10),
                    recommandHotel = rdr.GetString(11),
                    sightWorthSeeing = rdr.GetString(12)
                }
                );
            }
            con.Close();
            return myLogs;


        }

        public bool updateLog(Log updatedLog)
        {
            throw new NotImplementedException();
        }

        public bool copyLogsofTour(string tourname, string copiedtourname)
        {
            return true;
        }

        public bool deleteAllLogsofTour(string tourname)
        {
            return true;
        }

        public bool deleteOneLog(string logname)
        {
            throw new NotImplementedException();
        }
    }
}
