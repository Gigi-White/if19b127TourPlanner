using Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        ITourItemFactory myFactory;


        public ObservableCollection <Tour> Tours { get; set; }
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
                if(currentTour != value && value != null)
                {
                    currentTour = value;
                    RaisePropertyChangedEvent(nameof(CurrentTour));
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
            SearchOptionList = new ObservableCollection<string>();
            Tours = new ObservableCollection<Tour>();
            FillCompleteTourList();
            FillSearchOtionList();

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
            myFactory = TourItemFactory.GetInstance();
            FillTourList(myFactory.GetTours());
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
            if(searchTour!=null && searchTour != "" && searchOption != null && searchOption != "")
            {

                FillTourList(myFactory.SearchTours(searchTour, searchOption));
            }
            else
            {
                FillCompleteTourList();
            }
        }

    }

}
