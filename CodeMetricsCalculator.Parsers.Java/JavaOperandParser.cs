using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.Operators;

namespace CodeMetricsCalculator.Parsers.Java
{
    public class JavaOperandParser : JavaCodeParser<JavaExpression, OperandParsingResult<JavaOperand>>,
                                     IOperandParser<JavaExpression, JavaOperand>
    {
        public override OperandParsingResult<JavaOperand> Parse(JavaExpression code)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            var parsingResult = new OperandParsingResult<JavaOperand>();
            string source = code.NormalizedSource;
            foreach (JavaOperator javaOperator in JavaOperator.Operators)
            {
                var parsingRegex = BuildRegexForOperator(javaOperator);
                var matches = parsingRegex.Matches(source).Cast<Match>();
                foreach (var match in matches)
                {
                    for (int i = 1; i < match.Groups.Count; i++)
                    {
                        if (string.IsNullOrWhiteSpace(match.Groups[i].Value))
                            continue;
                        var operand = new JavaOperand(match.Groups[i].Value, match.Groups[i].Value);
                        if (parsingResult.ContainsKey(operand))
                            parsingResult[operand]++;
                        else
                            parsingResult.Add(operand, 1);
                    }
                }
            }
            return parsingResult;
        }

        private static Regex BuildRegexForOperator(JavaOperator operatorInfo)
        {
            return operatorInfo.Pattern.ToRegex();
        }
    }
}