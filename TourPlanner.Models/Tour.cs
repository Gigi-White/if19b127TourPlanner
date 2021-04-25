using System;

namespace TourPlanner.Models
{
    public class Tour
    {
        public string Name { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string CreationDate { get; set; }
        public float Distance { get; set; }
        public string FormattedTime { get; set; }
        public string Imagefile { get; set; }

        public string Descriptionfile { get; set; }
    }
}
