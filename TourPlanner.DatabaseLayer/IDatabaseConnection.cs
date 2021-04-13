using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public interface IDatabaseConnection
    {
        List<Tour> getTours();
        bool saveTour(Tour newTour);

        bool changeTour(Tour changedTour);
    }
}
