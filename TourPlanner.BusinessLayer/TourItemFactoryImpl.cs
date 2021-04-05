using Enums;
using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    internal class TourItemFactoryImpl : ITourItemFactory
    {
        public IEnumerable<Tour> GetTours()
        {
            return new List<Tour>
            {
                new Tour
                {
                    Name = "Gigi",
                    Start = "Wien",
                    End = "Berlin",
                    CreationDate = "26.04.2020",
                    Distance = 200
                },
                new Tour
                {
                    Name = "Mike",
                    Start = "Frankfurt",
                    End = "Berlin",
                    CreationDate = "27.04.2020",
                    Distance = 100
                },


            };
        }

        public IEnumerable<Tour> SearchTours(string word, TourData searchOption)
        {
            List<Tour> foundTours = new List<Tour>();
            IEnumerable<Tour> allTours = GetTours();

            switch(searchOption)
            {
                case TourData.Name:
                    foreach(var item in allTours)
                    {
                        if (item.Name.ToLower().Contains(word.ToLower()))
                        {
                            foundTours.Add(item);
                        }
                    }
                    break;

            };
            return foundTours;


        }
    }
}