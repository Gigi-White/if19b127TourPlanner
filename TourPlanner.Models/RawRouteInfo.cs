using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models
{
    public class RawRouteInfo
    {
        public string tourName { get; set; } 
        public int maneuverNumber { get; set; }
        public string narrative { get; set; }
        public float distance { get; set; }
        public string formattedTime { get; set; }


    }
}
