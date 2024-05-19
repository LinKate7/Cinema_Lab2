namespace Lab2OOP.DateTimeExtension
{
	public class DateTimeExtensions
	{
        public static DateTime UnixTimeStampToDateTime(long unixSeconds)
        {
           
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixSeconds).ToLocalTime();
            return dateTime;
        }

        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeSeconds();
        }
    }
}

