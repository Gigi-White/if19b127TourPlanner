using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public interface IFileHandler
    {

        string DownloadSaveImage(MainMapSearchData searchData, string tourname);
        bool DeleteImage();
    }
}
