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
            if (station.Bounds == null)
            {
                return;
            }

            foreach (var bound in station.Bounds)
            {
                var parameters = new DynamicParameters();

                parameters.Add("@StationId", station.Id, DbType.Int32);
                parameters.Add("@Latitude", station.Center.Latitude, DbType.Decimal);
                parameters.Add("@Longitude", station.Center.Longitude, DbType.Decimal);

                var sql = $@"
                INSERT INTO ob.Points
                (Latitude, Longitude)
                SELECT @Latitude, @Longitude

                DECLARE @pointId INT = SCOPE_IDENTITY()

                INSERT INTO ob.StationBoundaries
                (StationId, PointId)
                SELECT @StationId, @pointId
                ";

                await dbConnection.ExecuteAsync(sql);
            }            
        }

        private async Task InsertStation(IDbConnection dbConnection, Station station)
        {

            var parameters = new DynamicParameters();

            var sql = @"DECLARE @centerPoint INT
                ";

            if (station.Center != null)
            {
                parameters.Add("@Latitude", station.Center.Latitude, DbType.Decimal);
                parameters.Add("@Longitude", station.Center.Longitude, DbType.Decimal);

                sql += $@"
                INSERT INTO ob.Points
                (Latitude, Longitude)
                SELECT @Latitude, @Longitude
                
                SET @centerPoint = SCOPE_IDENTITY()
                ";
            }
            
            parameters.Add("@StationId", station.Id, DbType.Int32);
            parameters.Add("@Title", StringOrNull(station.Title), DbType.String);
            parameters.Add("@Subtitle", StringOrNull(station.Subtitle), DbType.String);
            parameters.Add("@NUmberOfLocks", station.Number_Of_Locks, DbType.Int32);

            sql += $@"
                    
                INSERT INTO ob.Stations
                (Id, Title, Subtitle, NumberOfLocks, CenterPointId)
                SELECT 
                    @StationId, 
                    @Title, 
                    @Subtitle, 
                    @NUmberOfLocks, 
                    @centerPoint 
                ";

            await dbConnection.ExecuteAsync(sql, parameters);
        }
    }
}
