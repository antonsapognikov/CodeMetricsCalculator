using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Exceptions;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.Operators;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal class JavaExpressionParser : JavaCodeParser<JavaCode, IReadOnlyCollection<JavaExpression>>,
                                          IExpressionParser<JavaCode, JavaExpression>
    {
        private readonly List<Regex> _blockOperatorRegexps;

        public JavaExpressionParser()
        {
            GC.KeepAlive(JavaOperator.Operators);
            _blockOperatorRegexps = BlockOperator.All.Select(@operator => @operator.Pattern.ToRegex()).ToList();
        }

        public override IReadOnlyCollection<JavaExpression> Parse(JavaCode code)
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
                var blockOperatorLength = 0;
                var blockOperatorIndex = FindBlockOperatorIndex(sources, startExpressionIndex, out blockOperatorLength);

                if (semicolonIndex == -1 && openingBracketIndex == -1)
                    break;

                if (blockOperatorIndex != -1 && openingBracketIndex != -1 &&
                    openingBracketIndex < (blockOperatorIndex + blockOperatorLength) &&
                    blockOperatorIndex < semicolonIndex &&
                    blockOperatorIndex < openingBracketIndex)
                {
                    var closingBracketIndex = FindClosingBracketIndex(sources, "{", "}", openingBracketIndex);
                    if (closingBracketIndex == -1)
                        throw new ParsingException("Cannot find closing brace.");
                    var expressionSource = sources.Substring(blockOperatorIndex,
                        closingBracketIndex - blockOperatorIndex + 1);
                    expressionSources.Add(expressionSource);
                    startExpressionIndex = closingBracketIndex + 1;
                    continue;
                }

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
                    if (closingBracketIndex == -1)
                        throw new ParsingException("Cannot find closing brace.");
                    var expressionSource = sources.Substring(startExpressionIndex,
                        closingBracketIndex - startExpressionIndex + 1);
                    expressionSources.Add(expressionSource);
                    startExpressionIndex = closingBracketIndex + 1;
                }
            }
            return expressionSources;
        }

        private int FindBlockOperatorIndex(string source, int startIndex, out int length)
        {
            var matches =
                       _blockOperatorRegexps.Select(regex => regex.Match(source, startIndex))
                           .Where(match => match.Success)
                           .ToList();
            if (matches.Count == 0)
            {
                length = 0;
                return -1;
            }
            var blockOperatorIndex = matches.Min(match => match.Index);
            length = matches.Find(match => match.Index == blockOperatorIndex).Length;
            return blockOperatorIndex;
        }
    }
}
