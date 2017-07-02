using System;

namespace Stories.Data.Sorting
{
    // Taken from Reddit and ported to C#
    public static class Rank
    {
        public static double Hot(int ups, int downs, DateTime CreatedDate)
        {
            var score = ups - downs;
            var order = Math.Log10(Math.Max(Math.Abs(score), 1));
            double sign = 0;

            if (score > 0)
                sign = 1;
            else if (score < 0)
                sign = -1;

            double seconds = EpochSeconds(CreatedDate);

            return Math.Round(sign * order + seconds / 45000, 7);
        }

        public static double Confidence(int ups, int downs)
        {
            if (ups + downs == 0)
                return 0;

            var n = ups + downs;

            double z = 1.281551565545F; // 80% confidence
            double p = (double)ups / n;

            double left = p + 1 / (2 * n) * z * z;
            double right = z * Math.Sqrt(p * (1 - p) / n + z * z / (4*n*n));
            double under = 1 + 1 / n * z * z;

            return (left - right) / under;
        }

        private static double EpochSeconds(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var datetime = date - epoch;

            double microseconds = datetime.Ticks / 10;

            return datetime.Days * 86400 + datetime.Seconds + (microseconds / 1000000);
        }
    }
}
