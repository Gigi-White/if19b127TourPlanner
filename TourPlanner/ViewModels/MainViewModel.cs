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

        public MainViewModel()
        {
            Tours = new ObservableCollection<Tour>();
            FillCompleteTourList();

        }

        private void FillCompleteTourList()
        {
            myFactory = TourItemFactory.GetInstance();
            FillTourList(myFactory.GetTours());
        }

        private void FillTourList(IEnumerable<Tour> myTourList)
        {
            Tours.Clear();
            foreach (var item in myTourList)
                Tours.Add(item);
        }

        private void SearchTours()
        {
            if(searchTour!=null && searchTour != "")
            {

                FillTourList(myFactory.SearchTours(searchTour, TourData.Name));
            }
            else
            {
                FillCompleteTourList();
            }
        }

    }

}
