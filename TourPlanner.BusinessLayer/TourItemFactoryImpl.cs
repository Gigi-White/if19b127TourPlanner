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


        public TourItemFactoryImpl()
        {

            AllTours = DataConnectionFactory.GetDatabaseToursInstance().GetTours();


            //Testbereich für Http Request------------------------
            TourSearch Test = new TourSearch
            {
                newTourName = "TestTour",
                fromCity = "Vienna",
                fromCountry = "Austria",
                toCity = "Graz",
                toCountry = "Austria"
            };

            
            string request = DataConnectionFactory.GetHttpInstance().GetJsonResponse(Test);  //send "get" request and get json response
            HttpResponseHandler ResponseHandler = new HttpResponseHandler(request);

            NewRouteInfo = ResponseHandler.GrabRouteData(Test.newTourName);  //getroute info out of json response

            MainMapSearchData mainMap = ResponseHandler.GrabMainMapData(); //getmapdata out of jaosn response


            string imagepath = DataConnectionFactory.GetFileHandlerInstance().DownloadSaveImage(mainMap, Test.newTourName); //download and save image


            //fill the new created tour with data

            Tour newTour = ResponseHandler.GrabMainTimeAndDistance();
            newTour.Name = Test.newTourName;
            newTour.Start = Test.fromCity;
            newTour.End = Test.toCity;
            newTour.CreationDate = DateTime.Now.ToString(@"dd\/MM\/yyyy h\:mm tt");
            newTour.Imagefile = imagepath;

            //save tour in database

            DataConnectionFactory.GetDatabaseToursInstance().SaveTours(newTour);

            //DataConnectionFactory.GetDatabaseToursInstance().SaveTourRouteData(NewRouteInfo);
            //Databasehandler.SaveTourRouteData(NewRouteInfo);


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