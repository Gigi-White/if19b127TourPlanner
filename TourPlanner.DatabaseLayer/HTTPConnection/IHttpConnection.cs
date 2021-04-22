using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public interface IHttpConnection
    {
        string GetJsonResponse(TourSearch searchData); 

    }
}