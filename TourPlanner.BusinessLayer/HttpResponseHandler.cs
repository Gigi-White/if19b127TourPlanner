using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using TourPlanner.Models;
namespace TourPlanner.BusinessLayer
{
    class HttpResponseHandler
    {
        private List<RawRouteInfo> routeInfoList;

        private JObject jsonData;

        public HttpResponseHandler(string jsonstring)
        {
            jsonData = JObject.Parse(jsonstring);
        }

        public List<RawRouteInfo> GrabRouteData(string newtourName)
        {
          
            routeInfoList = new List<RawRouteInfo>();
            fillList(newtourName);

            return routeInfoList;
        }

        private void fillList(string newtourName)
        {

            foreach (var maneuver in jsonData["route"]["legs"][0]["maneuvers"]) 
            {
                routeInfoList.Add(new RawRouteInfo
                    {
                        tourName = newtourName,
                        maneuverNumber = int.Parse(maneuver["index"].ToString()),
                        narrative = maneuver["narrative"].ToString(),
                        distance = float.Parse(maneuver["distance"].ToString()),
                        formattedTime = maneuver["formattedTime"].ToString()
                    }

                );

            }
        }

        public MainMapSearchData GrabMainMapData()
        {
            MainMapSearchData searchData = new MainMapSearchData();

            searchData.sessionId = jsonData["route"]["sessionId"].ToString();

            searchData.boundingBox = new List<string>() ;
            searchData.boundingBox.Add(jsonData["route"]["boundingBox"]["ul"]["lat"].ToString());
            searchData.boundingBox.Add(jsonData["route"]["boundingBox"]["ul"]["lng"].ToString());
            searchData.boundingBox.Add(jsonData["route"]["boundingBox"]["lr"]["lat"].ToString());
            searchData.boundingBox.Add(jsonData["route"]["boundingBox"]["lr"]["lng"].ToString());

            for (int i=0;i < searchData.boundingBox.Count; i++)
            {
                searchData.boundingBox[i] = searchData.boundingBox[i].Replace(",", ".");
            }

            return searchData;

        }


        public Tour GrabMainTimeAndDistance()
        {
            Tour newTour = new Tour();
            newTour.FormattedTime = jsonData["route"]["legs"][0]["formattedTime"].ToString();
            newTour.Distance = float.Parse(jsonData["route"]["distance"].ToString());

            return newTour;


        }
        
    }
}
