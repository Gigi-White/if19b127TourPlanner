using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    class CreateTourViewModel : ViewModelBase
    {


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


        private string tourName = "";

        public string TourName
        {
            get
            {
                return tourName;
            }
            set
            {
                if (tourName != value)
                {
                    tourName = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(TourName));
                }
            }

        }


        private string startCity="";
        
        public string StartCity
        {
            get
            {
                return startCity;
            }
            set
            {
                if (startCity != value)
                {
                    startCity = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(StartCity));
                }
            }

        }

        private string startCountry="";

        public string StartCountry
        {
            get
            {
                return startCountry;
            }
            set
            {
                if (startCountry != value)
                {
                    startCountry = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(StartCountry));
                }
            }
        }

        private string endCity="";

        public string EndCity
        {
            get
            {
                return endCity;
            }
            set
            {
                if (endCity != value)
                {
                    endCity = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(EndCity));
                }
            }
        }

        private string endCountry="";

        public string EndCountry
        {
            get
            {
                return endCountry;
            }
            set
            {
                if (endCountry != value)
                {
                    endCountry = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(EndCountry));
                }
            }
        }

        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(Description));
                }
            }
        }



        private ICommand createNewTourCommand;

        public ICommand CreateNewTourCommand => createNewTourCommand ??= new RelayCommand(CreateNewTour);
       
        private void CreateNewTour(object commandParameter)
        {
            if(tourName == "" || startCity =="" || startCountry == "" || endCity == "" || endCountry == "")
            {
                ErrorMessage = "Please fill out all fields!!!";
            }
            
            else
            {
                TourSearch newTour = new TourSearch
                {
                    newTourName = tourName,
                    fromCity = startCity,
                    fromCountry = startCountry,
                    toCity = endCity,
                    toCountry = endCountry
                };
                if (!TourItemFactory.GetMainViewInstance().CheckSearchOption(newTour))
                {
                    ErrorMessage = "Pleas only use Letters numbers and spaces";
                }
               
                else if (TourItemFactory.GetMainViewInstance().CreateTours(newTour))
                {
                    SuccessMessage = "Tour was successfully created";
                }
                else 
                {
                    ErrorMessage = "A error emerged";
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
