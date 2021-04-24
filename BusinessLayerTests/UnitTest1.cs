using NUnit.Framework;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace BusinessLayerTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RegexTestEins()
        {
            Tour onlyTimeAndDistance = new Tour
            {
                Distance = 100,
                FormattedTime = "00:20"
            };


            var databaseConnection = UnitTestMocks.GeneratesDatabaseTourOrdersMock(UnitTestMocks.standardTourList(), true);
            var httpConnection = UnitTestMocks.GeneratesHttpConnectionMock("myJsonResponse");
            var fileHandler = UnitTestMocks.GenerateFileHanlderMock("file/image.png", "file/description.txt", true, true);
            var httpResponseHandler = UnitTestMocks.GetResponseHandlerMock(UnitTestMocks.standardRawRouteInfoList(), UnitTestMocks.standardMainMapSearchData(), onlyTimeAndDistance);

            ITourItemFactory element = new TourItemFactoryImpl(databaseConnection.Object, httpConnection.Object, fileHandler.Object, httpResponseHandler.Object);
            TourSearch goodtest = new TourSearch
            {
                fromCity = "Wien",
                fromCountry = "Austria",
                toCity = "Graz",
                toCountry = "Austria",
                tourDescription="Dies ist ein Test"
            };
            bool Expected = true;

            bool result=element.CheckSearchOption(goodtest);
            Assert.AreEqual(Expected, result);
        }
    }
}