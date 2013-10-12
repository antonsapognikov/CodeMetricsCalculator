using System.Text.RegularExpressions;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    internal class InstanceofOperator : JavaOperator
    {
        public InstanceofOperator()
            : base("instanceof", new Regex(@" instanceof  ", RegexOptions.Compiled))
        {
        }
    }
}