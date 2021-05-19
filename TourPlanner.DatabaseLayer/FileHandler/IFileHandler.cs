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
        bool ChangeDescription(string filename, string newDescription);
        bool DeleteImage(string imagefile);
        bool DeleteDescription(string description);

        bool CopyFile(string filename, string newfilename);

        string getDescription(string descriptionFile);
        string SaveReport(string report, string logname);
    }
}
