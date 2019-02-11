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
        public async Task<bool> StationExists(int stationId)
        {
            return await WithDatabaseConnection(async c =>
            {
                var sql = $@"
                select 1 from OsloBysykkel.Stations
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
                INSERT INTO OsloBysykkel.Points
                (Latitude, Longitude)
                SELECT {bound.Latitude}, {bound.Longitude}

                DECLARE @pointId INT = SCOPE_IDENTITY()

                INSERT INTO OsloBysykkel.StationBoundaries
                (StationId, PointId)
                SELECT {station.Id}, @pointId
                ";

                await dbConnection.ExecuteAsync(sql);
            }            
        }

        private async Task InsertStation(IDbConnection dbConnection, Station station)
        {
            var sql = $@"
                INSERT INTO OsloBysykkel.Points
                (Latitude, Longitude)
                SELECT {station.Center.Latitude}, {station.Center.Longitude}
                
                DECLARE @centerPoint INT = SCOPE_IDENTITY()
    
                INSERT INTO OsloBysykkel.Stations
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
