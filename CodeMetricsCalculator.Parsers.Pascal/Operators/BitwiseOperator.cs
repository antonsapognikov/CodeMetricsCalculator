using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.Operators
{
    /// <summary>
    ///     Bitwise and Bit Shift Operators (<<, >>, >>>, &, |, ^).
    /// </summary>
    internal class BitwiseOperator : CommonOperator
    {
        private static readonly List<BitwiseOperator> AllOperators = new List<BitwiseOperator>
        {
            new BitwiseOperator("&"),
            new BitwiseOperator("|"),
            new BitwiseOperator("~"),
            new BitwiseOperator("<<"),
            new BitwiseOperator(">>"),
            new BitwiseOperator("^"),
            new BitwiseOperator("not"),
            new BitwiseOperator("and"),
            new BitwiseOperator("or"),
            new BitwiseOperator("xor"),
            new BitwiseOperator("shl"),
            new BitwiseOperator("shr")
        };

        public BitwiseOperator(string operatorString)
            : base(operatorString, operatorString, OperationType.Binary, OperatorSyntax.Infix)
        {
        }

        public static IReadOnlyCollection<BitwiseOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
    }
}