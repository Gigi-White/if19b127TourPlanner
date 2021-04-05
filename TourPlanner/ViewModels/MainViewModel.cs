using System.Collections.Generic;
using System.Collections.ObjectModel;

using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : ViewModelBase
    {


        

        string searchTour;

        public ObservableCollection <Tour> Tours { get; set; }

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
            List<Tour> MyTours = new List<Tour>();

            Tour eins = new Tour()
            {
                Name = "Gigi",
                Start = "Wien",
                End = "Berlin",
                CreationDate = "26.04.2020",
                Distance = 200,
               
            };

            MyTours.Add(eins);


            Tour zwei = new Tour()
            {
                Name = "Mike",
                Start = "Hamburg",
                End = "Berlin",
                CreationDate = "25.04.2020",
                Distance = 100,
                
            };

            MyTours.Add(zwei);

            Tours = new ObservableCollection<Tour>();
            foreach (var item in MyTours)
                Tours.Add(item);

        }       

    }

}
