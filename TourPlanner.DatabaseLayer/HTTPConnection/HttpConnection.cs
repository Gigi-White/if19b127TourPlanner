using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    internal class HttpConnection : IHttpConnection
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private string urlResource;
        private string responseFromServer;

        public HttpConnection()
        {
            urlResource = ConfigurationManager.AppSettings["UrlResource"].ToString();
        }


        public string GetJsonResponse(TourSearch searchData)
        {
            string completeRequest = BuildRequest(searchData);

            GetMapquestData(completeRequest);
            return (responseFromServer);
        }

        private void GetMapquestData(string completeRequest)
        {
            try
            {
                WebRequest request = WebRequest.Create(completeRequest);
                WebResponse response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);


                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);         
                    responseFromServer = reader.ReadToEnd();
                   
                }
                log.Debug("Sucessfuly got Json Response form Mapquest");
                response.Close();
            }
            catch (Exception)
            {
                log.Error("There was an Error in the Http Connection with Mapqust");
                responseFromServer = "null";
            }
        }

        private string BuildRequest(TourSearch searchData)
        {
            string completeRequest = urlResource + "&from=" + searchData.fromCity + "," + searchData.fromCountry + 
                                "&to=" + searchData.toCity + "," + searchData.toCountry;
            return completeRequest;
        }
    }
}
