using SkyNet.Services.Shared.Extensions;

namespace CleanArchitecture.Services.Shared.Helpers
{
    public interface ITimestampHelper : ISingletonService
    {
        DateTime GetCurrent();
        DateTime GetCurrentUTC();
        DateTime GetEntityDateTime(DateTime dateTimeInUTC, string entity);
    }

    public class TimestampHelper : ITimestampHelper
    {
        public DateTime GetCurrent() => DateTime.Now;
        public DateTime GetCurrentUTC() => DateTime.UtcNow;

        public DateTime GetEntityDateTime(DateTime dateTimeInUTC, string timezoneId)
        {
            try
            {
                if (!string.IsNullOrEmpty(timezoneId))
                {
                    var utc = new DateTime(dateTimeInUTC.Year, dateTimeInUTC.Month, dateTimeInUTC.Day, dateTimeInUTC.Hour,
                        dateTimeInUTC.Minute, dateTimeInUTC.Second, dateTimeInUTC.Millisecond, DateTimeKind.Utc);
                    var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                    return TimeZoneInfo.ConvertTimeFromUtc(utc, timezone);
                }
                else
                    return dateTimeInUTC;
            }
            catch (Exception)
            {
                return dateTimeInUTC;
            }
        }
    }
}
