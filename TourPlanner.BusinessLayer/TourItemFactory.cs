﻿using System;
using System.Configuration;
using TourPlanner.DataAccessLayer;

namespace TourPlanner.BusinessLayer
{
    public class TourItemFactory
    {
        private static ITourItemFactory instance;

        public static ITourItemFactory GetMainViewInstance()
        {
            if (instance == null)
            {
                instance = new TourItemFactoryImpl(); 
            }
            return instance;
        }
    }

}
