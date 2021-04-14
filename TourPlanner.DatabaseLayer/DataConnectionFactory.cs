using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.DataAccessLayer
{
    public class DataConnectionFactory
    {
        private static IDatabaseConnection databaseinstance;
        private static IHttpConnection httpinstance;

        public static IDatabaseConnection GetdatabaseInstance()
        {
            if (databaseinstance == null)
            {
                databaseinstance = new DatabaseConnection();
            }
            return databaseinstance;
        }

    }
}
