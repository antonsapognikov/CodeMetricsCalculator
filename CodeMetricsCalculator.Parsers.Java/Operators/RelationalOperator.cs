using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
#pragma warning disable 1570
    /// <summary>
    ///     Relational Java operator (==, !=, >, >=, <, <=).
    /// </summary>
#pragma warning restore 1570
    internal class RelationalOperator : CommonOperator
    {
        private static readonly List<RelationalOperator> AllOperators = new List<RelationalOperator>
        {
            new RelationalOperator("=="),
            new RelationalOperator("!="),
            new RelationalOperator(">"),
            new RelationalOperator(">="),
            new RelationalOperator("<"),
            new RelationalOperator("<=")
        };
        
        public RelationalOperator(string operatorString)
            : base(operatorString, OperationType.Binary, OperatorSyntax.Infix)
        {
        }

        public static IReadOnlyCollection<RelationalOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
    }
}