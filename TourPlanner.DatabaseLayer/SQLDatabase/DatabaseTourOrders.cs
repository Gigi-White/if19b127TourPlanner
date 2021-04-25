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
           
            var sql = "SELECT * FROM tours";

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
                    CreationDate = rdr.GetString(6)
                    
                }
                );
            }
            con.Close();
            return MyTours;

            

        }
        //Save new Tour in Database----------------------------------------------------------------
        public bool SaveTours(Tour newTour)
        {

            var sql = "INSERT INTO tours(tourname,tourstart,tourend,tourdistance,creationdate,imagefile) VALUES(@name, @start, @end, @distance, @date, @image)";

            NpgsqlConnection con = DataConnectionFactory.GetCon();
            con.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("name", newTour.Name);
            cmd.Parameters.AddWithValue("start", newTour.Start);
            cmd.Parameters.AddWithValue("end", newTour.End);
            cmd.Parameters.AddWithValue("distance", newTour.Distance);
            cmd.Parameters.AddWithValue("date", newTour.CreationDate);
            cmd.Parameters.AddWithValue("image", newTour.Imagefile);
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


        // Save Route Data in Database and Folder------------------------------------------------

        public bool ChangeTour(Tour changedTour)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTour(string tourname)
        {
            throw new NotImplementedException();
        }
        public bool CopyTour(string tourname)
        {
            throw new NotImplementedException();
        }



    }
}
