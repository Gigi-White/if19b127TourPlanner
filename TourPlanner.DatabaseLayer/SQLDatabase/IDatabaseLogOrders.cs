using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.SQLDatabase
{
    public interface IDatabaseLogOrders
    {
        bool CreateLog(Log newLog);

        List<Log> GetLogsofTour(string tourname);

        bool UpdateLog(Log updatedLog);

        bool DeleteOneLog(string tourname, string logname);
        bool DeleteAllLogsofTour(string tourname);
        bool CopyLogsofTour(string tourname, string copiedtourname);

    }
}
