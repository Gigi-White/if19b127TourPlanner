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

        private string currentTourName;
       
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

        public void SetCurrentTourName(string currentTourName)
        {
            this.currentTourName = currentTourName;
        }
        public string GetCurrentTourName()
        {
            return currentTourName;
        }
    }
}
