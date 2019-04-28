using Dapper;
using OsloBysykkel.Models;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OsloBysykkel.DataAccess
{
    public class StationsRepository : BaseRepository
    {
        public async Task<bool> StationExists(int stationId)
        {
            return await WithDatabaseConnection(async c =>
            {
                var sql = $@"
                select 1 from ob.Stations
                where Id = {stationId}";

                var exists = await c.QueryAsync<int>(sql);
                return exists.Any();
            });
        }

        public async Task<int> AddStation(Station station)
        {
            return await WithDatabaseConnection(async c =>
            {
                await InsertStation(c, station);
                await InsertBoundaries(c, station);
                return station.Id;
            });
        }

        private async Task InsertBoundaries(IDbConnection dbConnection, Station station)
        {
            foreach (var bound in station.Bounds)
            {
                var sql = $@"
                INSERT INTO ob.Points
                (Latitude, Longitude)
                SELECT {bound.Latitude}, {bound.Longitude}

                DECLARE @pointId INT = SCOPE_IDENTITY()

                INSERT INTO ob.StationBoundaries
                (StationId, PointId)
                SELECT {station.Id}, @pointId
                ";

                await dbConnection.ExecuteAsync(sql);
            }            
        }

        private async Task InsertStation(IDbConnection dbConnection, Station station)
        {
            var sql = @"DECLARE @centerPoint INT
                ";

            if (station.Center != null)
            {
                sql += $@"
                INSERT INTO ob.Points
                (Latitude, Longitude)
                SELECT {station.Center.Latitude}, {station.Center.Longitude}
                
                SET @centerPoint = SCOPE_IDENTITY()
                ";
            }
            

            sql += $@"
                    
                INSERT INTO ob.Stations
                (Id, Title, Subtitle, NumberOfLocks, CenterPoint)
                SELECT 
                    {station.Id}, 
                    {StringOrNull(station.Title)}, 
                    {StringOrNull(station.Subtitle)}, 
                    {station.Number_Of_Locks}, 
                    @centerPoint 
                ";

            await dbConnection.ExecuteAsync(sql);
        }
    }
}
