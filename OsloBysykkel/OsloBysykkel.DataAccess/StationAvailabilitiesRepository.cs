using Dapper;
using OsloBysykkel.Models;
using System.Data;
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

                return await c.QueryAsync(sql);
            });
        }
    }
}
