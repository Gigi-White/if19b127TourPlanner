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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


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

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(fullUrl, imagePath);
                    webClient.Dispose();
                }
               

                return imagePath;
            }
            catch(Exception ex)
            {
                log.Error("Maintenance: Falied to download the image",ex);
                return "null";

            }

           


        }

        public string SaveDescription(string description, string tourname)
        {
            string descritionPath = descriptionfolder +"\\"+ tourname + ".txt";
            StreamWriter newTextfile = File.CreateText(descritionPath);
            newTextfile.WriteLine(description);
            newTextfile.Close();
            return descritionPath;
        }
        
        public bool ChangeFilename(string filename)
        {
            return true;
        }

        public bool ChangeDescription(string filename,string newDescription)
        {
            return true;
        }

        public bool DeleteImage(string imageFile)
        {
            File.Delete(imageFile);
            return(!File.Exists(imageFile) ?  true :  false);
                        
        }

        public bool DeleteDescription(string descriptionFile)
        {
            File.Delete(descriptionFile);
            return (!File.Exists(descriptionFile) ? true : false);
        }



    }
}
