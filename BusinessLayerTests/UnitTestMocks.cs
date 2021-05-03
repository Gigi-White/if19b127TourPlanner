using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using TourPlanner.BusinessLayer;
using TourPlanner.DataAccessLayer;
using TourPlanner.DataAccessLayer.SQLDatabase;
using TourPlanner.Models;

namespace BusinessLayerTests
{
    public class UnitTestMocks
    {
        public static Mock<IDatabaseTourOrders> GeneratesDatabaseTourOrdersMock(List<Tour> myTours, bool saveToursAnswer)
        {
            var mockDatabaseTourOrders = new Mock<IDatabaseTourOrders>();
            List<Tour> allTours = myTours;
            bool mySaveToursAnswer = saveToursAnswer;
            //create fake FileInfo Array from files array;

            mockDatabaseTourOrders.Setup(mr => mr.GetTours())
                       .Returns(allTours);

            mockDatabaseTourOrders.Setup(mr => mr.SaveTours(It.IsAny<Tour>()))
                       .Returns(mySaveToursAnswer);

            mockDatabaseTourOrders.Setup(mr => mr.ChangeTour(It.IsAny<Tour>()))
                       .Returns(true);

            mockDatabaseTourOrders.Setup(mr => mr.CopyTour(It.IsAny<string>()))
                       .Returns(true);
            
            mockDatabaseTourOrders.Setup(mr => mr.DeleteTour(It.IsAny<string>()))
                       .Returns(true);

            return mockDatabaseTourOrders;
        }

        public static Mock<IDatabaseRouteOrders> GeneratesDatabaseRouteOrdersMock(List<RawRouteInfo> routeinfoList, string Tourname)
        {
            var mockDatabaseRouteOrders = new Mock<IDatabaseRouteOrders>();
            List<RawRouteInfo> routeInfo = routeinfoList;

            mockDatabaseRouteOrders.Setup(mr => mr.SaveRouteInfo(It.IsAny<List<RawRouteInfo>>()))
                       .Returns(true);

            mockDatabaseRouteOrders.Setup(mr => mr.GetRouteInfo(It.IsAny<string>()))
                       .Returns(routeinfoList);

            mockDatabaseRouteOrders.Setup(mr => mr.DeleteRouteData(It.IsAny<string>()))
                       .Returns(true);

            return mockDatabaseRouteOrders;

        }

      



        public static Mock<IHttpConnection> GeneratesHttpConnectionMock(string jsonResponse)
        {
            var mockHttpConnection = new Mock<IHttpConnection>();

            string myJsonResponse = jsonResponse;

            mockHttpConnection.Setup(mr => mr.GetJsonResponse(It.IsAny<TourSearch>()))
                .Returns(jsonResponse);

            return mockHttpConnection;
        }


        public static Mock<IFileHandler> GenerateFileHanlderMock(string imagepath, string descriptionpath, bool deleteImageAnswer, bool deleteDescriptionAnswer)
        {
            var mockFileHanlder = new Mock<IFileHandler>();
            string myimagepath = imagepath;
            string mydescriptionpath = descriptionpath;
            bool myDeleteImageAnswer = deleteImageAnswer;
            bool myDeleteDescriptionAnswer = deleteDescriptionAnswer;

            mockFileHanlder.Setup(mr => mr.DownloadSaveImage(It.IsAny<MainMapSearchData>(), It.IsAny<string>()))
                .Returns(myimagepath);

            mockFileHanlder.Setup(mr => mr.SaveDescription(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(mydescriptionpath);

            mockFileHanlder.Setup(mr => mr.DeleteImage(It.IsAny<string>()))
                .Returns(myDeleteImageAnswer);

            mockFileHanlder.Setup(mr => mr.DeleteDescription(It.IsAny<string>()))
                .Returns(myDeleteDescriptionAnswer);

            return mockFileHanlder;
        }

        public static Mock<IHttpResponseHandler> GetResponseHandlerMock(List<RawRouteInfo> routeinfoList, MainMapSearchData mapDataItem, Tour newtour)
        {
            var mockHttpResponseHandler = new Mock<IHttpResponseHandler>();
            var myRouteInfoList = routeinfoList;
            var myMapDataItem = mapDataItem;
            var myNewtour = newtour;

            mockHttpResponseHandler.Setup(mr => mr.SetJObject(It.IsAny<string>()))
                .Verifiable();

            mockHttpResponseHandler.Setup(mr => mr.GrabRouteData(It.IsAny<string>()))
                .Returns(myRouteInfoList);

            mockHttpResponseHandler.Setup(mr => mr.GrabMainMapData())
                .Returns(myMapDataItem);
            mockHttpResponseHandler.Setup(mr => mr.GrabMainTimeAndDistance())
                .Returns(myNewtour);



            return mockHttpResponseHandler;


        }

        public static List<Tour> standardTourList()
        {

            return new List<Tour>
            {
                new Tour
                {
                    Name="TestTour",
                    Start="Wien",
                    End="Graz",
                    CreationDate="25.12.1993",
                    Descriptionfile="file/description.txt",
                    Distance= 100,
                    FormattedTime="2 Stunden",
                    Imagefile="file/image.png"
                },
                new Tour
                {
                     Name="TestTourZwei",
                    Start="Graz",
                    End="Wien",
                    CreationDate="25.12.1994",
                    Descriptionfile="file/description.txt",
                    Distance= 200,
                    FormattedTime="2 Stunden",
                    Imagefile="file/image.png"
                }
            };

        }
        public static List<RawRouteInfo> standardRawRouteInfoList()
        {
            return new List<RawRouteInfo>
            {
                new RawRouteInfo
                {
                tourName = "TestTourEins",
                maneuverNumber = 1,
                narrative ="jetzt rechts abbiegen",
                distance = 5,
                formattedTime = "20 min"
                },
                new RawRouteInfo
                {
                tourName = "TestTourEins",
                maneuverNumber = 2,
                narrative ="jetzt links abbiegen",
                distance = 2,
                formattedTime = "10 min"
                }

            };
        }
        public static MainMapSearchData standardMainMapSearchData()
        {
            return new MainMapSearchData
            {
                sessionId = "123456789",

                boundingBox = new List<string>() { "10", "20", "30", "40" }
            };
    
        }
    }
    
}
