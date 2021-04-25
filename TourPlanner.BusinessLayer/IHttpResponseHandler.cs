using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public interface IHttpResponseHandler
    {
        void SetJObject(string jsonstring);
        List<RawRouteInfo> GrabRouteData(string newtourName);
        MainMapSearchData GrabMainMapData();
        Tour GrabMainTimeAndDistance();
    }
}
