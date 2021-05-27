using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public interface IFileHandler
    {

        string DownloadSaveImage(MainMapSearchData searchData, string tourname);
        string SaveDescription(string tourname, string description);

        bool ChangeFilename(string filename);
        bool ChangeFile(string txtfilepath, string newDescription);
        bool DeleteImage(string imagefilepath);
        bool DeleteFile(string txtfilepath);

        bool CopyFile(string filepath, string newfilepath);

        string GetFileText(string txtfilepath);
        string SaveReport(string report,string tourname, string logname);
        bool CreateTourReport(Tour currentTour, List<RawRouteInfo> routeList, List<Log> logList);
        bool CreateSummarizeReport(Tour currentTour, List<Log> logList);

        bool ExportTour(JsonTour exportData);
    }
}
