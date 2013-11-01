using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        
        internal static BlockOperator ParseAsBlockOperator(string source)
        {
            foreach (BlockOperator blockOperator in BlockOperator.All)
            {
                var parsingRegex = BuildRegexForOperator(blockOperator);
                var matches = parsingRegex.Matches(source);
                if (matches.Cast<Match>().Any(match => match.Length == source.Length))
                {
                    return blockOperator;
                }
            }
            return null;
        }

        private static Regex BuildRegexForOperator(JavaOperator operatorInfo)
        {
            return operatorInfo.Pattern.ToRegex();
        }
    }
}