using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using OsloBysykkel.Models;

namespace OsloBysykkel.DataAccess
{
    public class StationsRepository : BaseRepository
    {
        public async Task<int> AddStation(Station station)
        {
            return await WithDatabaseConnection(async c =>
            {
                await PrepareBoundaries(c, station);
                return await InsertStation(c, station);
            });
        }

        private async Task PrepareBoundaries(IDbConnection dbConnection, Station station)
        {
            var sql = @"
                CREATE TABLE ##PointsToInsert
                (
                    Lat DECIMAL(18,15),
                    Long DECIMAL(18,15)
                )
                INSERT INTO ##PointsToInsert (Lat, Long)
                ";

            var endOfLine = " UNION ALL" + Environment.NewLine;

            foreach (var bound in station.Bounds)
            {
                sql += $"SELECT {bound.Latitude}, {bound.Longitude}{endOfLine}";
            }

            sql = sql.Substring(0, sql.Length - endOfLine.Length);

            await dbConnection.ExecuteAsync(sql);
        }

        private Task<int> InsertStation(IDbConnection dbConnection, Station station)
        {
            var sql = $@"
                INSERT INTO OsloBysykkel.Stations
                (Id, Title, Subtitle, NumberOfLocks, CenterLatitude, CenterLongitude)
                SELECT 
                    {station.Id}, 
                    {StringOrNull(station.Title)}, 
                    {StringOrNull(station.Subtitle)}, 
";
        }
    }
}
