using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TourPlanner.BusinessLayer;

namespace TourPlanner.ViewModels
{
    class CreateTourLogViewModel: ViewModelBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ILogItemFactory LogWorker;

        private static string tourname;

        public ObservableCollection<string> TravelBy { get; set; }
        public ObservableCollection<int> Rating { get; set; }


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



        //----------------------------------------------------------------------------------------



        //constructor-----------------------------------------------------------------------------
        public CreateTourLogViewModel()
        {
            LogWorker = TourItemFactory.GetLogViewInstance();
            FillTravelBy();
            FillRating();
            
        }

        private void FillTravelBy()
        {
            TravelBy = new ObservableCollection<string> { "car", "on foot", "train", "bicycle", "motorcycle","plain","other" };
        }

        private void FillRating()
        {
            Rating = new ObservableCollection<int> {0, 1, 2, 3, 4, 5 };
        }
        //----------------------------------------------------------------------------------------




        //commands--------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------



        //help methods----------------------------------------------------------------------------

        private void CleanMessages()  //delets Error and success message
        {
            SuccessMessage = null;
            ErrorMessage = null;
        }

        //----------------------------------------------------------------------------------------
    }
}
