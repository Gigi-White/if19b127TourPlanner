using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public interface IDatabaseConnection
    {
        NpgsqlDataReader getDatabaseData(NpgsqlCommand sqlCommand);

        bool updateDatabaseData(NpgsqlCommand sqlCommand);
    }
}
