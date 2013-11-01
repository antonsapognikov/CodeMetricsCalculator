using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    internal abstract class CommonOperator : JavaOperator
    {
        private readonly OperationType _operationType;
        private readonly OperatorSyntax _syntax;

        protected CommonOperator(string operatorString, OperationType operationType, OperatorSyntax syntax)
            : base(GeneratePattern(operatorString, operationType, syntax))
        {
            _operationType = operationType;
            _syntax = syntax;
        }

        public OperationType OperationType
        {
            get { return _operationType; }
        }

        public OperatorSyntax Syntax
        {
            get { return _syntax; }
        }

        private static Pattern GeneratePattern(string operatorString, OperationType operationType, OperatorSyntax syntax)
        {
            Contract.Requires<ArgumentNullException>(operatorString != null, "operatorString");

            if (operationType == OperationType.Unary)
            {
                return syntax == OperatorSyntax.Prefix
                    ? new Pattern(operatorString + Pattern.Identifier)
                    : new Pattern(Pattern.Identifier + operatorString);
            }
            if (operationType == OperationType.Binary)
                return new Pattern(Pattern.Identifier + " " + operatorString + " " + Pattern.Identifier);
            return new Pattern(@" ? " + Pattern.Identifier + " : " + Pattern.Identifier);
        }
    }
}
