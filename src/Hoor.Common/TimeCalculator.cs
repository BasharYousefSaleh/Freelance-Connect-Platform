namespace Hoor.Common
{
    using System;

    public static class TimeCalculator
    {
        public static string GetTimeAgo(DateTime time)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - time.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 60)
            {
                return ts.Seconds == 1 ? "قبل ثانية واحدة" :"قبل "+ ts.Seconds + " ثانية";
            }

            if (delta < 60 * 2)
            {
                return "قبل دقيقة واحدة";
            }

            if (delta < 45 * 60)
            {
                return "قبل "+ ts.Minutes + " دقيقة";
            }

            if (delta < 90 * 60)
            {
                return "قبل ساعة واحدة";
            }

            if (delta < 24 * 60 * 60)
            {
                return "قبل " + ts.Hours + " ساعة";
            }

            if (delta < 48 * 60 * 60)
            {
                return "أمس ";
            }

            if (delta < 30 * 24 * 60 * 60)
            {
                return "قبل " + ts.Days + " يوم";
            }

            if (delta < 12 * 30 * 24 * 60 * 60)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "قبل شهر " : "قبل "+ months + " شهر";
            }

            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "قبل سنة واحدة" : "قبل "+ years + " سنة";
        }
    }
}
