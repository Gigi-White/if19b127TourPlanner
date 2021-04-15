using System;
using System.Collections.Generic;
using TourPlanner.Models;
using System.Configuration;
using Npgsql;
namespace TourPlanner.DataAccessLayer
{

    internal class DatabaseConnection : IDatabaseConnection
    {
        private string accessData { get; set; }
        private ImageHandler myImageHandler;


        public DatabaseConnection()
        {
            accessData = ConfigurationManager.AppSettings["DatabaseAccess"].ToString();
            myImageHandler = new ImageHandler();

        }
         ~DatabaseConnection()
        {
            // Your code
        }

        //Get all Tours form Database---------------------------------------------------------
        public List<Tour> GetTours()
        {
            try
            {
                NpgsqlConnection con = new NpgsqlConnection(accessData);
                con.Open();

                var sql = "SELECT * FROM tours";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
                cmd.Prepare();

                NpgsqlDataReader rdr = cmd.ExecuteReader();

                List<Tour> MyTours = new List<Tour>();
                while (rdr.Read())
                {
                    MyTours.Add(new Tour
                    {
                        Name = rdr.GetString(1),
                        Start = rdr.GetString(2),
                        End = rdr.GetString(3),
                        CreationDate = rdr.GetDate(5).ToString(),
                        Distance = rdr.GetInt32(4)
                    }
                    );                 
                }
                return MyTours;

            }
            catch (Exception)
            {
                Console.WriteLine("Error while trying to insert in player");
                return null;
            }
        }
        //Save new Tour in Database----------------------------------------------------------------
        public bool SaveTour(Tour newTour)
        {
            throw new NotImplementedException();
        }


        // Save Route Data in Database and Folder------------------------------------------------
        public bool SaveTourRouteData(List<RawRouteInfo> RouteInfo)
        {

            List<RawRouteInfo> CompleteRouteInfo = myImageHandler.DownloadSaveImages(RouteInfo);



            return true;
        }




        public bool ChangeTour(Tour changedTour)
        {
            throw new NotImplementedException();
        }

 

    }
}
