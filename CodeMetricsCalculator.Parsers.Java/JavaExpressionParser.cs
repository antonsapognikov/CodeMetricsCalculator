using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Exceptions;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal class JavaExpressionParser : JavaCodeParser<JavaMethodBody, IReadOnlyCollection<JavaExpression>>,
                                          IExpressionParser<JavaMethodBody, JavaExpression>
    {
        public override IReadOnlyCollection<JavaExpression> Parse(JavaMethodBody code)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            var expressionSources = ParseExpressionSources(code.NormalizedSource);
            var expressions = expressionSources.Select(s => new JavaExpression(s)).ToList();
            return expressions.AsReadOnly();
        }

        private IEnumerable<string> ParseExpressionSources(string sources)
        {
            var expressionSources = new List<string>();

            var startExpressionIndex = 0;

            while (startExpressionIndex != -1)
            {
                var semicolonIndex = sources.IndexOf(';', startExpressionIndex);
                var openingBracketIndex = sources.IndexOf('{', startExpressionIndex);

                if (semicolonIndex == -1 && openingBracketIndex == -1)
                    break;

                if (semicolonIndex < openingBracketIndex || openingBracketIndex == -1)
                {
                    var expressionSource = sources.Substring(startExpressionIndex,
                        semicolonIndex - startExpressionIndex + 1);
                    expressionSources.Add(expressionSource);
                    startExpressionIndex = semicolonIndex + 1;
                }
                else
                {

                    var closingBracketIndex = FindClosingBracketIndex(sources, "{", "}", openingBracketIndex);
                    var expressionSource = sources.Substring(startExpressionIndex,
                        closingBracketIndex - startExpressionIndex + 1);
                    expressionSources.Add(expressionSource);
                    startExpressionIndex = closingBracketIndex + 1;
                }
            }
            return expressionSources;
        }
    }
}
