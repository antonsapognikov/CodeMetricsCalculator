using System.Text.RegularExpressions;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    /// <summary>
    ///     Arithmetic Java operator (+, -, *, /, %).
    /// </summary>
    internal class ArithmeticOperator : JavaOperator
    {
        //todo: как-то ограничить, чтобы и слева, и справа от операторы были операнды, 
        //todo: иначе это будет унарный оператор
        public ArithmeticOperator(string operatorString)
            : base(operatorString, new Regex(string.Format(@"[^{0}]\{0}[^{0}=]", operatorString), RegexOptions.Compiled)
                )
        {
        }

        public ArithmeticOperator(string operatorString, Regex regex)
            : base(operatorString, regex)
        {
        }
    }
}