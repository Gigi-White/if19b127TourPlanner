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
        private List<RawRouteInfo> NewRouteInfo { get; set; }

        private IDatabaseConnection Databasehandler;
        private IHttpConnection HttpRequestHandler;

        public TourItemFactoryImpl()
        {

            Databasehandler = DataConnectionFactory.GetDatabaseInstance();
            HttpRequestHandler = DataConnectionFactory.GetHttpInstance();
            AllTours = Databasehandler.GetTours();


            //Testbereich für Http Request------------------------
            TourSearch Test = new TourSearch
            {
                newTourName="TestTour",
                fromCity = "Vienna",
                fromCountry = "Austria",
                toCity = "Graz",
                toCountry = "Austria"
            };

            
            string request = HttpRequestHandler.GetJsonResponse(Test);
            HttpResponseHandler ResponseHandler = new HttpResponseHandler(request);

            NewRouteInfo = ResponseHandler.GrabRouteData(Test.newTourName);

            MainMapSearchData mainMap = ResponseHandler.GrabMainMapData();

            Databasehandler.SaveTourRouteData(NewRouteInfo);
            

        }


        public IEnumerable<Tour> GetTours()
        {

            return AllTours;
        }
        //Search Tours after certen value and searchOtion----------------------------------------------------
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