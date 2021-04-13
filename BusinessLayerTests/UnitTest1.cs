using NUnit.Framework;
using TourPlanner.BusinessLayer;
namespace BusinessLayerTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            TourItemFactory factory = new TourItemFactory();
            ITourItemFactory element = TourItemFactory.GetInstance();
        }
    }
}