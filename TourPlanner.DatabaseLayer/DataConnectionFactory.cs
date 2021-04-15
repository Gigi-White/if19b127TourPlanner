using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.DataAccessLayer
{
    public class DataConnectionFactory
    {
        private static IDatabaseConnection databaseinstance;
        private static IHttpConnection httpinstance;
        private ImageHandler myImageHandle;

        public static IDatabaseConnection GetDatabaseInstance()
        {
            if (databaseinstance == null)
            {
                databaseinstance = new DatabaseConnection();
            }
            return databaseinstance;
        }
        public static IHttpConnection GetHttpInstance()
        {
            if (httpinstance == null)
            {
                httpinstance = new HttpConnection();
            }
            return httpinstance;
        }

    }
}
