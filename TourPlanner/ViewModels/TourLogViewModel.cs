using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
      

        //--------------------------------------------------------------------------------------

        //Getter and setter for the view Information---------------------------------------------
        private string searchLog;
        public string SearchLog
        {
            get
            {
                return searchLog;
            }
            set
            {
                if (searchLog != value)
                {
                    searchLog = value;
                    SearchLogs();
                    RaisePropertyChangedEvent(nameof(SearchLog));
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

            currentTourName = TourWorker.GetCurrentTourname();
            TourLogs = new ObservableCollection<Log>();
            FillCompleteLogList();
            FillSearchOtionList();

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


        private void FillSearchOtionList()
        {
            SearchOptionList.Add("Name");
            SearchOptionList.Add("Start");
            SearchOptionList.Add("End");
            SearchOptionList.Add("Distance");

        }


        //-----------------------------------------------------------------------------------

        //commands---------------------------------------------------------------------------
        private void CleanMessages()
        {
            throw new NotImplementedException();
        }

        private void SearchLogs()
        {
            throw new NotImplementedException();
        }
    }
}
