
using Npgsql;
using System.Configuration;
using TourPlanner.DataAccessLayer.SQLDatabase;

namespace TourPlanner.DataAccessLayer
{
    public class DataConnectionFactory
    {
        private static IDatabaseTourOrders databaseTourOrdersInstance;
        private static IHttpConnection httpConnectionInstance;
        //private ImageHandler imageHandleInstance;
        private static IDatabaseConnection databaseConnectionInstance;
        private static IDatabaseRouteOrders databaseRouteOrdersInstance;
        private static IFileHandler fileHandlerInstance;
        private static NpgsqlConnection con;


        public static IDatabaseTourOrders GetDatabaseToursInstance()
        {
            if (databaseTourOrdersInstance == null)
            {
                databaseTourOrdersInstance = new DatabaseTourOrders();
            }
            return databaseTourOrdersInstance;
        }

        public static IDatabaseRouteOrders GetDatabaseRouteInstance()
        {
            if (databaseRouteOrdersInstance == null)
            {
                databaseRouteOrdersInstance = new DatabaseRouteOrders();
            }
            return databaseRouteOrdersInstance;
        }

        public static IHttpConnection GetHttpInstance()
        {
            if (httpConnectionInstance == null)
            {
                httpConnectionInstance = new HttpConnection();
            }
            return httpConnectionInstance;
        }
        public static IDatabaseConnection GetDatabaseConnectionInstance()
        {
            if (databaseConnectionInstance == null)
            {
                databaseConnectionInstance = new DatabaseConnection();
            }
            return databaseConnectionInstance;
        }

        public static IFileHandler GetFileHandlerInstance()
        {
            if (fileHandlerInstance == null)
            {
                fileHandlerInstance = new FileHandler();
            }
            return fileHandlerInstance;
        }


        public static NpgsqlConnection GetCon()
        {
            if (con == null)
            {
                string accessData = ConfigurationManager.AppSettings["DatabaseAccess"].ToString();
                con = new NpgsqlConnection(accessData);
            }
            return con;
        }


    }
}
