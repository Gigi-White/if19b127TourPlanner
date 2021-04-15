using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public interface IDatabaseConnection
    {
        List<Tour> GetTours();
        bool SaveTour(Tour newTour);

        bool ChangeTour(Tour changedTour);

        bool SaveTourRouteData(List<RawRouteInfo> RouteInfo);
    }
}
