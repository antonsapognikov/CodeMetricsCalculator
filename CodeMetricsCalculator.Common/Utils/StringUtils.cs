using System;

namespace CodeMetricsCalculator.Common.Utils
{
    public static class StringUtils
    {
        private const string QuotesFormat = "\"{0}\"";
        
        public static string Quotes(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            return string.Format(QuotesFormat, value);
        }
    }
}
