using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public interface IHttpConnection
    {
        string getJsonResponse(TourSearch searchData); 

    }
}