using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models
{
    public class Log
    {
        public string tourname {get; set; }
        public string logname {get; set; }
        public string date { get; set; }
        public string reportfile { get; set; }
        public string distance { get; set; }
        public string totalTime { get; set; }
        public int rating { get; set; }
        public string travelBy { get; set; }
        public string averageSpeed { get; set; }
        public string recommandRestaurant { get; set; }
        public string recommandHotel { get; set; }
        public string sightWorthSeeing { get; set; }

    }
}
