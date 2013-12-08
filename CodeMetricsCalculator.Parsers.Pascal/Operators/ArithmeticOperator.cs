using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.Operators
{
    /// <summary>
    ///     Arithmetic Pascal operator (+, -, *, /, %).
    /// </summary>
    internal class ArithmeticOperator : CommonOperator
    {
        private static readonly List<ArithmeticOperator> AllOperators = new List<ArithmeticOperator>
        {
            new ArithmeticOperator("+"),
            new ArithmeticOperator("-"),
            new ArithmeticOperator("*"),
            new ArithmeticOperator("/"),
            new ArithmeticOperator("div"),
            new ArithmeticOperator("mod")
        };

        public ArithmeticOperator(string operatorString)
            : base(operatorString, operatorString, OperationType.Binary, OperatorSyntax.Infix)
                
        {
        }

        public static IReadOnlyCollection<ArithmeticOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
    }
}