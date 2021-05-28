using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
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
        private IDatabaseTourOrders mydatabaseTourOrders;
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
                string myText = myFileHandler.GetFileText(currentTour.Descriptionfile);
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
            List<Log> myLogList = mydatabaseLogOrders.GetLogsofTour(currentTour.Name);

            if (!mydatabaseTourOrders.DeleteTour(currentTour.Name)) {
                return false;
            }
            if (!myFileHandler.DeleteFile(currentTour.Descriptionfile))
            {
                return false;
            }
            if (!myFileHandler.DeleteImage(currentTour.Imagefile))
            {
                return false;
            }
            if (!myFileHandler.DeleteImage(currentTour.Imagefile))
            {
                return false;
            }
            foreach(var item in myLogList)
            {
                if (!myFileHandler.DeleteFile(item.reportfile))
                {
                    return false;
                }
            }

            
            AllTours.Remove(currentTour);
            OnUpdateTourList();
            return true;
            


             
            
           
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

            //check if tour with copy name already exists 
            foreach(var item in AllTours)
            {
                if(item.Name == copiedTour.Name)
                {
                    return false;
                }
            }

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

            if (!HandleCopieLogs(copiedTour.Name))
            {
                return false;
            }
            
            AllTours.Add(copiedTour);
            OnUpdateTourList();

            return true;
        }

        //Help Function to copy All Logs of the Tour

        private bool HandleCopieLogs(string copiedTourName)
        {
            try
            {


                List<Log> myLogList = mydatabaseLogOrders.GetLogsofTour(currentTour.Name);
                foreach (var item in myLogList)
                {
                    item.tourname = copiedTourName;
                    string currentReportPath = item.reportfile;
                    item.reportfile = item.reportfile.Replace(".txt", "copy.txt");
                    myFileHandler.CopyFile(currentReportPath, item.reportfile);
                    mydatabaseLogOrders.CreateLog(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Modify current Tour------------------------------------------------------------------------------------
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
            //check if user wants to change the tourname
            if (currentTourName != changedTourName)
            {
                //if yes, first change the tourname in the database
                worked = mydatabaseTourOrders.ChangeTour(currentTourName,changedTourName);
                //then change the toruname for the Logs in the database + the name of the report file
                worked = HandlechangeTourLogs(currentTourName, changedTourName);
            }
            //change the text of the description file
            worked = myFileHandler.ChangeFile(toModifyTour.Descriptionfile, changedTourDescription);
            if (worked)
            {
                toModifyTour.Name = changedTourName;
                OnUpdateTourList();
            }
            log.Info("Tour \"" + currentTourName + "\" was changed");

            return worked;
        }


        private bool HandlechangeTourLogs(string currentTourName, string changedTourName)
        {
            //first get all logs of the tour who s name gets changed
            List<Log> myLogList = mydatabaseLogOrders.GetLogsofTour(changedTourName);
            //get the new toruname for the logs and the report file and save both in the database + change the name of the report txt file 
            foreach(var item in myLogList)
            {
                string oldFilename = item.reportfile;
                item.reportfile = item.reportfile.Replace(currentTourName + item.logname + ".txt", changedTourName + item.logname + ".txt");
                if (!myFileHandler.CopyFile(oldFilename, item.reportfile)|| !myFileHandler.DeleteFile(oldFilename))
                {
                    return false;
                }

            }

            return true;
        }


        //------------------------------------------------------------------------------------------

        //Create Tour Report------------------------------------------------------------------------

        public bool CreateReport(Tour currentTour)
        {
            List<RawRouteInfo> routeList = mydatabaseRouteOrders.GetRouteInfo(currentTour.Name);
            List<Log> logList = mydatabaseLogOrders.GetLogsofTour(currentTour.Name);

            if(!myFileHandler.CreateTourReport(currentTour, routeList, logList))
            {
                return false;
            }
            if(!myFileHandler.CreateSummarizeReport(currentTour, logList))
            {
                return false;
            }

            return true;
        }
        //--------------------------------------------------------------------------------------------


        //Export Tour--------------------------------------------------------------------------------
        public bool ExportTour(Tour currentTour)
        {

            //get all data from the current tour: Route and Log Data
            List<RawRouteInfo> routeList = mydatabaseRouteOrders.GetRouteInfo(currentTour.Name);
            List<Log> logList = mydatabaseLogOrders.GetLogsofTour(currentTour.Name);

            //create a new Tour where you can change the data without changing the current tour data
            Tour myCurrentTour = new Tour
            {
                Name = currentTour.Name,
                Start = currentTour.Start,
                End = currentTour.End,
                CreationDate = currentTour.CreationDate,
                Distance = currentTour.Distance,
                FormattedTime = currentTour.FormattedTime,
                Imagefile = currentTour.Imagefile,
                Descriptionfile = currentTour.Descriptionfile
            };


            //get the image and save it as 64Byte string with helpfunction "ConvertImageToString"
            myCurrentTour.Imagefile = ConvertImageToString(myCurrentTour.Imagefile);
            if (myCurrentTour.Imagefile == "")
            {
                return false;
            }

            //get the Tour Description 
            myCurrentTour.Descriptionfile = myFileHandler.GetFileText(myCurrentTour.Descriptionfile);

            //get the Log Reports
            foreach (var item in logList)
            {
                item.reportfile =myFileHandler.GetFileText(item.reportfile);
            }

            //save all in a Json Tour object
            JsonTour exportData = new JsonTour();
            exportData.tourData = myCurrentTour;
            exportData.routeData = routeList;
            exportData.logData = logList;

            //Export the Tour 

            if (!myFileHandler.ExportTour(exportData))
            {
                return false;
            }

            return true;
        }

        //heplfunction to convert image to base64string-------------
        private string ConvertImageToString(string imagefile)
        {
            try
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(imagefile);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                return base64ImageRepresentation;
            }
            catch
            {
                return "";
            }
        }
        //Import Tour from Json File---------------------------------------------------------------
        public string ImportTour(string jsonFile)
        {
            string filepath = ConfigurationManager.AppSettings["FolderDirectory"].ToString();
            string jsonFilePath = filepath + "\\jsonFolder\\" + jsonFile;
            //check if file exists and check if it is a json file
            if (!myFileHandler.CheckJsonFile(jsonFilePath))
            {
                return "This file does not exist in the folder";
            }
            JObject myJsonTour= myFileHandler.GetJsonFile(jsonFilePath);
            if(myJsonTour == null)
            {
                return "This file is not a JSON file";
            }
            //check if the json file has the right schema

           
            JSchemaGenerator generator = new JSchemaGenerator();  //create schema 

            JSchema jsonTourSchema = generator.Generate(typeof(JsonTour));

            if (!myJsonTour.IsValid(jsonTourSchema))
            {
                return "This has not the right JSON schema";
            }
            //check if tour already exists
            foreach (var item in AllTours)
            {
                if (item.Name == myJsonTour["tourData"]["Name"].ToString())
                {
                    return "This file already exists";
                }
            }

            //create the imagefile and save it in folder
            string base64Image = myJsonTour["tourData"]["Imagefile"].ToString();

            string imageFilePath = myFileHandler.SaveImage(myJsonTour["tourData"]["Name"].ToString(), base64Image);

            if (imageFilePath == "")
            {
                return "There was a error in the system";
            }

            //create the text file for the tour
            string description = myJsonTour["tourData"]["Descriptionfile"].ToString();
            string descriptionFile = myFileHandler.SaveDescription(description, myJsonTour["tourData"]["Name"].ToString());

            //fill logs
            List<Log> logList = new List<Log>();
            foreach (var mylog  in myJsonTour["logData"])
            {
                logList.Add(new Log
                {
                    tourname = mylog["tourname"].ToString(),
                    logname = mylog["logname"].ToString(),
                    date = mylog["date"].ToString(),
                    reportfile = mylog["reportfile"].ToString(),
                    distance = mylog["distance"].ToString(),
                    totalTime = mylog["totalTime"].ToString(),
                    rating = int.Parse(mylog["rating"].ToString()),
                    travelBy = mylog["totalTime"].ToString(),
                    averageSpeed = mylog["averageSpeed"].ToString(),
                    recommandRestaurant = mylog["recommandRestaurant"].ToString(),
                    recommandHotel = mylog["recommandHotel"].ToString(),
                    sightWorthSeeing = mylog["recommandRestaurant"].ToString()
                }

                );

            }
            //create log files
            foreach (var item in logList)
            {
                item.reportfile = myFileHandler.SaveReport(item.reportfile, item.tourname, item.logname);
            }

            //create tour object and rawrouteinfo objects
            Tour myNewTour = new Tour();

            myNewTour.Name = myJsonTour["tourData"]["Name"].ToString();
            myNewTour.Start = myJsonTour["tourData"]["Start"].ToString();
            myNewTour.End = myJsonTour["tourData"]["End"].ToString();
            myNewTour.CreationDate = myJsonTour["tourData"]["CreationDate"].ToString();
            myNewTour.Distance = float.Parse(myJsonTour["tourData"]["Distance"].ToString());
            myNewTour.FormattedTime = myJsonTour["tourData"]["FormattedTime"].ToString();
            myNewTour.Imagefile = imageFilePath;
            myNewTour.Descriptionfile = descriptionFile;

            List<RawRouteInfo> routeList = new List<RawRouteInfo>();
            foreach (var route in myJsonTour["routeData"])
            {
                routeList.Add(new RawRouteInfo
                {
                    tourName = route["tourName"].ToString(),
                    maneuverNumber = int.Parse(route["maneuverNumber"].ToString()),
                    narrative = route["narrative"].ToString(),
                    distance = float.Parse(route["distance"].ToString()),
                    formattedTime = route["formattedTime"].ToString()

                }
                );
                
            }
            //save everything in the database

            if (!mydatabaseTourOrders.SaveTours(myNewTour))                      
            {
                return "There was a error in the system";
            }
            
            foreach (var item in logList)
            {
                if (!mydatabaseLogOrders.CreateLog(item)) 
                {
                    return "There was a error in the system";
                };
            }
            if (!mydatabaseRouteOrders.SaveRouteInfo(routeList))
            {
                return "There was a error in the system";
            }


            AllTours.Add(myNewTour);
            OnUpdateTourList();
            return "";
        }

        //------------------------------------------------------------------------------------------

    }

}