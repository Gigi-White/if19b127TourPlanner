using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.SQLDatabase
{
    internal class DatabaseRouteOrders : IDatabaseRouteOrders
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string SaveRouteInfoSql = "INSERT INTO routeinfo(tourname, maneuvernumber, narrative, distance, formattedtime) VALUES(@name, @number, @narrative, @distance, @time)";

        private static string deleteRouteInfoSql = "DELETE FROM routeinfo WHERE tourname = @name";

        private string accessData { get; set; }

        public DatabaseRouteOrders()
        {
            accessData = ConfigurationManager.AppSettings["DatabaseAccess"].ToString();

        }

        public bool SaveRouteInfo(List<RawRouteInfo> routeInfoList)
        {

            bool check = true;
            NpgsqlConnection con = DataConnectionFactory.GetCon();
            con.Open();
            foreach (RawRouteInfo item in routeInfoList)
            {
                //var sql = "INSERT INTO routeinfo(tourname,maneuvernumber,narrative,distance,formattedtime) VALUES(@name, @number, @narrative, @distance, @time)";
                var sql = SaveRouteInfoSql;

                //con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Parameters.AddWithValue("name", item.tourName);
                cmd.Parameters.AddWithValue("number", item.maneuverNumber);
                cmd.Parameters.AddWithValue("narrative", item.narrative);
                cmd.Parameters.AddWithValue("distance", item.distance);
                cmd.Parameters.AddWithValue("time", item.formattedTime);
                cmd.Prepare();


                if (!DataConnectionFactory.GetDatabaseConnectionInstance().updateDatabaseData(cmd))
                {
                    check = false;
                    break;
                }
 
            }
            con.Close();
            return check;

        }


        public List<RawRouteInfo> GetRouteInfo(string tourName)
        {
            throw new NotImplementedException();
        }


        public bool DeleteRouteData(string tourName)
        {
            var sql = deleteRouteInfoSql;
            NpgsqlConnection con = DataConnectionFactory.GetCon();
            con.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("name", tourName);

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
