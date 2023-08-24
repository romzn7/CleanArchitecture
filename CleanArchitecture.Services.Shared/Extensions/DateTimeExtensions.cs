namespace CleanArchitecture.Services.Shared.Extensions;

public static class DateTimeExtensions
{
    public static DateTime GetPreviousMonthLastDayOfWeek(this DateTime dateEntry, DayOfWeek day)
    {
        var newDate = dateEntry.AddMonths(-1);
        DateTime lastDayOfMonth = new DateTime(newDate.Year, newDate.Month, 1)
            .AddMonths(1).AddDays(-1);
        int wantedDay = (int)day;
        int lastDay = (int)lastDayOfMonth.DayOfWeek;
        return lastDayOfMonth.AddDays(
            lastDay >= wantedDay ? wantedDay - lastDay : wantedDay - lastDay - 7);
    }


    public static DateTime GetEndDateByStartDate(this DateTime startDate)
    {
        int numberOfWeeks = 51;
        var endDate = startDate.AddDays(numberOfWeeks * 7); // adding weeks

        // Return date of coming saturday
        DateTime finalEndDate = endDate.AddDays(0); // (0) instead of (1)
        while (finalEndDate.DayOfWeek != DayOfWeek.Saturday)
            finalEndDate = finalEndDate.AddDays(1);

        return finalEndDate;
    }

    public static bool IsWeekend(this DateTime date) => date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;

    public static DateTime GetNextDayOfWeekDateInclusive(this DateTime date, DayOfWeek dayOfWeek)
    {
        var newDate = new DateTime(date.Ticks, date.Kind);
        while (newDate.DayOfWeek != dayOfWeek)
            newDate = newDate.AddDays(1);

        return newDate;
    }

    public static string GetTimestamp(this DateTime date) =>
        date.ToString("yyyyMMddHHmmssfff");

    private const long _MILLISECOND_FACTOR = 10000;
    private const long _SECOND_FACTOR = 10000000;

    public static long ToEpochTime(this DateTime dateTime) =>
        _ToEpochTime(dateTime, _SECOND_FACTOR);

    public static DateTime ToDateTimeFromEpoch(this long date)
        => _ToDateTimeFromEpoch(date, _SECOND_FACTOR);

    public static long ToEpochTimeMilliseconds(this DateTime dateTime) =>
        _ToEpochTime(dateTime, _MILLISECOND_FACTOR);

    public static DateTime ToDateTimeFromEpochMilliseconds(this long date)
        => _ToDateTimeFromEpoch(date, _MILLISECOND_FACTOR);

    #region Helpers

    private static long _ToEpochTime(this DateTime dateTime, long divisor) => (dateTime.ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks) / 10000;

    public static DateTime _ToDateTimeFromEpoch(this long date, long multiplier)
    {
        long value = date * multiplier;
        return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddTicks(value);
    }

    #endregion
}
