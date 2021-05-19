using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public interface ILogItemFactory
    {
        IEnumerable<Log> GetLogs(string currentTourName);
        void SetCurrentTourName(string currentTourName);
        string GetCurrentTourName();
    }
}
