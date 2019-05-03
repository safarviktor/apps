using System;
using System.Collections;
using System.Collections.Generic;
using Dapper;
using OsloBysykkel.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace OsloBysykkel.DataAccess
{
    public class StationAvailabilitiesRepository : BaseRepository
    {
        public async Task AddStationAvailability(StationAvailabilityAtTime availability)
        {
            await WithDatabaseConnection(async c =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@StationId", availability.Station.Id, DbType.Int32);
                parameters.Add("@LogDateTime", availability.Time, DbType.DateTime);
                parameters.Add("@Bikes", availability.Availability.Bikes, DbType.Int32);
                parameters.Add("@Locks", availability.Availability.Locks, DbType.Int32);
                parameters.Add("@UpdatedAt", availability.Availability.UpdatedAt, DbType.DateTime);
                parameters.Add("@RefreshRate", availability.Availability.RefreshRate, DbType.Decimal);

                var sql = $@"                
                INSERT INTO ob.StationAvailabilities
                (StationId, LogDateTime, Bikes, Locks, UpdatedAt, RefreshRate)
                SELECT @StationId, @LogDateTime, @Bikes, @Locks, @UpdatedAt, @RefreshRate

                SELECT SCOPE_IDENTITY();
                ";

                return await c.QueryAsync(sql, parameters);
            });
        }

        public async Task AddStationAvailabilities(IEnumerable<StationAvailabilityAtTime> data)
        {
            var dt = new DataTable();
            dt.Columns.Add("StationId");
            dt.Columns.Add("LogDateTime");
            dt.Columns.Add("Bikes");
            dt.Columns.Add("Locks");
            dt.Columns.Add("UpdatedAt");
            dt.Columns.Add("RefreshRate");

            foreach (var row in data)
            {
                var values = new object[6];
                values[0] = row.Station.Id;
                values[1] = row.Time;
                values[2] = row.Availability.Bikes;
                values[3] = row.Availability.Locks;
                values[4] = row.Availability.UpdatedAt;
                values[5] = row.Availability.RefreshRate;

                dt.Rows.Add(values);
            }

            using (var sqlBulk = new SqlBulkCopy(ConnectionString))
            {
                sqlBulk.DestinationTableName = "ob.StationAvailabilities";
                await sqlBulk.WriteToServerAsync(dt);
            }
        }
    }
}
