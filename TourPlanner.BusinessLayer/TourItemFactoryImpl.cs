using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using TourPlanner.DataAccessLayer;
using TourPlanner.DataAccessLayer.SQLDatabase;
using TourPlanner.Models;
using VisioForge.MediaFramework.ONVIF;

namespace TourPlanner.BusinessLayer
{
    public class TourItemFactoryImpl : ITourItemFactory
    {

        private Regex whitelist;
        private List<Tour> AllTours { get; set; }
        private List<RawRouteInfo> CurrentRouteInfo { get; set; }
        private  IDatabaseTourOrders mydatabaseTourOrders;
        private IDatabaseRouteOrders mydatabaseRouteOrders;
        private IHttpConnection myHttpConnection;
        private IFileHandler myFileHandler;
        private IHttpResponseHandler myResponseHandler;

        public TourItemFactoryImpl()
        {
            mydatabaseTourOrders =DataConnectionFactory.GetDatabaseToursInstance();
            mydatabaseRouteOrders = DataConnectionFactory.GetDatabaseRouteInstance();
            myHttpConnection = DataConnectionFactory.GetHttpInstance();
            myFileHandler = DataConnectionFactory.GetFileHandlerInstance();
            myResponseHandler = new HttpResponseHandler();
            AllTours = mydatabaseTourOrders.GetTours();
            whitelist =new Regex (ConfigurationManager.AppSettings["Whitelist"].ToString());

        }
        public TourItemFactoryImpl(IDatabaseTourOrders databaseTourOrders, IHttpConnection httpConnection, IFileHandler filehandler, IHttpResponseHandler responseHandler, string thewhitelist)
        {
            mydatabaseTourOrders = databaseTourOrders;
            myHttpConnection = httpConnection;
            myFileHandler = filehandler;
            myResponseHandler = responseHandler;
            AllTours = mydatabaseTourOrders.GetTours();
            whitelist = new Regex(thewhitelist);

        }


        public IEnumerable<Tour> GetTours()
        {

            return AllTours;
        }
        //Search Tours after certen value and searchOtion----------------------------------------------------
        public IEnumerable<Tour> SearchTours(string word, string searchOption)
        {
            List<Tour> foundTours = new List<Tour>();

            switch (searchOption)
            {
                case "Name":
                    foreach (var item in AllTours)
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

        private UpdateToursEventHandler UpdatingTourList;


        public void setUpdateToursEventhandler(UpdateToursEventHandler newEvent)
        {
            UpdatingTourList += newEvent;
        }

        protected virtual void OnUpdateTourList()
        {
            if (UpdatingTourList!=null)
            {
                UpdatingTourList(this, EventArgs.Empty);
            }

        }



        public bool CreateTours(TourSearch newTourData)
        {

            string request = myHttpConnection.GetJsonResponse(newTourData);  //send "get" request and get json response
            myResponseHandler.SetJObject(request);
            List<RawRouteInfo> NewRouteInfoList = new List<RawRouteInfo>();
            NewRouteInfoList = myResponseHandler.GrabRouteData(newTourData.newTourName);  //getroute info out of json response

            Tour newTour = FillNewTour(newTourData);
            
            //save tour in database
            mydatabaseTourOrders.SaveTours(newTour);
            mydatabaseRouteOrders.SaveRouteInfo(NewRouteInfoList);

            AllTours.Add(newTour);
            OnUpdateTourList();
            return true;
        }



        private Tour FillNewTour(TourSearch newTourData)
        {
            MainMapSearchData mainMap = myResponseHandler.GrabMainMapData(); //getmapdata out of jaosn response
            string imagepath = myFileHandler.DownloadSaveImage(mainMap, newTourData.newTourName); //download and save image
            string descriptionpath = myFileHandler.SaveDescription(newTourData.tourDescription, newTourData.newTourName);
            //fill the new created tour with data
            Tour newTour = myResponseHandler.GrabMainTimeAndDistance();
            newTour.Name = newTourData.newTourName;
            newTour.Start = newTourData.fromCity;
            newTour.End = newTourData.toCity;
            newTour.CreationDate = System.DateTime.Now.ToString(@"dd\/MM\/yyyy h\:mm tt");
            newTour.Imagefile = imagepath;
            newTour.Descriptionfile = descriptionpath;

            return newTour;
        }

        public bool CheckNewTourData(TourSearch info)
        {
            if (!CheckText(info.fromCity) || !CheckText(info.fromCountry) || !CheckText(info.toCity) || !CheckText(info.toCountry)|| !CheckText(info.tourDescription))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        private bool CheckText(string text)
        {
            return whitelist.IsMatch(text);

        }

    }
}