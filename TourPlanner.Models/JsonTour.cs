using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models
{
    public class JsonTour
    {
        public Tour tourData{ get; set; }

        public List<RawRouteInfo> routeData { get; set; }

        public List<Log> logData { get; set; }
    }
}
