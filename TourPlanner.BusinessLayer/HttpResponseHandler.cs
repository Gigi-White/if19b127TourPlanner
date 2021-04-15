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


        public List<RawRouteInfo> GrabRouteData(string newtourName, string jsonstring)
        {
           /*
            if (routeInfoList != null)
            {
                routeInfoList.Clear();
            }
            if(jsonData!= null)
            {
                jsonData.RemoveAll();
            }
           */
            routeInfoList = new List<RawRouteInfo>();
            jsonData = JObject.Parse(jsonstring);

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

    }
}
