using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    class ModifyTourViewModel : ViewModelBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ITourItemFactory TourWorker;

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
                    RaisePropertyChangedEvent(nameof(CurrentTourName));
                }
            }
        }
        

        private string currentTourDescription;
        public string CurrentTourDescription
        {
            get
            {
                return currentTourDescription;
            }
            set
            {
                if (currentTourDescription != value)
                {
                    currentTourDescription = value;
                    RaisePropertyChangedEvent(nameof(CurrentTourDescription));
                }
            }
        }

        

        private string changedTourName = "";

        public string ChangedTourName
        {
            get
            {
                return changedTourName;
            }
            set
            {
                if (changedTourName != value)
                {
                    changedTourName = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(ChangedTourName));
                }
            }

        }

        private string changedTourDescription = "";

        public string ChangedTourDescription
        {
            get
            {
                return changedTourDescription;
            }
            set
            {
                if (changedTourDescription != value)
                {
                    changedTourDescription = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(ChangedTourDescription));
                }
            }

        }


        //Constructor----------------------------------------------------------

        public ModifyTourViewModel()
        {
            TourWorker = TourItemFactory.GetMainViewInstance();

            currentTourName = TourWorker.GetCurrentTourname();
            currentTourDescription = TourWorker.GetCurrentTourDescription();
            if(currentTourName==null || currentTourDescription == null)
            {
                ErrorMessage = "No Trip was chosen. Please close this window";
            }

        }




        //Commands-------------------------------------------------------------------
        private ICommand modifyTourCommand;
        public ICommand ModifyTourCommand => modifyTourCommand ??= new RelayCommand(ModifyTour);
        
        private void ModifyTour(object commandParameter)
        {
            if (currentTourName==null|| currentTourDescription== null)
            {
                ErrorMessage = "No Trip was chosen. Please close this window";
            }
            else if (changedTourName==null&& changedTourDescription == null)
            {
                ErrorMessage = "Please fill out one field!!";
            }
            else if (!TourWorker.CheckText(currentTourName)&& !TourWorker.CheckText(currentTourDescription))
            {
                ErrorMessage = "please only use letters, numbers or spaces";
            }

            else
            {
               if(changedTourName == null)
                {
                    TourWorker.ModifyTour(currentTourName, currentTourName, changedTourDescription);
                }
               else if (changedTourDescription == null)
                {
                    TourWorker.ModifyTour(currentTourName, changedTourName, currentTourDescription);
                }
                else
                {
                    if (!TourWorker.ModifyTour(currentTourName, changedTourName, changedTourDescription))
                    {
                        ErrorMessage = "There was a problem with the system";
                    }
                    else
                    {
                        SuccessMessage = "Trip was successfully changed";

                    }
                }
            }

           
        }


        private void CleanMessages()
        {
            SuccessMessage = null;
            ErrorMessage = null;
        }

    }
}
