using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    /// <summary>
    ///     Bitwise and Bit Shift Operators (<<, >>, >>>, &, |, ^).
    /// </summary>
    internal class BitwiseOperator : CommonOperator
    {
        private static readonly List<BitwiseOperator> AllOperators = new List<BitwiseOperator>
        {
            new BitwiseOperator("<<"),
            new BitwiseOperator(">>"),
            new BitwiseOperator(">>>"),
            new BitwiseOperator("&"),
            new BitwiseOperator("|"),
            new BitwiseOperator("^")
        };

        public BitwiseOperator(string operatorString)
            : base(operatorString, OperationType.Binary, OperatorSyntax.Infix)
        {
        }

        public static IReadOnlyCollection<BitwiseOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
    }
}