using System;
using System.Configuration;
using TourPlanner.DataAccessLayer;

namespace TourPlanner.BusinessLayer
{
    public class TourItemFactory
    {
        private static ITourItemFactory instance;
        private static ILogItemFactory logInstance;

        public static ITourItemFactory GetMainViewInstance()
        {
            if (instance == null)
            {
                instance = new TourItemFactoryImpl(); 
            }
            return instance;
        }

        public static ILogItemFactory GetLogViewInstance()
        {
            if (logInstance == null)
            {
                logInstance = new LogItemFactoryImpl();
            }
            return logInstance;
        }
    }

}
