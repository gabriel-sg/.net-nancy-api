﻿
using System;
using System.Diagnostics;
using System.Globalization;

namespace webapi.Utils
{
    public abstract class Clock
    {
        private sealed class StopwatchClock : Clock
        {
            private static readonly long factor = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            public override long Nanoseconds { get { return Stopwatch.GetTimestamp() * factor; } }
            public override DateTime UTCDateTime { get { return DateTime.UtcNow; } }
        }

        private sealed class SystemClock : Clock
        {
            public override long Nanoseconds { get { return DateTime.UtcNow.Ticks * 100L; } }
            public override DateTime UTCDateTime { get { return DateTime.UtcNow; } }
        }

        public static readonly Clock SystemDateTime = new SystemClock();
        public static readonly Clock Default = new StopwatchClock();

        public abstract long Nanoseconds { get; }
        public abstract DateTime UTCDateTime { get; }


        public static string FormatTimestamp(DateTime timestamp)
        {
            return timestamp.ToString("yyyy-MM-ddTHH:mm:ss.ffffK", CultureInfo.InvariantCulture);
        }
    }

    public static class DateTimeExtensions
    {
        private static readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().ToUniversalTime();

        public static long ToUnixTime(this DateTime date)
        {
            return Convert.ToInt64((date.ToUniversalTime() - unixEpoch).TotalSeconds);
        }
    }
}
