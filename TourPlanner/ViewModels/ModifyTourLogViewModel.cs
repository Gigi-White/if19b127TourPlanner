using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    class ModifyTourLogViewModel : ViewModelBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ILogItemFactory LogWorker;
        Log currentLog;

        private static Regex whitelist;
        private static Regex numberwhitelist;

        public ObservableCollection<string> TravelByList { get; set; }
        public ObservableCollection<int> RatingList { get; set; }

        //successMessage and Error Message Getter and Setter----------------------------------------
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
        //-------------------------------------------------------------------------------------


        //Current Data Getter and Setter-------------------------------------------------------

        private string currentLogName;
        public string CurrentLogName
        {
            get
            {
                return currentLogName;
            }
            set
            {

                if (currentLogName != value)
                {
                    currentLogName = value;
                    
                    RaisePropertyChangedEvent(nameof(CurrentLogName));
                }

            }
        }
        private string currentDistance;
        public string CurrentDistance
        {
            get
            {
                return currentDistance;
            }
            set
            {
                if (currentDistance != value )
                {
                    currentDistance = value;
                    RaisePropertyChangedEvent(nameof(CurrentDistance));
                }
            }
        }

        private string currentTotalTime;
        public string CurrentTotalTime
        {
            get
            {
                return currentTotalTime;
            }
            set
            {
                if (currentTotalTime != value )
                {
                    currentTotalTime = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(CurrentTotalTime));
                }
            }
        }

        private string currentTravelBy;

        public string CurrentTravelBy
        {
            get
            {
                return currentTravelBy;
            }
            set
            {
                if (currentTravelBy != value)
                {
                    currentTravelBy = value;
                    RaisePropertyChangedEvent(nameof(CurrentTravelBy));
                }
            }
        }

        private string currentAverageSpeed;
        public string CurrentAverageSpeed
        {
            get
            {
                return currentAverageSpeed;
            }
            set
            {
                if (currentAverageSpeed != value )
                {
                    currentAverageSpeed = value;
                    RaisePropertyChangedEvent(nameof(CurrentAverageSpeed));
                }
            }
        }



        private string currentRecRestaurant;
        public string CurrentRecRestaurant
        {
            get
            {
                return currentRecRestaurant;
            }
            set
            {
                if (currentRecRestaurant != value )
                {
                    currentRecRestaurant = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(CurrentRecRestaurant));
                }
            }
        }

        private string currentRecHotel;
        public string CurrentRecHotel
        {
            get
            {
                return currentRecHotel;
            }
            set
            {
                if (currentRecHotel != value)
                {
                    currentRecHotel = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(CurrentRecHotel));
                }
            }
        }


        private string currentSightWorthSeeing;
        public string CurrentSightWorthSeeing
        {
            get
            {
                return currentSightWorthSeeing;
            }
            set
            {
                if (currentSightWorthSeeing != value)
                {
                    currentSightWorthSeeing = value;
                    RaisePropertyChangedEvent(nameof(CurrentSightWorthSeeing));
                }
            }
        }

        private int currentRating;
        public int CurrentRating
        {
            get
            {
                return currentRating;
            }
            set
            {
                if (currentRating != value)
                {
                    currentRating = value;
                    RaisePropertyChangedEvent(nameof(CurrentRating));
                }
            }
        }

        private string currentReport;
        public string CurrentReport
        {
            get
            {
                return currentReport;
            }
            set
            {
                if (currentReport != value)
                {
                    currentReport = value;
                    RaisePropertyChangedEvent(nameof(CurrentReport));
                }
            }
        }

        //-------------------------------------------------------------------------------------

        //Changed Data Getter and Setter-------------------------------------------------------

        private string changedLogName;
        public string ChangedLogName
        {
            get
            {
                return changedLogName;
            }
            set
            {

                if (changedLogName != value && CheckText(20, whitelist, value, "New Log Name"))
                {
                    changedLogName = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(ChangedLogName));
                }

            }
        }
        private string changedDistance;
        public string ChangedDistance
        {
            get
            {
                return changedDistance;
            }
            set
            {
                if (changedDistance != value && CheckText(8, numberwhitelist, value, "New Distance"))
                {
                    changedDistance = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(ChangedDistance));
                }
            }
        }

        private string changedTotalTime;
        public string ChangedTotalTime
        {
            get
            {
                return changedTotalTime;
            }
            set
            {
                if (changedTotalTime != value && CheckText(8, numberwhitelist, value, "New Total Time"))
                {
                    changedTotalTime = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(ChangedTotalTime));
                }
            }
        }

        private string changedTravelBy;

        public string ChangedTravelBy
        {
            get
            {
                return changedTravelBy;
            }
            set
            {
                if (changedTravelBy != value)
                {
                    changedTravelBy = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(ChangedTravelBy));
                }
            }
        }

        private string changedAverageSpeed;
        public string ChangedAverageSpeed
        {
            get
            {
                return changedAverageSpeed;
            }
            set
            {
                if (changedAverageSpeed != value && CheckText(8, numberwhitelist, value, "New Average speed"))
                {
                    changedAverageSpeed = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(ChangedAverageSpeed));
                }
            }
        }



        private string changedRecRestaurant;
        public string ChangedRecRestaurant
        {
            get
            {
                return changedRecRestaurant;
            }
            set
            {
                if (changedRecRestaurant != value && CheckText(20, whitelist, value, "New Rec. Restaurant"))
                {
                    changedRecRestaurant = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(ChangedRecRestaurant));
                }
            }
        }

        private string changedRecHotel;
        public string ChangedRecHotel
        {
            get
            {
                return changedRecHotel;
            }
            set
            {
                if (changedRecHotel != value && CheckText(20, whitelist, value, "New Rec. Hotel"))
                {
                    changedRecHotel = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(ChangedRecHotel));
                }
            }
        }


        private string changedSightWorthSeeing;
        public string ChangedSightWorthSeeing
        {
            get
            {
                return changedSightWorthSeeing;
            }
            set
            {
                if (changedSightWorthSeeing != value && CheckText(20, whitelist, value, "New Sight worth seeing"))
                {
                    changedSightWorthSeeing = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(ChangedSightWorthSeeing));
                }
            }
        }

        private int changedRating;
        public int ChangedRating
        {
            get
            {
                return changedRating;
            }
            set
            {
                if (changedRating != value)
                {
                    changedRating = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(ChangedRating));
                }
            }
        }

        private string changedReport;
        public string ChangedReport
        {
            get
            {
                return changedReport;
            }
            set
            {
                if (changedReport != value && CheckText(200, whitelist, value, "Report"))
                {
                    ChangedReport = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(ChangedReport));
                }
            }
        }


        //-------------------------------------------------------------------------------------
     
        public ModifyTourLogViewModel()
        {
            LogWorker = TourItemFactory.GetLogViewInstance();
            FillTravelBy();
            FillRating();
            whitelist = new Regex(ConfigurationManager.AppSettings["Whitelist"].ToString());
            numberwhitelist = new Regex(ConfigurationManager.AppSettings["NumberWhitelist"].ToString());

            currentLog = LogWorker.GetCurrentLog();
            if (LogWorker.GetCurrentLog() == null)
            {
                ErrorMessage = "No Log was selected. Please close this window";
                return;
            }
            CurrentLogName = currentLog.logname;
            CurrentDistance = currentLog.distance;
            CurrentTotalTime = currentLog.totalTime;
            CurrentTravelBy = currentLog.travelBy;
            CurrentAverageSpeed = currentLog.averageSpeed;
            CurrentRecRestaurant = currentLog.recommandRestaurant;
            CurrentRecHotel = currentLog.recommandHotel;
            CurrentSightWorthSeeing = currentLog.sightWorthSeeing;
            CurrentRating = currentLog.rating;
            CurrentReport = LogWorker.GetLogReport(currentLog.reportfile);
                   
        }

        private void FillTravelBy()
        {
            TravelByList = new ObservableCollection<string> { "car", "on foot", "train", "bicycle", "motorcycle", "plain", "other" };
        }

        private void FillRating()
        {
            RatingList = new ObservableCollection<int> { 0, 1, 2, 3, 4, 5 };
        }


        //checks the text, if it is long enouth and has only whitelist characters---
        private void CleanMessages()  //delets Error and success message
        {
            SuccessMessage = null;
            ErrorMessage = null;
        }


        private bool CheckText(int lenght, Regex mywhitelist, string text, string field)
        {
            if (CheckLength(lenght, text, field) && CheckWhiteList(mywhitelist, text, field))
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
            if (text.Length < length)
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
            if (text == null || text == "")
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

    }
}
