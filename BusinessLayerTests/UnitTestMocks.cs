using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Newtonsoft.Json.Linq;
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
            //create fake FileInfo Array from files array;

            mockDatabaseTourOrders.Setup(mr => mr.GetTours())
                       .Returns(myTours);
            mockDatabaseTourOrders.Setup(mr => mr.SaveTours(It.IsAny<Tour>()))
                       .Returns(saveToursAnswer);
            mockDatabaseTourOrders.Setup(mr => mr.ChangeTour(It.IsAny<string>(),It.IsAny<string>()))
                       .Returns(true);           
            mockDatabaseTourOrders.Setup(mr => mr.DeleteTour(It.IsAny<string>()))
                       .Returns(true);

            return mockDatabaseTourOrders;
        }

        public static Mock<IDatabaseRouteOrders> GeneratesDatabaseRouteOrdersMock(List<RawRouteInfo> routeinfoList, string Tourname)
        {
            var mockDatabaseRouteOrders = new Mock<IDatabaseRouteOrders>();

            mockDatabaseRouteOrders.Setup(mr => mr.SaveRouteInfo(It.IsAny<List<RawRouteInfo>>()))
                       .Returns(true);
            mockDatabaseRouteOrders.Setup(mr => mr.GetRouteInfo(It.IsAny<string>()))
                       .Returns(routeinfoList);
            mockDatabaseRouteOrders.Setup(mr => mr.DeleteRouteData(It.IsAny<string>()))
                       .Returns(true);
            mockDatabaseRouteOrders.Setup(mr => mr.CopyRouteInfo(It.IsAny<string>(), It.IsAny<string>()))
                       .Returns(true);

            return mockDatabaseRouteOrders;

        }

      
        public static Mock<IDatabaseLogOrders> GeneratesDatabaseLogOrdersMock(List<Log> logList)
        {
            var mockDatabaseLogOrders = new Mock<IDatabaseLogOrders>();

            mockDatabaseLogOrders.Setup(mr => mr.CreateLog(It.IsAny<Log>()))
                .Returns(true);
            mockDatabaseLogOrders.Setup(mr => mr.GetLogsofTour(It.IsAny<string>()))
                .Returns(logList);
            mockDatabaseLogOrders.Setup(mr => mr.UpdateLog(It.IsAny<string>(), It.IsAny<Log>()))
                .Returns(true);
            mockDatabaseLogOrders.Setup(mr => mr.DeleteOneLog(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            mockDatabaseLogOrders.Setup(mr => mr.DeleteAllLogsofTour(It.IsAny<string>()))
                .Returns(true);
            mockDatabaseLogOrders.Setup(mr => mr.CopyLogsofTour(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            return mockDatabaseLogOrders;
        }


        public static Mock<IHttpConnection> GeneratesHttpConnectionMock(string jsonResponse)
        {
            var mockHttpConnection = new Mock<IHttpConnection>();

            mockHttpConnection.Setup(mr => mr.GetJsonResponse(It.IsAny<TourSearch>()))
                .Returns(jsonResponse);

            return mockHttpConnection;
        }


        public static Mock<IFileHandler> GenerateFileHanlderMock(string imagepath, string descriptionpath, string getFileText, string reportPath, JObject getJsonFile)
        {
            var mockFileHanlder = new Mock<IFileHandler>();

            mockFileHanlder.Setup(mr => mr.DownloadSaveImage(It.IsAny<MainMapSearchData>(), It.IsAny<string>()))
                .Returns(imagepath);
            mockFileHanlder.Setup(mr => mr.SaveDescription(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(descriptionpath);
            mockFileHanlder.Setup(mr => mr.DeleteImage(It.IsAny<string>()))
                .Returns(true);
            mockFileHanlder.Setup(mr => mr.DeleteFile(It.IsAny<string>()))
                .Returns(true);
            mockFileHanlder.Setup(mr => mr.CopyFile(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(true);
            mockFileHanlder.Setup(mr => mr.GetFileText(It.IsAny<string>()))
                .Returns(getFileText);
            mockFileHanlder.Setup(mr => mr.SaveReport(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(reportPath);
            mockFileHanlder.Setup(mr => mr.CreateTourReport(It.IsAny<Tour>(), It.IsAny<List<RawRouteInfo>>(), It.IsAny<List<Log>>()))
                .Returns(true);
            mockFileHanlder.Setup(mr => mr.CreateSummarizeReport(It.IsAny<Tour>(), It.IsAny<List<Log>>()))
                .Returns(true);
            mockFileHanlder.Setup(mr => mr.ExportTour(It.IsAny<JsonTour>()))
                .Returns(true);
            mockFileHanlder.Setup(mr => mr.CheckJsonFile(It.IsAny<string>()))
                .Returns(true);
            mockFileHanlder.Setup(mr => mr.GetJsonFile(It.IsAny<string>()))
                .Returns(getJsonFile);
            mockFileHanlder.Setup(mr => mr.SaveImage(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(imagepath);

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

        public static List<Tour> StandardTourList()
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
        public static List<RawRouteInfo> StandardRawRouteInfoList()
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
        public static List<Log> StandardLogList()
        {
            return new List<Log>{
                new Log
                {
                    tourname = "TestTourEins",
                    logname = "TestLogEins",
                    date = "25.12.2005",
                    reportfile = "file/report.txt",
                    distance = "20",
                    totalTime = "1,4",
                    rating = 5,
                    travelBy = "car",
                    averageSpeed = "70",
                    recommandRestaurant = "TestRestaurantEins",
                    recommandHotel = "TestHotelEins",
                    sightWorthSeeing = "TestLandschaftEins"
                },
                new Log
                {
                    tourname = "TestTourZwei",
                    logname = "TestLogZwei",
                    date = "25.01.2005",
                    reportfile = "file/reportzwei.txt",
                    distance = "30",
                    totalTime = "5,4",
                    rating = 3,
                    travelBy = "on foot",
                    averageSpeed = "50",
                    recommandRestaurant = "TestRestaurantZwei",
                    recommandHotel = "TestHotelZwei",
                    sightWorthSeeing = "TestLandschaftZwei"
                }

            };
        }

        public static JObject StandardJObect()
        {

        }
        public static MainMapSearchData StandardMainMapSearchData()
        {
            return new MainMapSearchData
            {
                sessionId = "123456789",

                boundingBox = new List<string>() { "10", "20", "30", "40" }
            };
    
        }
    }
    
}
