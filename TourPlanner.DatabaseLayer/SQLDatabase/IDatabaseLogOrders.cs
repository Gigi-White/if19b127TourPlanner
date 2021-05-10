using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.SQLDatabase
{
    public interface IDatabaseLogOrders
    {
        bool createLog(Log newLog);

        List<Log> getLogsofTour(string tourname);

        bool updateLog(Log updatedLog);

        bool deleteOneLog(string logname);
        bool deleteAllLogsofTour(string tourname);
        bool copyLogsofTour(string tourname, string copiedtourname);

    }
}
