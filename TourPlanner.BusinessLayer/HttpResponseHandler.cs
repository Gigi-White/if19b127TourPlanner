using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
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
                        mapurl = (maneuver["mapUrl"]!= null) ? maneuver["mapUrl"].ToString() : "",
                        distance = float.Parse(maneuver["distance"].ToString()),
                        formattedTime = maneuver["formattedTime"].ToString()
                    }

                );

            }
        }

        public MainMapSearchData GrabMainMapData()
        {
            MainMapSearchData searchData = new MainMapSearchData();

            return searchData;
        }

    }
}
