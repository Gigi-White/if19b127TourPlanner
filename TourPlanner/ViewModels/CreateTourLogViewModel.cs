using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    class CreateTourLogViewModel: ViewModelBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private static Regex whitelist;
        private static Regex numberwhitelist;

        ILogItemFactory LogWorker;

        private static string tourname;

        public ObservableCollection<string> TravelByList { get; set; }
        public ObservableCollection<int> RatingList { get; set; }


        //Getter and setter for the view Information---------------------------------------------

        private string successMessage;
        public string SuccessMessage
        {
            get
            {
                return successMessage;
            }
            set
            {
                if (successMessage != value)
                {
                    successMessage = value;
                    RaisePropertyChangedEvent(nameof(SuccessMessage));
                }
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    RaisePropertyChangedEvent(nameof(ErrorMessage));
                }
            }
        }

        private string logName;
        public string LogName
        {
            get
            {
                return logName;
            }
            set
            {
               
                if (logName != value && CheckText(20, whitelist,value, "Log Name"))
                {
                    logName = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(LogName));
                }

            }
        }
        private string distance;
        public string Distance
        {
            get
            {
                return distance;
            }
            set
            {
                if (distance != value && CheckText(8, numberwhitelist, value, "Distance"))
                {
                    distance = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(Distance));
                }
            }
        }

        private string totalTime;
        public string TotalTime
        {
            get
            {
                return totalTime;
            }
            set
            {
                if (totalTime != value && CheckText(8, numberwhitelist, value, "Total Time"))
                {
                    totalTime = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(TotalTime));
                }
            }
        }

        private string travelBy;

        public string TravelBy
        {
            get
            {
                return travelBy;
            }
            set
            {
                if (travelBy != value)
                {
                    travelBy = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(TravelBy));
                }
            }
        }

        private string averageSpeed;
        public string AverageSpeed
        {
            get
            {
                return averageSpeed;
            }
            set
            {
                if (averageSpeed != value && CheckText(8, numberwhitelist, value, "Average speed"))
                {
                    averageSpeed = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(AverageSpeed));
                }
            }
        }

        

        private string recommandRestaurant;
        public string RecommandRestaurant
        {
            get
            {
                return recommandRestaurant;
            }
            set
            {
                if (recommandRestaurant != value && CheckText(20, whitelist, value, "Recommand Restaurant"))
                {
                    recommandRestaurant = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(RecommandRestaurant));
                }
            }
        }

        private string recommandHotel;
        public string RecommandHotel
        {
            get
            {
                return recommandHotel;
            }
            set
            {
                if (recommandHotel != value && CheckText(20, whitelist, value, "Recommand Hotel"))
                {
                    recommandHotel = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(RecommandHotel));
                }
            }
        }


        private string sightWorthSeeing;
        public string SightWorthSeeing
        {
            get
            {
                return sightWorthSeeing;
            }
            set
            {
                if (sightWorthSeeing != value && CheckText(20, whitelist, value, "Sight worth seeing"))
                {
                    sightWorthSeeing = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(SightWorthSeeing));
                }
            }
        }

        private int rating;
        public int Rating
        {
            get
            {
                return rating;
            }
            set
            {
                if (rating != value)
                {
                    rating = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(Rating));
                }
            }
        }

        private string report;
        public string Report
        {
            get
            {
                return report;
            }
            set
            {
                if (report != value && CheckText(200, whitelist, value, "Report"))
                {
                    report = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(Report));
                }
            }
        }

        //----------------------------------------------------------------------------------------



        //constructor-----------------------------------------------------------------------------
        public CreateTourLogViewModel()
        {
            LogWorker = TourItemFactory.GetLogViewInstance();
            FillTravelBy();
            FillRating();
            whitelist = new Regex(ConfigurationManager.AppSettings["Whitelist"].ToString());
            numberwhitelist = new Regex(ConfigurationManager.AppSettings["NumberWhitelist"].ToString());
        }

        private void FillTravelBy()
        {
            TravelByList = new ObservableCollection<string> { "car", "on foot", "train", "bicycle", "motorcycle","plain","other" };
        }

        private void FillRating()
        {
            RatingList = new ObservableCollection<int> {0, 1, 2, 3, 4, 5 };
        }
        //----------------------------------------------------------------------------------------




        //commands--------------------------------------------------------------------------------

        private ICommand createLogCommand;

        public ICommand CreateLogCommand => createLogCommand ??= new RelayCommand(CreateTourLog);

        private void CreateTourLog(object commandParameter)
        {
            //create and fill new log
            Log myNewLog = FillNewLog();
            //send Log Info to business layer
            string message = LogWorker.CreateNewTourLog(myNewLog, report);
            //check if everything ent smooth
            
            if (message == "true")
            {
                SuccessMessage = "New Tour Log was created";
            }
            else
            {
                ErrorMessage = message;
            }
        }
        //----------------------------------------------------------------------------------------



        //help methods----------------------------------------------------------------------------

        private Log FillNewLog()
        {
            Log myNewLog = new Log
            {
                tourname = LogWorker.GetCurrentTourName(),
                logname = this.logName,
                date = System.DateTime.Now.ToString(@"dd\/MM\/yyyy h\:mm tt"),
                distance = this.distance,
                totalTime = this.totalTime,
                rating = this.rating,
                travelBy = this.travelBy,
                averageSpeed = this.averageSpeed,
                recommandRestaurant = this.recommandRestaurant,
                recommandHotel = this.recommandHotel,
                sightWorthSeeing = this.sightWorthSeeing
            };

            return myNewLog;
        }



        //checks the text, if it is long enouth and has only whitelist characters---
        private void CleanMessages()  //delets Error and success message
        {
            SuccessMessage = null;
            ErrorMessage = null;
        }


        private bool CheckText(int lenght, Regex mywhitelist, string text, string field)
        {
            if(CheckLength(lenght, text, field) && CheckWhiteList(mywhitelist, text, field))
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        private bool CheckLength(int length, string text, string field)
        {
            if (text == null)
            {
                return true;
            }
            if(text.Length< length)
            {
                return true;
            }
            else
            {
                ErrorMessage = "Its not allowed that " + field + " has more then " + length + " characters";
                return false;
            }
        }

        private bool CheckWhiteList(Regex myWhiteList, string text, string field)
        {
            if(text==null || text == "")
            {
                return true;
            }

            if (myWhiteList.IsMatch(text))
            {
                return true;
            }

            if (myWhiteList == whitelist)
            {
                ErrorMessage = "For " + field + " only use letters and numbers";
                return false;
            }
            else
            {
                ErrorMessage = "For " + field + " only use numbers";
                return false;
            }
            
        }
        //---------


        //----------------------------------------------------------------------------------------
    }
}
