using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    internal class HttpConnection : IHttpConnection
    {
        private string urlResource;
        private string responseFromServer;

        public HttpConnection()
        {
            urlResource = ConfigurationManager.AppSettings["UrlResource"].ToString();
        }


        public string getJsonResponse(TourSearch searchData)
        {
            string completeRequest = buildRequest(searchData);

            getMapquestData(completeRequest);
            return (responseFromServer);
        }

        private void getMapquestData(string completeRequest)
        {
            try
            {
                WebRequest request = WebRequest.Create(completeRequest);
                // If required by the server, set the credentials.

                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    Console.WriteLine(responseFromServer);
                }


                // Close the response.
                response.Close();
            }
            catch (Exception)
            {
                responseFromServer = "null";
            }
        }

        private string buildRequest(TourSearch searchData)
        {
            string completeRequest = urlResource + "&from=" + searchData.fromCity + "," + searchData.fromCountry + 
                                "&to=" + searchData.toCity + "," + searchData.toCountry;
            return completeRequest;
        }
    }
}
