using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private string pdfFolder;
        private string jsonFolder;


        //constructor-----------------------------------------------------------------------------------------
        public FileHandler()
        {
            //create all necessary folders for the programme 
            folderpath = ConfigurationManager.AppSettings["FolderDirectory"].ToString();
            urlImageDownload = ConfigurationManager.AppSettings["UrlImageDownload"].ToString();
            descriptionfolder = folderpath + "\\descriptionFolder";
            System.IO.Directory.CreateDirectory(descriptionfolder);
            imagefolder = folderpath + "\\imageFolder";
            System.IO.Directory.CreateDirectory(imagefolder);
            logreportfolder = folderpath + "\\logreportFolder";
            System.IO.Directory.CreateDirectory(logreportfolder);
            pdfFolder = folderpath + "\\pdfFolder";
            System.IO.Directory.CreateDirectory(pdfFolder);
            jsonFolder = folderpath + "\\jsonFolder";
            System.IO.Directory.CreateDirectory(jsonFolder);
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

                log.Debug("Successfully downloaded the image");
                return imagePath;
            }
            catch (Exception ex)
            {
                log.Error("Maintenance: Falied to download the image", ex);
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
            string descritionPath = descriptionfolder + "\\" + tourname + ".txt";
            StreamWriter newTextfile = File.CreateText(descritionPath);
            newTextfile.WriteLine(description);
            newTextfile.Close();
            log.Debug("new tourfile of " + tourname + " was created");
            return descritionPath;
        }



        //-----------------------------------------------------------------------------------------------------------

        //Logfile metodes--------------------------------------------------------------------------------------------

        public string SaveReport(string report, string tourname, string logname)
        {

            string logReportPath = logreportfolder + "\\" + tourname + logname + ".txt";
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

        public bool CopyFile(string filename, string newfilename)
        {

            File.Copy(filename, newfilename);
            return File.Exists(newfilename);
        }




        //Create Report PDF files----------------------------------------------------------------------- 

        //Create Tour Report PDF
        public bool CreateTourReport(Tour currentTour, List<RawRouteInfo> routeList, List<Log> logList)
        {

            try
            {
                //create pdf FilePath with name---------
                string pdfPath = pdfFolder + "\\" + currentTour.Name + ".pdf";
                int filenumber = 0;
                //check if file with this name already exists
                while (File.Exists(pdfPath))
                {
                    filenumber++;
                    pdfPath = pdfFolder + "\\" + currentTour.Name + filenumber + ".pdf";
                }
                //create pdf File----------

                PdfWriter writer = new PdfWriter(pdfPath);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                //create header image and Line Separaton----

                Paragraph header = new Paragraph(currentTour.Name).SetTextAlignment(TextAlignment.CENTER).SetFontSize(20).SetMarginBottom(10);


                LineSeparator ls = new LineSeparator(new SolidLine()).SetMarginTop(10).SetMarginBottom(10);

                Image map = new Image(ImageDataFactory.Create(currentTour.Imagefile))
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER).SetHeight(240).SetWidth(320);

                document.Add(header);
                document.Add(map);
                document.Add(ls);

                //create part with Tour Data

                document.Add(new Paragraph("Tour Data").SetTextAlignment(TextAlignment.CENTER).SetFontSize(15).SetMarginBottom(10));
                document.Add(new Paragraph($"Start:\t{currentTour.Start}"));
                document.Add(new Paragraph($"End:\t{currentTour.End}"));
                document.Add(new Paragraph($"Creation Date:\t{currentTour.CreationDate}"));
                document.Add(new Paragraph($"Distance:\t{currentTour.Distance}"));
                document.Add(new Paragraph($"Formatted Time:\t{currentTour.FormattedTime}"));

                string description = GetFileText(currentTour.Descriptionfile);
                document.Add(new Paragraph($"Description:\t{description}"));

                document.Add(ls);

                //create Route Information
                document.Add(new Paragraph("Route Data").SetTextAlignment(TextAlignment.CENTER).SetFontSize(15).SetMarginBottom(10));
                foreach (var item in routeList)
                {
                    document.Add(new Paragraph($"{item.maneuverNumber}.) {item.narrative} "));
                    document.Add(new Paragraph($"Für {item.distance}km\tZeit:{item.formattedTime}").SetMarginBottom(10));
                }
                document.Add(ls);

                //create Tour Log Information

                document.Add(new Paragraph("Log Data").SetTextAlignment(TextAlignment.CENTER).SetFontSize(15).SetMarginBottom(10));
                foreach (var item in logList)
                {
                    document.Add(new Paragraph($"Log Name:\t{item.logname}").SetBold());
                    document.Add(new Paragraph($"Creation Date:\t{item.date}"));
                    document.Add(new Paragraph($"Distance:\t{item.distance}km"));
                    document.Add(new Paragraph($"Total Time:\t{item.totalTime}h"));
                    document.Add(new Paragraph($"Travel By:\t{item.travelBy}"));
                    document.Add(new Paragraph($"Average Speed:\t{item.averageSpeed}km/h"));
                    document.Add(new Paragraph($"Recommand Restaurant:\t{item.recommandRestaurant}"));
                    document.Add(new Paragraph($"Recommand Hotel:\t{item.recommandHotel}"));
                    document.Add(new Paragraph($"Sight worth seeing:\t{item.sightWorthSeeing}"));
                    document.Add(new Paragraph($"Rating:\t{item.rating}"));

                    string logReport = GetFileText(item.reportfile);
                    document.Add(new Paragraph($"Log Report:\t{logReport}").SetMarginBottom(10));

                }
                document.Add(ls);
                document.Close();
                log.Debug("Tour Report was successfully created in FileHandler");
                return true;
            }
            catch (Exception ex)
            {
                log.Error("There was an Error in the File Handler while trying to create the Tour Report");
                return false;
            }
        }



        //Create Summarize Report PDF
        public bool CreateSummarizeReport(Tour currentTour, List<Log> logList)
        {
            try
            {
                //create pdf FilePath with name---------
                string pdfPath = pdfFolder + "\\" + currentTour.Name + "Sum.pdf";
                int filenumber = 0;
                //check if file with this name already exists
                while (File.Exists(pdfPath))
                {
                    filenumber++;
                    pdfPath = pdfFolder + "\\" + currentTour.Name + "Sum" + filenumber + ".pdf";
                }


                //create pdf File----------

                PdfWriter writer = new PdfWriter(pdfPath);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                //create header image and Line Separaton----

                Paragraph header = new Paragraph(currentTour.Name).SetTextAlignment(TextAlignment.CENTER).SetFontSize(20).SetMarginBottom(10);


                LineSeparator ls = new LineSeparator(new SolidLine()).SetMarginTop(10).SetMarginBottom(10);

                Image map = new Image(ImageDataFactory.Create(currentTour.Imagefile))
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER).SetHeight(240).SetWidth(320);

                document.Add(header);
                document.Add(map);
                document.Add(ls);

                //create part with Tour Data

                document.Add(new Paragraph("Tour Data").SetTextAlignment(TextAlignment.CENTER).SetFontSize(15).SetMarginBottom(10));
                document.Add(new Paragraph($"Start:\t{currentTour.Start}"));
                document.Add(new Paragraph($"End:\t{currentTour.End}"));
                document.Add(new Paragraph($"Creation Date:\t{currentTour.CreationDate}"));
                document.Add(new Paragraph($"Distance:\t{currentTour.Distance}km"));
                document.Add(new Paragraph($"Formatted Time:\t{currentTour.FormattedTime}"));

                string description = GetFileText(currentTour.Descriptionfile);
                document.Add(new Paragraph($"Description:\t{description}"));

                document.Add(ls);

                //create Tour Log Summary;
                document.Add(new Paragraph("Log Data Summary").SetTextAlignment(TextAlignment.CENTER).SetFontSize(15).SetMarginBottom(10));
                int numberLogs = logList.Count;
                float averageRating = 0;
                float averageSpeed = 0;
                float sumDistance = 0;
                float sumTotalTime = 0;

                foreach (var item in logList)
                {
                    averageRating += item.rating;
                    averageSpeed += float.Parse(item.averageSpeed);
                    sumDistance += float.Parse(item.distance);
                    sumTotalTime += float.Parse(item.totalTime);
                }
                if (numberLogs > 0)
                {
                    averageRating = averageRating / numberLogs;
                    averageSpeed = averageSpeed / numberLogs;
                }

                document.Add(new Paragraph($"Number of Tour Logs:\t{numberLogs}"));
                document.Add(new Paragraph($"Average Rating:\t{averageRating:0.0}"));
                document.Add(new Paragraph($"Average Speed:\t{averageSpeed:0.0}km/h"));
                document.Add(new Paragraph($"Distance Summary:\t{sumDistance:0.0km}"));
                document.Add(new Paragraph($"TotalTime Summary:\t{sumTotalTime:0.0}h"));

                document.Add(ls);
                document.Close();
                log.Debug("Tour SummarizeReport was successfully created in FileHandler");
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error in FilHandler while trying to create SummarizeReport");
                return false;
            }
        }
        //----------------------------------------------------------------------------------------------

        //Export Tour Data as Json File-----------------------------------------------------------------
        public bool ExportTour(JsonTour exportData)
        {

            try
            {
                //create a Json File out of this JsonTour Object 

                string jsonFile = JsonConvert.SerializeObject(exportData);

                //check if json name already exists in the json folder
                string jsonFilename = jsonFolder + "\\" + exportData.tourData.Name + ".json";
                int filenumber = 0;
                while (File.Exists(jsonFilename))
                {
                    filenumber++;
                    jsonFilename = jsonFolder + "\\" + exportData.tourData.Name + filenumber + ".json"; ;
                }

                //save the json folder in the file
                using (var tw = new StreamWriter(jsonFilename, true))
                {
                    tw.WriteLine(jsonFile.ToString());
                    tw.Close();
                }
                log.Debug("Successfully Exported Tour in Filehandler");
                return true;
            }

            catch
            {
                log.Error("Error in Filehandler while trying to Export Tour");
                return false;
            }
        }

        //----------------------------------------------------------------------------------------------






        //Import Tour Data from Json File---------------------------------------------------------------
        public bool CheckJsonFile(string jsonFilePath)
        {


            if (!File.Exists(jsonFilePath))
            {
                return false;
            }

            return true;
        }






        public JObject GetJsonFile(string jsonFilePath)
        {
            JObject jsonTour;
            try
            {
                jsonTour = JObject.Parse(File.ReadAllText(jsonFilePath));
                return jsonTour;
            }
            catch (JsonReaderException jex)
            {
                //Exception in parsing json
                Console.WriteLine(jex.Message);
                return null;
            }
        }
        //----------------------------------------------------------------------------------------------

        //save base64 string image------------------------------------------------------------------
        public string SaveImage(string tourname, string base64Image)
        {

            //check if file already existsexists 
            string filepath = imagefolder +"\\"+ tourname + ".png";

            try
            {

                byte[] data = Convert.FromBase64String(base64Image);
                using (var imageFile = new FileStream(filepath, FileMode.Create))
                {
                    imageFile.Write(data, 0, data.Length);
                    imageFile.Close();
                 
                }
                log.Debug("Successfully saved image in file in Filehanlder");
                return filepath;
            }
            catch
            {
                log.Error("Error while trying to save image in Filehandler");
                return "";
            }
        }

        //--------------------------------------------------------------------------------------------
    }
}
