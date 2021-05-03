﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using System;
using System.Windows.Input;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace TourPlanner.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ITourItemFactory TourWorker;


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
                    RaisePropertyChangedEvent(nameof(CurrentTourImage));
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
                TourWorker.DeleteCurrentTour(currentTour.Name);
            }
        }





    }

}
