using Enums;
using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;
using VisioForge.MediaFramework.ONVIF;

namespace TourPlanner.BusinessLayer
{
    public delegate void UpdateToursEventHandler(object source, EventArgs args);
    public interface ITourItemFactory
    {

        void setUpdateToursEventhandler(UpdateToursEventHandler newEvent);

        IEnumerable<Tour> GetTours();

        IEnumerable<Tour> SearchTours(string word, string searchOption);

        bool CreateTours(TourSearch newTourData);

    }
}
