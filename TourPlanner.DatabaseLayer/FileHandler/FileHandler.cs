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
        private string logreportfolder;


        //constructor-----------------------------------------------------------------------------------------
        public FileHandler()
        {
            folderpath = ConfigurationManager.AppSettings["FolderDirectory"].ToString();
            urlImageDownload = ConfigurationManager.AppSettings["UrlImageDownload"].ToString();
            descriptionfolder = folderpath + "\\descriptionFolder";
            System.IO.Directory.CreateDirectory(descriptionfolder);
            imagefolder = folderpath + "\\imageFolder";
            System.IO.Directory.CreateDirectory(imagefolder);
            logreportfolder = folderpath + "\\logreportFolder";
            System.IO.Directory.CreateDirectory(logreportfolder);

        }
        //------------------------------------------------------------------------------------------------------


        //methodes for the tour--------------------------------------------------------------------------------
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


        public bool DeleteImage(string imageFile)
        {
            File.Delete(imageFile);
            return (!File.Exists(imageFile) ? true : false);

        }

        public string SaveDescription(string description, string tourname)
        {
            string descritionPath = descriptionfolder +"\\"+ tourname + ".txt";
            StreamWriter newTextfile = File.CreateText(descritionPath);
            newTextfile.WriteLine(description);
            newTextfile.Close();
            log.Debug("new tourfile of " + tourname + " was created");
            return descritionPath;
        }



        //-----------------------------------------------------------------------------------------------------------

        //Logfile metodes--------------------------------------------------------------------------------------------

        public string SaveReport(string report,string tourname, string logname)
        {
            
            string logReportPath = logreportfolder + "\\"+ tourname + logname + ".txt";
            StreamWriter newTextfile = File.CreateText(logReportPath);
            newTextfile.WriteLine(report);
            newTextfile.Close();
            log.Debug("new logfile of " + logname + " was created");
            return logReportPath;
            
        }


        //-----------------------------------------------------------------------------------------------------------

        //generally metodes------------------------------------------------------

        public bool ChangeFile(string filename, string newDescription)
        {
            System.IO.File.WriteAllText(filename, newDescription);
            return true;
        }

        public bool DeleteFile(string descriptionFile)
        {
            File.Delete(descriptionFile);
            return (!File.Exists(descriptionFile) ? true : false);
        }

        public string GetFileText(string descriptionFile)
        {
            string text = File.ReadAllText(descriptionFile);
            return text;
        }

        public bool ChangeFilename(string filename)
        {
            return true;
        }

        public bool CopyFile(string filename, string newfilename)
        {

            File.Copy(filename, newfilename);
            return File.Exists(newfilename);
        }


        //---------------------------------------------------------------------------
    }
}
