using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    internal class FileHandler : IFileHandler
    {

        private string folderpath;
        private string urlImageDownload;

        public FileHandler()
        {
            folderpath = ConfigurationManager.AppSettings["ImageFolderDirectory"].ToString();
            urlImageDownload = ConfigurationManager.AppSettings["UrlImageDownload"].ToString();
        }


        public string DownloadSaveImage(MainMapSearchData searchData, string tourname)
        {

            System.IO.Directory.CreateDirectory(folderpath);
            string imagePath = folderpath + "\\" + tourname + ".png";


            string fullUrl = urlImageDownload +
                             "session=" +
                             searchData.sessionId +
                             "&boundingBox=" +
                             searchData.boundingBox[0].ToString() + "," +
                             searchData.boundingBox[1].ToString() + "," +
                             searchData.boundingBox[2].ToString() + "," +
                             searchData.boundingBox[3].ToString();

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(fullUrl, imagePath);
            }


            return imagePath;


        }
        public bool DeleteImage()
        {
            return true;
        }

    }
}
