using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.DataAccessLayer;
using TourPlanner.DataAccessLayer.SQLDatabase;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    internal class LogItemFactoryImpl : ILogItemFactory
    {

        private IDatabaseLogOrders myDatabaseLogOrders;
        private IFileHandler myFileHandler;
        private List<Log> AllLogs { get; set; }
       
        public LogItemFactoryImpl()
        {
            myDatabaseLogOrders = DataConnectionFactory.GetDatabaseLogInstance();
            myFileHandler = DataConnectionFactory.GetFileHandlerInstance();
        }

        public IEnumerable<Log> GetLogs(string currentTourName)
        {
            AllLogs = myDatabaseLogOrders.getLogsofTour(currentTourName);
            return AllLogs;
        }
    }
}
