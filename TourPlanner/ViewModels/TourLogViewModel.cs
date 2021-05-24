using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    class TourLogViewModel:ViewModelBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ITourItemFactory TourWorker;
        ILogItemFactory LogWorker;

        public ObservableCollection<Log> TourLogs { get; set; }
        public ObservableCollection<string> SearchOptionList { get; set; }

        private string currentTourName;

        public string CurrentTourName
        {
            get
            {
                return currentTourName;
            }
            set
            {
                if (currentTourName != value)
                {
                    currentTourName = value;
                    LogWorker.SetCurrentTourName(currentTourName);
                    RaisePropertyChangedEvent(nameof(CurrentTourName));
                }
            }
        }


        //--------------------------------------------------------------------------------------

        //Getter and setter for the view Information---------------------------------------------
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


        private string searchElement;
        public string SearchElement
        {
            get
            {
                return searchElement;
            }
            set
            {
                if (searchElement != value)
                {
                    searchElement = value;
                    CleanMessages();
                    SearchLogs();
                    RaisePropertyChangedEvent(nameof(SearchElement));
                }
            }
        }

        private string searchOption;

        public string SearchOption
        {
            get
            {
                return searchOption;
            }
            set
            {
                if (searchOption != value && value != null)
                {

                    searchOption = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(SearchOption));
                }
            }
        }


        private Log currentLog;
        public Log CurrentLog
        {
            get
            {
                return currentLog;
            }
            set
            {
                if (currentLog != value && value != null)
                {
                    currentLog = value;
                    CurrentLogReport = LogWorker.GetLogReport(currentLog.reportfile);
                    LogWorker.SetCurrentLog(currentLog);
                    CleanMessages();                   
                    RaisePropertyChangedEvent(nameof(CurrentLog));
                }
            }
        }

        private string currentLogReport;

        public string CurrentLogReport
        {
            get
            {
                return currentLogReport;
            }
            set
            {
                if (currentLogReport != value && value != null)
                {
                    currentLogReport = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(CurrentLogReport));
                }
            }
        }




        //------------------------------------------------------------------------------------


        //constructor-------------------------------------------------------------------------


        public TourLogViewModel()
        {
            TourWorker = TourItemFactory.GetMainViewInstance();
            LogWorker = TourItemFactory.GetLogViewInstance();
            TourLogs = new ObservableCollection<Log>();
            SearchOptionList = new ObservableCollection<string>();
            currentTourName = TourWorker.GetCurrentTourname();
            LogWorker.setUpdateLogsEventhandler(UpdateLogList);

            if (currentTourName == null || currentTourName == "")
            {
                ErrorMessage = "No Tour was chosen. Please close this window";
            }
            else
            {
                LogWorker.SetCurrentTourName(currentTourName);
                FillCompleteLogList();
                FillSearchOptionList();
            }
            
          

        }
        public void UpdateLogList(object source, EventArgs e)
        {
            FillCompleteLogList();
        }


        private void FillCompleteLogList()
        {

            FillLogList(LogWorker.GetLogs(currentTourName));
        }

        private void FillLogList(IEnumerable<Log> myLogList)
        {
            if (myLogList != null)
            {
                TourLogs.Clear();
                foreach (var item in myLogList)
                    TourLogs.Add(item);
            }
        }


        private void FillSearchOptionList()
        {
            SearchOptionList.Add("Log Name");
            SearchOptionList.Add("Rating");
            SearchOptionList.Add("Distance");
            SearchOptionList.Add("Total Time");
            SearchOptionList.Add("Travel By");
            SearchOptionList.Add("Average Speed");
    
        }


        //-----------------------------------------------------------------------------------

        //commands---------------------------------------------------------------------------
        
        //delete Command--------------
        private ICommand deleteLogCommand;

        public ICommand DeleteCommand => deleteLogCommand ??= new RelayCommand(DeleteLog);

        private void DeleteLog(object commandParameter)
        {

            if (currentLog != null)
            {            

                if (LogWorker.DeleteCurrentLog(currentLog))
                {
                    CurrentLog = null;
                    SuccessMessage = "Log was successfully deleted";
                }
                else
                {
                    ErrorMessage = "The deletion could not be completed";
                }

            }
            else
            {
                ErrorMessage = "Please select a Log first";
            }
        }
        //--------------------

        //copy command----------------
        private ICommand copyLogCommand;

        public ICommand CopyCommand => copyLogCommand ??= new RelayCommand(CopyLog);


        private void CopyLog(object commandParameter)
        {
            if (currentLog != null)
            {

                if (LogWorker.CopyCurrentLog(currentTourName, currentLog))
                {
                    CurrentLog = null;

                    SuccessMessage = "Log was successfully copied";
                }
                else
                {
                    ErrorMessage = "The copy could not be completed";
                }

            }
            else
            {
                ErrorMessage = "Please select a Log first";
            }
        }

        //----------------------------------------------------------------------------------
        private void CleanMessages()
        {
            ErrorMessage=null;
            SuccessMessage = null;
        }

        private void SearchLogs()
        {
            if (searchElement != null && searchElement != "" && searchOption != null && searchOption != "")
            {

                FillLogList(LogWorker.SearchLogs(searchElement, searchOption));
            }
            else
            {
                FillCompleteLogList();
            }
        }
    }
}
