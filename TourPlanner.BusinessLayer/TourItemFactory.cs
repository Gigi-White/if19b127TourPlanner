using System;

namespace TourPlanner.BusinessLayer
{
    public class TourItemFactory
    {
        private static ITourItemFactory instance;

        public static ITourItemFactory GetInstance()
        {
            if (instance == null)
            {
                instance = new TourItemFactoryImpl(); 
            }
            return instance;
        }
    }

}
