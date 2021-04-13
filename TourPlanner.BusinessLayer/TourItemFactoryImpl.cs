using Enums;
using System;
using System.Collections.Generic;
using TourPlanner.DataAccessLayer;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    internal class TourItemFactoryImpl : ITourItemFactory
    {
        private List<Tour> AllTours { get; set; }

        private IDatabaseConnection Databasehandler;

        public TourItemFactoryImpl()
        {

            Databasehandler = DataConnectionFactory.GetInstance();
            AllTours = Databasehandler.getTours();


            /*AllTours = new List<Tour>
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

            */

        }


        public IEnumerable<Tour> GetTours()
        {

            return AllTours;
        }
        //Toursuche nach bestimmten wert und Suchoption----------------------------------------------------
        public IEnumerable<Tour> SearchTours(string word, string searchOption)
        {
            List<Tour> foundTours = new List<Tour>();


            switch(searchOption)
            {
                case "Name":
                    foreach(var item in AllTours)
                    {
                        if (item.Name.ToLower().Contains(word.ToLower()))
                        {
                            foundTours.Add(item);
                        }
                    }
                    break;
                case "Start":
                    foreach (var item in AllTours)
                    {
                        if (item.Start.ToLower().Contains(word.ToLower()))
                        {
                            foundTours.Add(item);
                        }
                    }
                    break;
                case "End":
                    foreach (var item in AllTours)
                    {
                        if (item.End.ToLower().Contains(word.ToLower()))
                        {
                            foundTours.Add(item);
                        }
                    }
                    break;
                case "Distance":
                    foundTours = SearchWithDistance(word);
                    break;



            };
            return foundTours;


        }

        //Search Tour with distance-----------------------------------------------------------------
        private List<Tour> SearchWithDistance(string number)
        {
            List<Tour> foundTours = new List<Tour>();
            
            try
            {
                float checkdistance = float.Parse(number);
                foreach (var item in AllTours)
                {
                    
                    if (checkdistance <= item.Distance)
                    {
                        foundTours.Add(item);
                    }
                }


                return foundTours;
            }
            catch (Exception)
            {
                return null;
            }

           
        }
    }
}