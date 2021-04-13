using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.DataAccessLayer
{
    public class DataConnectionFactory
    {
        private static IDatabaseConnection instance;

        public static IDatabaseConnection GetInstance()
        {
            if (instance == null)
            {
                instance = new DatabaseConnection();
            }
            return instance;
        }

    }
}
