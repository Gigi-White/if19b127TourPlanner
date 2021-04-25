﻿using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.SQLDatabase
{
    public interface IDatabaseTourOrders
    {

        List<Tour> GetTours();

        bool SaveTours(Tour newTour);

        bool ChangeTour(Tour changedTour);
        bool CopyTour(string tourName);
        bool DeleteTour(string tourName);


    }
}
