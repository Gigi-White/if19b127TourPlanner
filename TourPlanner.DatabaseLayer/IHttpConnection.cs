using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    interface IHttpConnection
    {
        string getJsonResponse(TourSearch searchData); 

    }
}