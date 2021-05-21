using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.DataAccessLayer;
using TourPlanner.DataAccessLayer.SQLDatabase;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    internal class LogItemFactoryImpl : ILogItemFactory
    {

        private IDatabaseLogOrders myDatabaseLogOrders;
        private IFileHandler myFileHandler;
        private List<Log> AllLogs { get; set; }

        private Log currentLog;

       

        private string currentTourName;
       
        //constructor----------------------------------------------------------
        public LogItemFactoryImpl()
        {
            myDatabaseLogOrders = DataConnectionFactory.GetDatabaseLogInstance();
            myFileHandler = DataConnectionFactory.GetFileHandlerInstance();
        }
        //----------------------------------------------------------------------

        //Set Event Handler for Update of Loglist-------------------------------
        private UpdateLogsEventHandler UpdatingLogList;


        public void setUpdateLogsEventhandler(UpdateLogsEventHandler newEvent)
        {
            UpdatingLogList += newEvent;
        }

        protected virtual void OnUpdateLogList()
        {
            if (UpdatingLogList != null)
            {
                UpdatingLogList(this, EventArgs.Empty);
            }

        }

        //--------------------------------------------------------------------------


        //get logs of current Tour
        public IEnumerable<Log> GetLogs(string currentTourName)
        {
            //AllLogs.Clear();
            AllLogs = myDatabaseLogOrders.GetLogsofTour(currentTourName);
                     
            return AllLogs;
        }

        //get and set current Tour name-----------------------------------
        public string GetCurrentTourName()
        {
            return currentTourName;
        }

        public void SetCurrentTourName(string currentTourName)
        {
            this.currentTourName = currentTourName;
        }


        //---------------------------------------------------------------


        //create new Tour Log---------------------------------------------

        //main function
        public string CreateNewTourLog(Log myNewLog, string report)
        {
            //check if important parts have values
            string message = IsEmpty(myNewLog, report);
            if (message != null)
            {
                return message;
            }

            //check if Log already existst
            if (!LogAlreadyExists(myNewLog.logname))
            {
                return "A Tour Log with the name " + myNewLog.logname + " already exists";
            }

            //fill not important parts of Log if they have no value
            myNewLog = FillLog(myNewLog);

            //create new report file and save the report in file;
            string reportPath = myFileHandler.SaveReport(report, myNewLog.tourname, myNewLog.logname);                   
            myNewLog.reportfile = reportPath;
            //save the new log in the database
            if (myDatabaseLogOrders.CreateLog(myNewLog))
            {
                OnUpdateLogList();
                return "true";
                
            }
            
            else
            {
                return "There was a error in during the procedure";
            }


        }

        //check if important parts have values
        private string IsEmpty(Log myNewLog, string report) 
        {
            if(     myNewLog.logname==null|| myNewLog.logname == ""
                ||  myNewLog.distance == null || myNewLog.distance == ""
                ||  myNewLog.totalTime == null || myNewLog.totalTime == ""
                ||  report ==null || report == "" )
                {
                return "Please fill out all fields with * Symbol";
                }
            else
            {
                return null;
            }
        }
        
        //check if Log with certan name already exitst
        private bool LogAlreadyExists(string logName)
        {
            foreach (var item in AllLogs)
            {
                if (item.logname == logName)
                {
                    return false;
                }
            }
            
            return true;
        }


        //fillemptyLogElements
        private Log FillLog(Log myNewLog)
        {
            if(myNewLog.travelBy==null || myNewLog.travelBy == "")
            {
                myNewLog.travelBy = "not specified";
            }
            if (myNewLog.averageSpeed == null || myNewLog.averageSpeed == "")
            {
                myNewLog.averageSpeed = "not specified";
            }
            if (myNewLog.recommandRestaurant == null || myNewLog.recommandRestaurant == "")
            {
                myNewLog.recommandRestaurant = "not specified";
            }
            if (myNewLog.recommandHotel == null || myNewLog.recommandHotel == "")
            {
                myNewLog.recommandHotel = "not specified";
            }
            if (myNewLog.sightWorthSeeing == null || myNewLog.sightWorthSeeing == "")
            {
                myNewLog.sightWorthSeeing = "not specified";
            }
            return myNewLog;

        }

        //--------------------------------------------------------------


        //search Logs----------------------------------------------------------

        public IEnumerable<Log> SearchLogs(string searchElement, string searchOption)
        {
            List<Log> foundLogs = new List<Log>();

            switch (searchOption)
            {
                case "Log Name":
                    foreach(var item in AllLogs)
                    {
                        if (item.logname.ToLower().Contains(searchElement.ToLower()))
                        {
                            foundLogs.Add(item);
                        }
                    }
                    break;
                case "Rating":
                    foreach (var item in AllLogs)
                    {
                        if(item.rating.ToString()== searchElement)
                        {
                            foundLogs.Add(item);
                        }
                    }
                    break;
                case "Distance":
                    foreach (var item in AllLogs)
                    {
                        if(SearchWithNumber(searchElement, item.distance))
                        {
                            foundLogs.Add(item);
                        }
                     
                    }
                    break;
                case "Total Time":
                    foreach (var item in AllLogs)
                    {
                        if (SearchWithNumber(searchElement, item.totalTime))
                        {
                            foundLogs.Add(item);
                        }
                    }
                    break;
                case "Travel By":
                    foreach(var item in AllLogs)
                    if (item.travelBy.ToLower().Contains(searchElement.ToLower()))
                    {
                        foundLogs.Add(item);
                    }
                    break;
                case "Average Speed":
                    foreach(var item in AllLogs)
                    {
                        if (SearchWithNumber(searchElement, item.averageSpeed))
                        {
                            foundLogs.Add(item);
                        }
                    }
                    break;


            };
            return foundLogs;


        }

        //helpfunction to for search to number check
        private bool SearchWithNumber(string number, string element)
        {

            try
            {
                
                float myNumber = float.Parse(number);
                float myElement = float.Parse(element);

                if(myNumber >= myElement)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }


        }


        //----------------------------------------------------------------------


        //delete Log------------------------------------------------------------
        public bool DeleteCurrentLog(Log currentLog)
        {
            if (!myDatabaseLogOrders.DeleteOneLog(currentLog.tourname, currentLog.logname))
            {
                return false;
            }
            if (!myFileHandler.DeleteFile(currentLog.reportfile))
            {
                return false;
            }
            else
            {
                OnUpdateLogList();
                return true;
            }
            
        }

        //Copy Log-------------------------------------------------------------

        public bool CopyCurrentLog(string currentTourName, Log currentLog)
        {
            //add copy to logname and filename
            currentLog.logname = currentLog.logname + "copy";
            
            string copyfilePath = currentLog.reportfile.Replace(".txt", "copy.txt");
            //create copy of file with copy name
            if(!myFileHandler.CopyFile(currentLog.reportfile, copyfilePath))
            {
                return false;
            }
            currentLog.reportfile = copyfilePath;
            if (myDatabaseLogOrders.CreateLog(currentLog))
            {
                OnUpdateLogList();
                return true;
            }
            else
            {
                myFileHandler.DeleteFile(copyfilePath);
                return false;
            }
            
        }

        //---------------------------------------------------------------------


        //----------------------------------------------------------------------
        public string GetLogReport(string reportfile)
        {
            return myFileHandler.GetFileText(reportfile);
        }

        public void SetCurrentLog(Log myCurrentLog)
        {
            if (currentLog == null)
            {
                currentLog = new Log();
            }
            currentLog = myCurrentLog;


        }
        public Log GetCurrentLog()
        {
            return currentLog;
        }


        //modify Log
        public string ModifyLog(string report,string oldReportFile, string logname, Log myModifiedLog)
        {            
            //check if modifiedLogNamen already exists 
            if(logname!= myModifiedLog.logname)
            {
                foreach(var item in AllLogs)
                {
                    if(item.logname== myModifiedLog.logname)
                    {
                        return "This Log Name already exists. Please use another one";
                    }
                }
            }
            //update Log in Database
            if (!myDatabaseLogOrders.UpdateLog(logname, myModifiedLog))
            {
                return "false";
            }
            //update txtfile
            if (!myFileHandler.DeleteFile(oldReportFile)) 
            {
                return "false";
            }
            

            if(!myFileHandler.ChangeFile(myModifiedLog.reportfile, report))
            {
                return "false";
            }

            OnUpdateLogList();
            return "true";
            
        }



    }


}
