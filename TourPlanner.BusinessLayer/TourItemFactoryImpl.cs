using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using TourPlanner.DataAccessLayer;
using TourPlanner.DataAccessLayer.SQLDatabase;
using TourPlanner.Models;
namespace TourPlanner.BusinessLayer
{
    public class TourItemFactoryImpl : ITourItemFactory
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Regex whitelist;
        private List<Tour> AllTours { get; set; }
        private Tour currentTour { get; set; }
        private List<RawRouteInfo> CurrentRouteInfo { get; set; }
        private  IDatabaseTourOrders mydatabaseTourOrders;
        private IDatabaseRouteOrders mydatabaseRouteOrders;
        private IDatabaseLogOrders mydatabaseLogOrders;
        private IHttpConnection myHttpConnection;
        private IFileHandler myFileHandler;
        private IHttpResponseHandler myResponseHandler;

        public TourItemFactoryImpl()
        {
            mydatabaseTourOrders =DataConnectionFactory.GetDatabaseToursInstance();
            mydatabaseRouteOrders = DataConnectionFactory.GetDatabaseRouteInstance();
            mydatabaseLogOrders = DataConnectionFactory.GetDatabaseLogInstance();
            myHttpConnection = DataConnectionFactory.GetHttpInstance();
            myFileHandler = DataConnectionFactory.GetFileHandlerInstance();
            myResponseHandler = new HttpResponseHandler();
            AllTours = mydatabaseTourOrders.GetTours();
            whitelist =new Regex (ConfigurationManager.AppSettings["Whitelist"].ToString());


        }
        public TourItemFactoryImpl(IDatabaseTourOrders databaseTourOrders,IDatabaseRouteOrders databaseRouteOrders, IHttpConnection httpConnection, IFileHandler filehandler, IHttpResponseHandler responseHandler, string thewhitelist)
        {
            mydatabaseTourOrders = databaseTourOrders;
            mydatabaseRouteOrders = databaseRouteOrders;
           myHttpConnection = httpConnection;
            myFileHandler = filehandler;
            myResponseHandler = responseHandler;
            AllTours = mydatabaseTourOrders.GetTours();
            whitelist = new Regex(thewhitelist);

        }

        public void SetCurrentTour(Tour viewModelCurrentTour)
        {
            if (viewModelCurrentTour != null) 
            {
                currentTour = viewModelCurrentTour;
            }
        }

        public string GetCurrentTourname()
        {
            if (currentTour != null)
            {
                return currentTour.Name;
            }
            else
            {
                return null;
            }
        }

        public string GetCurrentTourDescription()
        {
            if (currentTour != null)
            {
                string myText = myFileHandler.getDescription(currentTour.Descriptionfile);
                return myText;
            }
            else 
            {
                return null;
            }
        }

        public IEnumerable<Tour> GetTours()
        {
            return AllTours;
        }
        public IEnumerable<RawRouteInfo> GetRouteInfo(string tourname)
        {
            IEnumerable<RawRouteInfo> myRouteInfoList = mydatabaseRouteOrders.GetRouteInfo(tourname);
            return myRouteInfoList;
        }
        //Search Tours after certan value and search Option----------------------------------------------------
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


        //Create new Tour plus help functions---------------------------------------------------------------------------------
        public bool CreateTours(TourSearch newTourData)
        {

            string response = myHttpConnection.GetJsonResponse(newTourData);  //send "get" request and get json response
            myResponseHandler.SetJObject(response);
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


        public bool CheckText(string text)
        {
            if (text == null)
            {
                return false;
            }
            
            return whitelist.IsMatch(text);

        }


        //Delete Current Tour Code----------------------------------------------------------------------
        public bool DeleteCurrentTour(Tour currentTour)
        {          
            OnUpdateTourList();
            if (mydatabaseTourOrders.DeleteTour(currentTour.Name) && mydatabaseRouteOrders.DeleteRouteData(currentTour.Name) && mydatabaseLogOrders.deleteAllLogsofTour(currentTour.Name) && myFileHandler.DeleteDescription(currentTour.Descriptionfile) && myFileHandler.DeleteImage(currentTour.Imagefile))
            {
                AllTours.Remove(currentTour);
                OnUpdateTourList();
                return true;
            }
            else
            {
                return false;
            }


             
            
           
        }
        //Copy Current Tour Code----------------------------------------------------------------------
        public bool CopyCurrentTour(Tour currentTour)
        {

            Tour copiedTour = new Tour
            {
                Start = currentTour.Start,
                End = currentTour.End,
                Distance = currentTour.Distance,
                FormattedTime = currentTour.FormattedTime

            };
                      
            copiedTour.Name = currentTour.Name + "copy";

            copiedTour.Imagefile = currentTour.Imagefile.Replace(".png", "copy.png");
            copiedTour.Descriptionfile = currentTour.Descriptionfile.Replace(".txt", "copy.txt");

            copiedTour.CreationDate = System.DateTime.Now.ToString(@"dd\/MM\/yyyy h\:mm tt");

            
            if (!myFileHandler.CopyFile(currentTour.Imagefile, copiedTour.Imagefile))
            {

                return false;
            }

            if (!myFileHandler.CopyFile(currentTour.Descriptionfile, copiedTour.Descriptionfile))
            {
                return false;
            }

            if(!mydatabaseTourOrders.SaveTours(copiedTour))
            {
                return false;
            }

            if(!mydatabaseRouteOrders.CopyRouteInfo(currentTour.Name, copiedTour.Name))
            {
                return false;
            }

            if(!mydatabaseLogOrders.copyLogsofTour(currentTour.Name, copiedTour.Name))
            {
                return false;
            }
            
            
            AllTours.Add(copiedTour);
            OnUpdateTourList();

            return true;
        }

        
        
        public bool ModifyTour(string currentTourName, string changedTourName, string changedTourDescription)
        {
            bool worked = true;
            Tour toModifyTour = new Tour();
            foreach(var item in AllTours)
            {
                if (item.Name == currentTourName)
                {
                    toModifyTour = item;
                }

            }
            if (currentTourName != changedTourName)
            {
                worked = mydatabaseTourOrders.ChangeTour(currentTourName,changedTourName);
            }

            worked = myFileHandler.ChangeDescription(toModifyTour.Descriptionfile, changedTourDescription);
            if (worked)
            {
                toModifyTour.Name = changedTourName;
                OnUpdateTourList();
            }
            log.Info("Tour \"" + currentTourName + "\" was changed");

            return worked;
        }
    }
        
}