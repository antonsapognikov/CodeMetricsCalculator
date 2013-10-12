using System.Text.RegularExpressions;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    /// <summary>
    ///     Bitwise and Bit Shift Operators (~, <<, >>, >>>, &, |, ^).
    /// </summary>
    internal class BitwiseOperator : JavaOperator
    {
        public BitwiseOperator(string operatorString)
            : base(operatorString, new Regex(string.Format(@"[^{0}]{0}[^{0}=]", operatorString), RegexOptions.Compiled))
        {
        }

        public BitwiseOperator(string operatorString, Regex regex)
            : base(operatorString, regex)
        {
        }
    }
}