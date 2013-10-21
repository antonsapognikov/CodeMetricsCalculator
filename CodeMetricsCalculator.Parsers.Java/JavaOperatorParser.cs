using System;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

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
            //слева от оператор что угодно, кроме него самого; справа от него что угодно, кроме него самого и знака '=',
            //чтобы не спутать с бинарными операторами -= и т.п.

            //эскейпим символы слешами
            var operatorString = Regex.Escape(operatorInfo.Name);

            //todo: тут надо серьёзно подумать
            if (operatorInfo.OperationType == OperationType.Ternary) //тернарный оператор в java только один
                return new Regex(@"\?.+:", RegexOptions.Compiled);

            //todo: надо как-то различать префиксные и постфиксные, унарные и бинарные операторы
            return new Regex(string.Format(@"[^{0}]{0}[^{0}=]", operatorString), RegexOptions.Compiled);
        }
    }
}