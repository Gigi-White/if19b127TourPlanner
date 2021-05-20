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
        
        private static string createLogSql = "INSERT INTO tourlog(tourname, logname, date, reportfile, totaltime," +
                                                "rating, travelby, averagespeed, recommandrestaurant, recommandhotel, sightworthseeing, distance)" +
                                                "VALUES(@tourname, @logname, @date, @reportfile, @totaltime, @rating, @travelby, @averagespeed, @recommandrestaurant, @recommandhotel, @sightworthseeing, @distance)";

        private static string deleteOneLogSql = "DELETE FROM tourlog WHERE tourname = @tourname AND logname = @logname";

        public bool CreateLog(Log newLog)
        {
            string sql = createLogSql;
            NpgsqlConnection con = DataConnectionFactory.GetCon();
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("tourname", newLog.tourname);
            cmd.Parameters.AddWithValue("logname", newLog.logname);
            cmd.Parameters.AddWithValue("date", newLog.date);
            cmd.Parameters.AddWithValue("reportfile", newLog.reportfile);       
            cmd.Parameters.AddWithValue("totaltime", newLog.totalTime);
            cmd.Parameters.AddWithValue("rating", newLog.rating);
            cmd.Parameters.AddWithValue("travelby", newLog.travelBy);
            cmd.Parameters.AddWithValue("averagespeed", newLog.averageSpeed);
            cmd.Parameters.AddWithValue("recommandrestaurant", newLog.recommandRestaurant);
            cmd.Parameters.AddWithValue("recommandhotel", newLog.recommandHotel);
            cmd.Parameters.AddWithValue("sightworthseeing", newLog.sightWorthSeeing);
            cmd.Parameters.AddWithValue("distance", newLog.distance);
            cmd.Prepare();

            if (DataConnectionFactory.GetDatabaseConnectionInstance().updateDatabaseData(cmd))
            {
                con.Close();
                return true;
            }
            else
            {

                con.Close();
                return false;
            }
          
        }

 







        public List<Log> GetLogsofTour(string tourname)
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
                    tourname = rdr.GetString(1),
                    logname = rdr.GetString(2),
                    date = rdr.GetString(3),
                    reportfile = rdr.GetString(4),                   
                    totalTime = rdr.GetString(5),               
                    rating = rdr.GetInt32(6),
                    travelBy = rdr.GetString(7),
                    averageSpeed = rdr.GetString(8),
                    recommandRestaurant = rdr.GetString(9),
                    recommandHotel = rdr.GetString(10),
                    sightWorthSeeing = rdr.GetString(11),
                    distance = rdr.GetString(12)
                }
                );
            }
            con.Close();
            return myLogs;


        }

        public bool UpdateLog(Log updatedLog)
        {
            throw new NotImplementedException();
        }

        public bool CopyLogsofTour(string tourname, string copiedtourname)
        {
            return true;
        }

        public bool DeleteAllLogsofTour(string tourname)
        {
            return true;
        }

        public bool DeleteOneLog(string tourname, string logname)
        {
            
            string sql = deleteOneLogSql;
            NpgsqlConnection con = DataConnectionFactory.GetCon();
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("tourname", tourname);
            cmd.Parameters.AddWithValue("logname", logname);
            cmd.Prepare();

            if (DataConnectionFactory.GetDatabaseConnectionInstance().updateDatabaseData(cmd))
            {
                con.Close();
                return true;
            }
            else
            {

                con.Close();
                return false;
            }
        }
    }
}
