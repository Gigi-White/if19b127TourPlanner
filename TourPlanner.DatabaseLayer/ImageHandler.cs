using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class ImageHandler
    {

        public  List<RawRouteInfo> downloadImages(List<RawRouteInfo> RouteInfo)
        {
            
            string folderpath= ConfigurationManager.AppSettings["ImageFolderDirectory"].ToString();
            System.IO.Directory.CreateDirectory(folderpath);
            string newTourFolder = folderpath + "\\" + RouteInfo[0].tourName;
            foreach (RawRouteInfo Route in RouteInfo)
            {
                if (Route.mapurl != "")
                {
                    string imagePath = newTourFolder + "\\" + Route.tourName + Route.maneuverNumber.ToString() + "image.png";
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile(Route.mapurl, imagePath);
                    }
                    Route.mapImagePath = imagePath;
                }
            }

            return RouteInfo;
        }


        public void deleteFolder()
        {

        }
    }
}
