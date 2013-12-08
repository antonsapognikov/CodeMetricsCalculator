using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.Operators
{
#pragma warning disable 1570
    /// <summary>
    ///     Relational Pascal operator (==, !=, >, >=, <, <=).
    /// </summary>
#pragma warning restore 1570
    internal class RelationalOperator : CommonOperator
    {
        private static readonly List<RelationalOperator> AllOperators = new List<RelationalOperator>
        {
            new RelationalOperator("="),
            new RelationalOperator("<>"),
            new RelationalOperator(">="),
            new RelationalOperator("<="),
            new RelationalOperator(">"),
            new RelationalOperator("<"),
        };
        
        public RelationalOperator(string operatorString)
            : base(operatorString, operatorString, OperationType.Binary, OperatorSyntax.Infix)
        {
        }

        public static IReadOnlyCollection<RelationalOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
    }
}