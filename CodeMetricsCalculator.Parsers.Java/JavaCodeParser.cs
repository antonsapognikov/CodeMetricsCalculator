using System;
using System.Collections.Generic;
using System.Linq;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    public abstract class JavaCodeParser
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

        protected static IReadOnlyCollection<JavaType> GetUsedTypes(JavaClass code)
        {
            var types = new List<JavaType>();
            types.AddRange(code.GetFields().Select(info => info.Type).Cast<JavaType>());
            var methods = code.GetMethods();
            types.AddRange(methods.Select(info => info.ReturnType).Cast<JavaType>());
            var variablesTypes = methods.SelectMany(info => info.GetVariables()).Select(pair => pair.Key.Type).Cast<JavaType>();
            types.AddRange(variablesTypes);
            return types.Distinct().ToList();
        } 
    }

    public abstract class JavaCodeParser<TJavaMember, TParsingResult> : JavaCodeParser, ICodeParser<TJavaMember, TParsingResult>
        where TJavaMember : JavaCode
    {
        public abstract TParsingResult Parse(TJavaMember code);
    }
}