using System;
using TourPlanner.DataAccessLayer;

namespace TourPlanner.BusinessLayer
{
    public class TourItemFactory
    {
        private static ITourItemFactory instance;

        public static ITourItemFactory GetMainViewInstance()
        {
            if (instance == null)
            {
                instance = new TourItemFactoryImpl(DataConnectionFactory.GetDatabaseToursInstance(),
                    DataConnectionFactory.GetHttpInstance(), DataConnectionFactory.GetFileHandlerInstance(),new HttpResponseHandler()); 
            }
            return instance;
        }
    }

}
