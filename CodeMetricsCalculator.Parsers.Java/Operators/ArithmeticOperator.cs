using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    /// <summary>
    ///     Arithmetic Java operator (+, -, *, /, %).
    /// </summary>
    internal class ArithmeticOperator : CommonOperator
    {
        private static readonly List<ArithmeticOperator> AllOperators = new List<ArithmeticOperator>
        {
            new ArithmeticOperator("+"),
            new ArithmeticOperator("-"),
            new ArithmeticOperator("*"),
            new ArithmeticOperator("/"),
            new ArithmeticOperator("%")
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