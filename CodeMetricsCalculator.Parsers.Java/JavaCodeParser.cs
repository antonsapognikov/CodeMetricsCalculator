using System;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    public abstract class JavaCodeParser<TJavaMember, TParsingResult> : ICodeParser<TJavaMember, TParsingResult>
        where TJavaMember : JavaCode
    {
        public abstract TParsingResult Parse(TJavaMember code);

        protected int FindClosingBracketIndex(string source, string openingBracket, string closingBracket,
                                              int startOpeningBracketIndex)
        {
            var bracketsCounter = 1;
            var lastBracketIndex = startOpeningBracketIndex;
            while (bracketsCounter != 0)
            {
                var openingBracketIndex = source.IndexOf(openingBracket, lastBracketIndex + 1, StringComparison.Ordinal);
                var closeingBracketIndex = source.IndexOf(closingBracket, lastBracketIndex + 1, StringComparison.Ordinal);
                if (closeingBracketIndex == -1) //не нашли закрывающую скобку 
                    return -1;

                if (openingBracketIndex < closeingBracketIndex)
                {
                    lastBracketIndex = openingBracketIndex;
                    bracketsCounter++;
                }
                else
                {
                    lastBracketIndex = closeingBracketIndex;
                    bracketsCounter--;
                }
            }
            return lastBracketIndex;
        }
    }
}