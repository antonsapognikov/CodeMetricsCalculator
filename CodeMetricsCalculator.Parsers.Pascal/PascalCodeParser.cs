using System;
using System.Collections.Generic;
using System.Linq;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal
{
    public abstract class PascalCodeParser
    {
        protected const string IdentifierPattern = @"[a-zA-Z1-9_]+";
        protected const string ArgumentsPattern = "[\"'a-zA-Z1-9,_ ]+";
        
        protected int FindClosingBracketIndex(string source, string openingBracket, string closingBracket,
                                              int startOpeningBracketIndex)
        {
            var bracketsCounter = 1;
            var lastBracketIndex = startOpeningBracketIndex;
            while (bracketsCounter != 0)
            {
                var openingBracketIndex = source.IndexOf(openingBracket, lastBracketIndex + 1, StringComparison.Ordinal);
                var closingBracketIndex = source.IndexOf(closingBracket, lastBracketIndex + 1, StringComparison.Ordinal);
                if (closingBracketIndex == -1) //не нашли закрывающую скобку 
                    return -1;

                if (openingBracketIndex < closingBracketIndex && openingBracketIndex != -1)
                {
                    lastBracketIndex = openingBracketIndex;
                    bracketsCounter++;
                }
                else
                {
                    lastBracketIndex = closingBracketIndex;
                    bracketsCounter--;
                }
            }
            return lastBracketIndex;
        }

        protected static IReadOnlyCollection<PascalType> GetUsedTypes(PascalClass code)
        {
            var types = new List<PascalType>();
            types.AddRange(code.GetFields().Select(info => info.Type).Cast<PascalType>());
            var methods = code.GetMethods();
            types.AddRange(methods.Select(info => info.ReturnType).Cast<PascalType>());
            var variablesTypes = methods.SelectMany(info => info.GetVariables()).Select(pair => pair.Key.Type).Cast<PascalType>();
            types.AddRange(variablesTypes);
            return types.Distinct().ToList();
        } 
    }

    public abstract class PascalCodeParser<TPascalMember, TParsingResult> : PascalCodeParser, ICodeParser<TPascalMember, TParsingResult>
        where TPascalMember : PascalCode
    {
        public abstract TParsingResult Parse(TPascalMember code);
    }
}