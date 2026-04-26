namespace YourApp.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixMs(this DateTime dt) =>
            new DateTimeOffset(dt).ToUnixTimeMilliseconds();

        public static long? ToUnixMs(this DateTime? dt) =>
            dt.HasValue ? new DateTimeOffset(dt.Value).ToUnixTimeMilliseconds() : null;

        public static DateTime FromUnixMs(this long ms) =>
            DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime;
    }
}