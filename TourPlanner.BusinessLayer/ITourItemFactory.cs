using Enums;
using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public interface ITourItemFactory
    {
        IEnumerable<Tour> GetTours();

        IEnumerable<Tour> SearchTours(string word, TourData searchOption);
    }
}
