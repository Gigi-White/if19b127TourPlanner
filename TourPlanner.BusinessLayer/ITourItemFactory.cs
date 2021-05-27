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
        void SetCurrentTour(Tour viewModelCurrentTour);
        string GetCurrentTourname();
        string GetCurrentTourDescription();

        IEnumerable<Tour> SearchTours(string word, string searchOption);

        bool CreateTours(TourSearch newTourData);

        bool CheckNewTourData(TourSearch info);
        bool DeleteCurrentTour(Tour currentTour);
        bool CopyCurrentTour(Tour currentTour);
        bool CheckText(string text);
        IEnumerable<RawRouteInfo> GetRouteInfo(string tourname);
        bool ModifyTour(string currentTourName, string changedTourName, string currentTourDescription);
        bool CreateReport(Tour currentTour);
        bool ExportTour(Tour currentTour);
        string ImportTour(string jsonFile);
    }
}
