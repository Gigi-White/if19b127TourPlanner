using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    class HttpConnection : IHttpConnection
    {
        private string urlResource;
        private string getRequest;

        public HttpConnection()
        {
            urlResource = ConfigurationManager.AppSettings["UrlResource"].ToString();
        }


        public string getJsonResponse(TourSearch searchData)
        {
            buildRequest(searchData);
        }

        private void buildRequest(TourSearch searchData)
        {
            string getRequest = urlResource + "&from=" + searchData.fromCity + "," + searchData.fromCountry + 
                                "&to=" + searchData.toCity + "," + searchData.toCountry;
        }
    }
}
