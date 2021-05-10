using System.Collections.Generic;
using System.Collections.ObjectModel;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace TourPlanner.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ITourItemFactory TourWorker;


        private string tourMessage;
        public string TourMessage
        {
            get
            {
                return tourMessage;
            }
            set
            {
                if (tourMessage != value)
                {
                    tourMessage = value;
                    RaisePropertyChangedEvent(nameof(TourMessage));
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


        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<string> SearchOptionList { get; set; }
        //--------------------------------------------------------

        private string searchTour;
        public string SearchTour
        {
            get
            {
                return searchTour;
            }
            set
            {
                if (searchTour != value)
                {
                    searchTour = value;
                    SearchTours();
                    RaisePropertyChangedEvent(nameof(SearchTour));
                }
            }
        }

        //--------------------------------------------------------
        private Tour currentTour;
        public Tour CurrentTour
        {
            get
            {
                return currentTour;
            }
            set
            {
                if (currentTour != value && value != null)
                {
                    currentTour = value;
                    CleanMessages();
                    CurrentTourImage = currentTour.Imagefile;
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                }
            }
        }

        private string currentTourImage;

        public string CurrentTourImage
        {
            get
            {
                return currentTourImage;
            }

            set
            {
                if (currentTourImage != value)
                {
                    currentTourImage = value;
                    ShowImage = CreateBitmapImage(currentTourImage);
                    //RaisePropertyChangedEvent(nameof(CurrentTourImage));
                }
            }
        }

        private BitmapImage showImage;
        public BitmapImage ShowImage
        {
            get
            {
                return showImage;
            }
            set
            {
                if (showImage != value)
                {
                    showImage = value;
                    RaisePropertyChangedEvent(nameof(ShowImage));
                }
            }

        }

        private BitmapImage CreateBitmapImage(string imagePath)
        {
            if (imagePath == null)
            {
                return null;
            }
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(imagePath);
            image.EndInit();
            return image;
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



        //--------------------------------------------------------
        public MainViewModel()
        {

            log.Info("Maintenance: Main View Model has startet");
            TourWorker = TourItemFactory.GetMainViewInstance();
            SearchOptionList = new ObservableCollection<string>();
            Tours = new ObservableCollection<Tour>();
            FillCompleteTourList();
            FillSearchOtionList();
            TourWorker.setUpdateToursEventhandler(UpdateTourList);


        }
        public void UpdateTourList(object source, EventArgs e)
        {
            FillCompleteTourList();
        }

        private void FillSearchOtionList()
        {
            SearchOptionList.Add("Name");
            SearchOptionList.Add("Start");
            SearchOptionList.Add("End");
            SearchOptionList.Add("Distance");

        }

        private void FillCompleteTourList()
        {

            FillTourList(TourWorker.GetTours());
        }

        private void FillTourList(IEnumerable<Tour> myTourList)
        {
            if (myTourList != null)
            {
                Tours.Clear();
                foreach (var item in myTourList)
                    Tours.Add(item);
            }
        }

        private void SearchTours()
        {
            if (searchTour != null && searchTour != "" && searchOption != null && searchOption != "")
            {

                FillTourList(TourWorker.SearchTours(searchTour, searchOption));
            }
            else
            {
                FillCompleteTourList();
            }
        }

        private ICommand deleteTourCommand;

        public ICommand DeleteTourCommand => deleteTourCommand ??= new RelayCommand(DeleteTour);

        private void DeleteTour(object commandParameter)
        {

            if (currentTour != null)
            {
                CurrentTourImage = null;

                if (TourWorker.DeleteCurrentTour(currentTour))
                {
                    CurrentTour = null;
                    TourMessage ="Tour was successfully deleted";
                }
                else
                {
                    ErrorMessage = "The deletion could not be completed";
                }
                
            }
            else
            {
                ErrorMessage = "Please select a Tour first";
            }
        }


        private void CleanMessages()
        {
            TourMessage = null;
            ErrorMessage = null;
        }


    }

}
