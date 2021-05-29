using NUnit.Framework;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;




namespace BusinessLayerTests
{
    public class Tests
    {
        ITourItemFactory testTourWorker;

        [SetUp]
        public void Setup()
        {
            Tour onlyTimeAndDistance = new Tour
            {
                Distance = 100,
                FormattedTime = "00:20"
            };  
            string tourname = "mytour";       
            var databaseConnection = UnitTestMocks.GeneratesDatabaseTourOrdersMock(UnitTestMocks.StandardTourList(), true);
            var databaserouteOrders = UnitTestMocks.GeneratesDatabaseRouteOrdersMock(UnitTestMocks.StandardRawRouteInfoList(), tourname);
            var databaselogOrders = UnitTestMocks.GeneratesDatabaseLogOrdersMock(UnitTestMocks.StandardLogList());
            var httpConnection = UnitTestMocks.GeneratesHttpConnectionMock("myJsonResponse");
            var fileHandler = UnitTestMocks.GenerateFileHanlderMock("file\\image.png", "file\\description.txt", "Ich bin ein File Text", "file\\report.txt",null);
            var httpResponseHandler = UnitTestMocks.GetResponseHandlerMock(UnitTestMocks.StandardRawRouteInfoList(), UnitTestMocks.StandardMainMapSearchData(), onlyTimeAndDistance);

            testTourWorker = new TourItemFactoryImpl(databaseConnection.Object, databaserouteOrders.Object, databaselogOrders.Object, httpConnection.Object, fileHandler.Object, httpResponseHandler.Object);
        }


        //Regex Tests------------------------------------------------------------------------------------------------------
        [Test]
        public void RegexTestNumbersLetters()
        {

            TourSearch goodtest = new TourSearch
            {
                newTourName="TestTour",
                fromCity = "Wien",
                fromCountry = "Austria",
                toCity = "Graz",
                toCountry = "Austria",
                tourDescription="999"
            };
            bool Expected = true;

            bool result=testTourWorker.CheckNewTourData(goodtest);
            Assert.AreEqual(Expected, result);

        }
        [Test]
        public void RegexTestDotsCommasAndSpaces()
        {

            TourSearch goodtest = new TourSearch
            {
                newTourName = "Test Tour",
                fromCity = "Wien.",
                fromCountry = "Austria,",
                toCity = "Graz",
                toCountry = "Austria",
                tourDescription = "Dies ist ein Test, mal sehen ob das auch geht."
            };
            bool Expected = true;

            bool result = testTourWorker.CheckNewTourData(goodtest);
            Assert.AreEqual(Expected, result);

        }
        [Test]
        public void RegexTestFalseSigns()
        {

            TourSearch badTest = new TourSearch
            {
                newTourName = "hallo\"",
                fromCity = "hello",
                fromCountry = "hello",
                toCity = "hello",
                toCountry = "Austria",
                tourDescription = "Dies ist ein Test, mal sehen ob das auch geht."
            };
            bool Expected = false;

            bool result = testTourWorker.CheckNewTourData(badTest);
            Assert.AreEqual(Expected, result);

        }

        [Test]
        public void RegexTestSQLInjection()
        {

            TourSearch badTest = new TourSearch
            {
                newTourName = "My new tour;UPDATE USER SET TYPE=\"admin\" WHERE ID=23;",
                fromCity = "hello",
                fromCountry = "hello",
                toCity = "hello",
                toCountry = "Austria",
                tourDescription = "Dies ist ein Test, mal sehen ob das auch geht."
            };
            bool Expected = false;

            bool result = testTourWorker.CheckNewTourData(badTest);
            Assert.AreEqual(Expected, result);

        }
        //###################################################################################################################


        //SearchTourTests-----------------------------------------------------------------------------------
        [Test]
        public void SearchTourTestFullWord()
        {

            string foundTourname = "TestTourZwei";

            IEnumerable<Tour> foundTour = testTourWorker.SearchTours("Graz", "Start");
            
            Assert.AreEqual(foundTourname, foundTour.First().Name);
        }
        [Test]
        public void SearchTourTestWordPart()
        {

            string foundTournameOne = "TestTour";
            string foundTournameTwo = "TestTourZwei";
            
            string[] list = new string[2];
            IEnumerable<Tour> foundTour = testTourWorker.SearchTours("Test", "Name");
           
            int i = 0;
            foreach (Tour item in foundTour)
            {
                list[i] = item.Name;
                i++;
            }

            Assert.AreEqual(foundTournameOne, list[0]);
            Assert.AreEqual(foundTournameTwo, list[1]);
        }


        [Test]
        public void SearchTourTestDistancet()
        {

            string foundTourname = "TestTourZwei";

            //string[] list = new string[2];
            IEnumerable<Tour> foundTour = testTourWorker.SearchTours("150", "Distance");

            Assert.AreEqual(foundTourname, foundTour.First().Name);
           
        }




        //##############################################################################################

        //CreateTours
        [Test]
        public void CreateToursTest()
        {

            TourSearch test = new TourSearch
            {
                newTourName = "mynewTour",
                fromCity = "Paris",
                fromCountry = "France",
                toCity = "Rome",
                toCountry = "Italy",
                tourDescription = "dies ist wieder ein Test"
            };


            testTourWorker.CreateTours(test);

            IEnumerable <Tour> newTour = testTourWorker.SearchTours("Paris", "Start");

            Assert.AreEqual(test.newTourName, newTour.First().Name);


        }

        //DeleteTour
        [Test]
        public void DeleteToursTest()
        {



            IEnumerable<Tour> testList = testTourWorker.GetTours();
            Assert.AreEqual(2, testList.Count());
            
            bool result = testTourWorker.DeleteCurrentTour(testList.First());
            IEnumerable<Tour> changedList = testTourWorker.GetTours();

            Assert.AreEqual(true, result);
            Assert.AreEqual(1, changedList.Count());


        }

        //CopyTour
        [Test]
        public void CopyTourTest() {
            IEnumerable<Tour> testList = testTourWorker.GetTours();
            Assert.AreEqual(2, testList.Count());

            bool result = testTourWorker.CopyCurrentTour(testList.First());
            IEnumerable<Tour> changedList = testTourWorker.GetTours();

            Assert.AreEqual(true, result);
            Assert.AreEqual(3, changedList.Count());
            Assert.AreEqual("TestTourcopy", changedList.Last().Name);

        }
        //ModifyTour
        [Test]
        public void ModifyTourTest()
        {

            bool result = testTourWorker.ModifyTour("TestTour","Modified Tour","Text I gues");
            IEnumerable<Tour> changedList = testTourWorker.GetTours();

            Assert.AreEqual(true, result);
            Assert.AreEqual("Modified Tour", changedList.First().Name);

        }


        //#######################################################################################

        //HTTP Response Handler
        [Test]
        public void checkHTTPResponseHandler()
        {
            IHttpResponseHandler myResponseHandler = new HttpResponseHandler();
            string myJsonString = UnitTestMocks.TestMapqestResponse().ToString();
            myResponseHandler.SetJObject(myJsonString);

            List <RawRouteInfo> routeList = myResponseHandler.GrabRouteData("testTour");

            Assert.AreEqual(2, routeList.Count());
        }
    }
}