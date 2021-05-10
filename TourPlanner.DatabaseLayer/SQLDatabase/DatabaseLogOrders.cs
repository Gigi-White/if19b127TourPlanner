using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.SQLDatabase
{
    internal class DatabaseLogOrders : IDatabaseLogOrders
    {
       

        public bool createLog(Log newLog)
        {
            throw new NotImplementedException();
        }

        public bool deleteAllLogsofTour(string tourname)
        {
            return true;
        }

        public bool deleteOneLog(string logname)
        {
            throw new NotImplementedException();
        }

        public List<Log> getLogsofTour(string tourname)
        {
            throw new NotImplementedException();
        }

        public bool updateLog(Log updatedLog)
        {
            throw new NotImplementedException();
        }

        public bool copyLogsofTour(string tourname, string copiedtourname)
        {
            return true;
        }
    }
}
