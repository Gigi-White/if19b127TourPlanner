using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TourPlanner.BusinessLayer;

namespace TourPlanner.ViewModels
{
    class ImportTourViewModel : ViewModelBase
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

        private string jsonFilePath;
        public string JsonFilePath
        {
            get
            {
                return jsonFilePath;
            }
            set
            {
                if (jsonFilePath != value)
                {
                    CleanMessages();
                    jsonFilePath = value;
                    RaisePropertyChangedEvent(nameof(JsonFilePath));
                }
            }
        }


        public ImportTourViewModel()
        {
            TourWorker = TourItemFactory.GetMainViewInstance();
        }

        private ICommand importTourCommand;

        public ICommand ImportTourCommand => importTourCommand ??= new RelayCommand(ImportTour);

        private void ImportTour(object commandParameter)
        {
            CleanMessages();
            if (jsonFilePath == null || jsonFilePath == "")
            {
                ErrorMessage = "Please write a File Name in the field";
                return;
            }
            
            string message = TourWorker.ImportTour(jsonFilePath);
            if(message != "")
            {
                JsonFilePath = "";
                ErrorMessage = message;              
                return;
            }
            JsonFilePath = "";
            SuccessMessage = "Tour was sucessfully imported";

        }


        private void CleanMessages()
        {
            SuccessMessage = null;
            ErrorMessage = null;
        }

    }
}
