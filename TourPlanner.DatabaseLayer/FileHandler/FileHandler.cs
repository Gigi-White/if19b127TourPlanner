using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    internal class FileHandler : IFileHandler
    {

        private string folderpath;
        private string urlImageDownload;
        private string imagefolder;
        private string descriptionfolder;

        public FileHandler()
        {
            folderpath = ConfigurationManager.AppSettings["FolderDirectory"].ToString();
            urlImageDownload = ConfigurationManager.AppSettings["UrlImageDownload"].ToString();
            descriptionfolder = folderpath + "\\descriptionFolder";
            System.IO.Directory.CreateDirectory(descriptionfolder);
            imagefolder = folderpath + "\\imageFolder";
            System.IO.Directory.CreateDirectory(imagefolder);

        }


        public string DownloadSaveImage(MainMapSearchData searchData, string tourname)
        {

       


            string imagePath = imagefolder + "\\" + tourname + ".png";


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

        public string SaveDescription(string description, string tourname)
        {
            string descritionPath = descriptionfolder +"\\"+ tourname + ".txt";
            StreamWriter newTextfile = File.CreateText(descritionPath);
            newTextfile.WriteLine(description);
            newTextfile.Close();
            return descritionPath;
        }
        
        public bool DeleteImage(string imagefile)
        {
            return true;
        }

        public bool DeleteDescription(string imagefile)
        {
            return true;
        }

    }
}
