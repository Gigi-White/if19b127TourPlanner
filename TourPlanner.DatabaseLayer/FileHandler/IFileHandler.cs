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
        bool DeleteImage(string imagefile);
        bool DeleteDescription(string description);

    }
}
