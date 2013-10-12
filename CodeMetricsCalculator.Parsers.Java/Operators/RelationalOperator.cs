using System.Text.RegularExpressions;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
#pragma warning disable 1570
    /// <summary>
    ///     Relational Java operator (==, !=, >, >=, <, <=).
    /// </summary>
#pragma warning restore 1570
    internal class RelationalOperator : JavaOperator
    {
        //todo: регулярка уныла
        public RelationalOperator(string operatorString)
            : base(operatorString, new Regex(string.Format(@"[^{0}]{0}[^{0}=]", operatorString), RegexOptions.Compiled))
        {
        }

        public RelationalOperator(string operatorString, Regex regex)
            : base(operatorString, regex)
        {
        }
    }
}