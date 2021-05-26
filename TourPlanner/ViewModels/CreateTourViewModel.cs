using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    class CreateTourViewModel : ViewModelBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Regex whitelist;

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
                if (tourName != value && CheckText(30, whitelist, value, "Tour Name"))
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
                if (startCity != value && CheckText(30, whitelist, value, "Tour Name"))
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
                if (startCountry != value && CheckText(30, whitelist, value, "Tour Name"))
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
                if (endCity != value && CheckText(30, whitelist, value, "Tour Name"))
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
                if (endCountry != value && CheckText(30, whitelist, value, "Tour Name"))
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
                if (description != value && CheckText(200, whitelist, value, "Tour Name"))
                {
                    description = value;
                    CleanMessages();
                    RaisePropertyChangedEvent(nameof(Description));
                }
            }
        }
       
        public  CreateTourViewModel()
        {
            whitelist = new Regex(ConfigurationManager.AppSettings["Whitelist"].ToString());
        }


        private ICommand createNewTourCommand;

        public ICommand CreateNewTourCommand => createNewTourCommand ??= new RelayCommand(CreateNewTour);
       
        private void CreateNewTour(object commandParameter)
        {

            log.Info("Interaction: Start to create new Tour");

            if(tourName == "" || startCity =="" || startCountry == "" || endCity == "" || endCountry == "" || description=="")
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
                    toCountry = endCountry,
                    tourDescription = description
                };

                if (!TourItemFactory.GetMainViewInstance().CheckNewTourData(newTour))
                {
                    ErrorMessage = "please only use letters, numbers or spaces";
                }
               
                else if (TourItemFactory.GetMainViewInstance().CreateTours(newTour))
                {
                    SuccessMessage = "Tour was successfully created";
                    log.Info("Interaction: Tour was successfully created");
                }
                else 
                {
                    ErrorMessage = "A error emerged";
                }

            }
            
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


        private void CleanMessages()
        {
            SuccessMessage = null;
            ErrorMessage = null;
        }


    }
}
