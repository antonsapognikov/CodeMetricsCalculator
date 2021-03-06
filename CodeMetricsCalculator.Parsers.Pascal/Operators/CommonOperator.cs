﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.Operators
{
    internal abstract class CommonOperator : PascalOperator
    {
        private readonly OperationType _operationType;
        private readonly OperatorSyntax _syntax;

        protected CommonOperator(string operatorString, string keyword, OperationType operationType, OperatorSyntax syntax)
            : base(GeneratePattern(operatorString, operationType, syntax), keyword)
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
            Contract.Requires(operatorString != null, "operatorString");

            if (operationType == OperationType.Unary)
            {
                return syntax == OperatorSyntax.Prefix
                    ? new Pattern(operatorString + Pattern.Operand)
                    : new Pattern(Pattern.Operand + operatorString);
            }
            if (operationType == OperationType.Binary)
                return new Pattern(Pattern.Operand + " " + operatorString + " " + Pattern.Operand);
            return new Pattern(@" ? " + Pattern.Operand + " : " + Pattern.Operand);
        }
    }
}
