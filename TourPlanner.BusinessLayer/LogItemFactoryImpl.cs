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

        private string currentTourName;
       
        //constructor----------------------------------------------------------
        public LogItemFactoryImpl()
        {
            myDatabaseLogOrders = DataConnectionFactory.GetDatabaseLogInstance();
            myFileHandler = DataConnectionFactory.GetFileHandlerInstance();
        }
        //----------------------------------------------------------------------

        //get logs of current Tour
        public IEnumerable<Log> GetLogs(string currentTourName)
        {
            AllLogs = myDatabaseLogOrders.GetLogsofTour(currentTourName);
            return AllLogs;
        }

        //get end set current Tour name-----------------------------------
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

            //fill not important parts of Log if they have no value
            myNewLog = FillLog(myNewLog);

            //create new report file and save the report in file;
            string reportPath = myFileHandler.SaveReport(report, myNewLog.logname);
            myNewLog.reportfile = reportPath;
            //save the new log in the database
            if (myDatabaseLogOrders.CreateLog(myNewLog))
            {
                return "true";
            }
            else
            {
                return "There was a error in during the procedure";
            }



            return message;
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

        public string GetLogReport(string reportfile)
        {
            return myFileHandler.getDescription(reportfile);
        }
    }
}
