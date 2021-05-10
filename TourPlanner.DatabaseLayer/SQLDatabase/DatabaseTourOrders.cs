using System;
using System.Collections.Generic;
using TourPlanner.Models;
using System.Configuration;
using Npgsql;
using TourPlanner.DataAccessLayer.SQLDatabase;

namespace TourPlanner.DataAccessLayer
{

    internal class DatabaseTourOrders : IDatabaseTourOrders
    {

        private string accessData { get; set; }
        private static string getAllToursSql = "SELECT* FROM tours";
        private static string saveToursSql = "INSERT INTO tours(tourname,tourstart,tourend,tourdistance,creationdate,imagefile,descriptionfile,formattedtime ) VALUES(@name, @start, @end, @distance, @date, @image, @description,@formattedtime)";
        private static string deleteTourSql = "DELETE FROM tours WHERE  tourname = @name";
        public DatabaseTourOrders()
        {
            accessData = ConfigurationManager.AppSettings["DatabaseAccess"].ToString();

        }
         ~DatabaseTourOrders()
        {
            // Your code
        }

        //Get all Tours form Database---------------------------------------------------------
        public List<Tour> GetTours()
        {
           
            var sql = getAllToursSql;

            //NpgsqlDataReader rdr = DataConnectionFactory.GetDatabaseConnectionInstance().getDatabaseData(sql);
            NpgsqlConnection con = DataConnectionFactory.GetCon();
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            cmd.Prepare();

            NpgsqlDataReader rdr = DataConnectionFactory.GetDatabaseConnectionInstance().getDatabaseData(cmd);
            if (rdr == null)
            {
                return null;
            }

            List<Tour> MyTours = new List<Tour>();
            while (rdr.Read())
            {
                MyTours.Add(new Tour
                {
                    Name = rdr.GetString(1),
                    Start = rdr.GetString(2),
                    End = rdr.GetString(3),                    
                    Distance = rdr.GetInt32(4),
                    Imagefile = rdr.GetString(5),
                    CreationDate = rdr.GetString(6),
                    Descriptionfile = rdr.GetString(7),
                    FormattedTime = rdr.GetString(8)

                }
                );
            }
            con.Close();
            return MyTours;

            

        }
        //Save new Tour in Database----------------------------------------------------------------
        public bool SaveTours(Tour newTour)
        {

            //var sql = "INSERT INTO tours(tourname,tourstart,tourend,tourdistance,creationdate,imagefile) VALUES(@name, @start, @end, @distance, @date, @image)";
            var sql = saveToursSql;
            NpgsqlConnection con = DataConnectionFactory.GetCon();
            con.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("name", newTour.Name);
            cmd.Parameters.AddWithValue("start", newTour.Start);
            cmd.Parameters.AddWithValue("end", newTour.End);
            cmd.Parameters.AddWithValue("distance", newTour.Distance);
            cmd.Parameters.AddWithValue("date", newTour.CreationDate);
            cmd.Parameters.AddWithValue("image", newTour.Imagefile);
            cmd.Parameters.AddWithValue("description", newTour.Descriptionfile);
            cmd.Parameters.AddWithValue("formattedtime", newTour.FormattedTime);
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


       

        public bool ChangeTour(Tour changedTour)
        {
            throw new NotImplementedException();
        }



        //Delete Tour Data
        public bool DeleteTour(string tourname)
        {
            //var sql = "INSERT INTO tours(tourname,tourstart,tourend,tourdistance,creationdate,imagefile) VALUES(@name, @start, @end, @distance, @date, @image)";
            var sql = deleteTourSql;
            NpgsqlConnection con = DataConnectionFactory.GetCon();
            con.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("name", tourname);
            
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



        public bool CopyTour(string tourname)
        {
            throw new NotImplementedException();
        }



    }
}
