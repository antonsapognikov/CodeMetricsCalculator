using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.Operators;

namespace CodeMetricsCalculator.Parsers.Java
{
    public class JavaOperatorParser : JavaCodeParser<JavaExpression, OperatorParsingResult<JavaOperator>>,
                                      IOperatorParser<JavaExpression, JavaOperator>
    {
        public override OperatorParsingResult<JavaOperator> Parse(JavaExpression code)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            var parsingResult = new OperatorParsingResult<JavaOperator>();
            string source = code.NormalizedSource;
            foreach (JavaOperator javaOperator in JavaOperator.Operators)
            {
                var parsingRegex = BuildRegexForOperator(javaOperator);
                var operatorCount = parsingRegex.Matches(source).Count;
                if (operatorCount != 0)
                    parsingResult.Add(javaOperator, operatorCount);
            }
            return parsingResult;
        }

        private static Regex BuildRegexForOperator(JavaOperator operatorInfo)
        {
            if (operatorInfo.IsPrimary)
            {
                var primaryOperator = operatorInfo as PrimaryOperator;
                Debug.Assert(primaryOperator != null);

                if (primaryOperator == PrimaryOperator.CreateArray)
                    throw new NotImplementedException();
                if (primaryOperator == PrimaryOperator.CreateObject)
                    throw new NotImplementedException();
                //...
                    
                throw new NotImplementedException();
            }

            if (operatorInfo.IsBlock)
            {
                var blockOperator = operatorInfo as BlockOperator;
                Debug.Assert(blockOperator != null);

                if (blockOperator == BlockOperator.DoWhile)
                    throw new NotImplementedException();
                if (blockOperator == BlockOperator.For)
                    throw new NotImplementedException();
                //...

                throw new NotImplementedException();
            }

            if (operatorInfo is CommonOperator)
            {
                var commonOperator = operatorInfo as CommonOperator;
                var operatorString = Regex.Escape(operatorInfo.Name);

                if (commonOperator.OperationType == OperationType.Ternary)
                    return new Regex(@"\?.+:", RegexOptions.Compiled);

                //todo: надо как-то различать префиксные и постфиксные, унарные и бинарные операторы
                return new Regex(string.Format(@"[^{0}]{0}[^{0}=]", operatorString), RegexOptions.Compiled);
            }

            throw new NotSupportedException(); //не должны сюда придти
        }
    }
}