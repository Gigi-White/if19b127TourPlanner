using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.SQLDatabase
{
    public interface IDatabaseRouteOrders
    {
        bool SaveRouteInfo(List<RawRouteInfo> routeInfoList);
        List<RawRouteInfo> GetRouteInfo(string tourName);

        bool DeleteRouteData(string tourName);
     

    }
}
