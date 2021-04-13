using System;
using System.Collections.Generic;
using TourPlanner.Models;
using System.Configuration;

namespace TourPlanner.DataAccessLayer
{

    internal class DatabaseConnection : IDatabaseConnection
    {
        private string accessData { get; set; }


        public DatabaseConnection()
        {
            accessData = ConfigurationManager.AppSettings["DatabaseAccess"].ToString();
        }
         ~DatabaseConnection()
        {
            // Your code
        }


        public IEnumerable<Tour> getTours()
        {
            throw new NotImplementedException();
        }

        public bool saveTour(Tour newTour)
        {
            throw new NotImplementedException();
        }

        public bool changeTour(Tour changedTour)
        {
            throw new NotImplementedException();
        }

    }
}
