using NUnit.Framework;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;




namespace BusinessLayerTests
{
    public class Tests
    {
        ITourItemFactory element;

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
            var httpConnection = UnitTestMocks.GeneratesHttpConnectionMock("myJsonResponse");
            var fileHandler = UnitTestMocks.GenerateFileHanlderMock("file\\image.png", "file\\description.txt", "Ich bin ein File Text", "file\\report.txt",null);
            var httpResponseHandler = UnitTestMocks.GetResponseHandlerMock(UnitTestMocks.StandardRawRouteInfoList(), UnitTestMocks.StandardMainMapSearchData(), onlyTimeAndDistance);

            element = new TourItemFactoryImpl(databaseConnection.Object, databaserouteOrders.Object, httpConnection.Object, fileHandler.Object, httpResponseHandler.Object);
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

            bool result=element.CheckNewTourData(goodtest);
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

            bool result = element.CheckNewTourData(goodtest);
            Assert.AreEqual(Expected, result);

        }
        [Test]
        public void RegexTestFalseSigns()
        {

            TourSearch badTest = new TourSearch
            {
                newTourName = "!!hello",
                fromCity = "hello?",
                fromCountry = "hello",
                toCity = "hello+-",
                toCountry = "Austria",
                tourDescription = "Dies ist ein Test, mal sehen ob das auch geht."
            };
            bool Expected = false;

            bool result = element.CheckNewTourData(badTest);
            Assert.AreEqual(Expected, result);

        }
        //###################################################################################################################


        //SearchTourTests-----------------------------------------------------------------------------------
        [Test]
        public void SearchTourTestFullWord()
        {

            string foundTourname = "TestTourZwei";

            IEnumerable<Tour> foundTour = element.SearchTours("Graz", "Start");
            
            Assert.AreEqual(foundTourname, foundTour.First().Name);
        }
        [Test]
        public void SearchTourTestWordPart()
        {

            string foundTournameOne = "TestTour";
            string foundTournameTwo = "TestTourZwei";
            
            string[] list = new string[2];
            IEnumerable<Tour> foundTour = element.SearchTours("Test", "Name");
           
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
            IEnumerable<Tour> foundTour = element.SearchTours("150", "Distance");

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


            element.CreateTours(test);

            IEnumerable <Tour> newTour = element.SearchTours("Paris", "Start");

            Assert.AreEqual(test.newTourName, newTour.First().Name);


        }

    }
}