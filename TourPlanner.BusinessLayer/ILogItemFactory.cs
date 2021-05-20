using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{

    public delegate void UpdateLogsEventHandler(object source, EventArgs args);
    public interface ILogItemFactory
    {
        void setUpdateLogsEventhandler(UpdateLogsEventHandler newEvent);
        IEnumerable<Log> GetLogs(string currentTourName);
        void SetCurrentTourName(string currentTourName);
        string GetCurrentTourName();
        string CreateNewTourLog(Log myNewLog, string report);
        string GetLogReport(string reportfile);
        IEnumerable<Log> SearchLogs(string searchElement, string searchOption);
        bool DeleteCurrentLog(Log currentLog);

        bool CopyCurrentLog(string currentTourName, Log currentLog);

        void SetCurrentLog(Log myCurrentLog);
        Log GetCurrentLog();
    }
}
