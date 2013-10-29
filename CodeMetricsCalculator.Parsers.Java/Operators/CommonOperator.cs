using System;
using System.Collections.Generic;
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
            : base(operatorString, false, false)
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
    }
}
