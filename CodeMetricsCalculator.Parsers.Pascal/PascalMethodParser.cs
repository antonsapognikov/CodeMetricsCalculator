using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Exceptions;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal
{
    internal class PascalMethodParser : PascalCodeParser<PascalClass, IReadOnlyCollection<PascalMethod>>,
        IMethodParser<PascalClass, PascalMethod>
    {

        private const string MethodRegexString =
            @"function +([a-z_][a-z0-9_]*)\.([a-z_][a-z0-9_]*) *\(([a-z_][a-z0-9<>,:;\.>_ ]*)\): *([a-z_][a-z0-9<>,\.>_]*) *;";

        static PascalMethodParser()
        {
            MethodRegex = new Regex(MethodRegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        private static readonly Regex MethodRegex;

        public override IReadOnlyCollection<PascalMethod> Parse(PascalClass code)
        {
            if (code == null)
                throw new NullReferenceException("code");
            
            var classSource = code.Implementation;
            var methodSources = ParseMethodSources(classSource);
            var methods =
                methodSources.Where(s => IsMethodBelongToClass(s, code.Name)).
                    Select(s => new PascalMethod(ParseReturnType(s), ParseMethodName(s), ParseParameters(s), s, code)).
                    ToList();
            return methods.AsReadOnly();
        }

        private static ITypeInfo ParseReturnType(string methodSource)
        {
            var typeName = MethodRegex.Matches(methodSource)[0].Groups[4].Value;
            return new PascalType(typeName);
        }

        private static IEnumerable<IMethodParameterInfo> ParseParameters(string methodSource)
        {
            var parametersString = MethodRegex.Matches(methodSource)[0].Groups[3].Value;
            if (string.IsNullOrWhiteSpace(parametersString))
                return Enumerable.Empty<IMethodParameterInfo>();
            var parameterSources = parametersString.Split(';').Select(s => s.Trim(' '));
            var parameters = new List<IMethodParameterInfo>();
            foreach (var parameterSource in parameterSources)
            {
                var typeAndParameterName = parameterSource.Split(new []{':'}, StringSplitOptions.RemoveEmptyEntries);
                if (typeAndParameterName.Length != 2)
                    throw new ParsingException("Method parameter parsing error.");
                var type = new PascalType(typeAndParameterName[1].Trim(' '));
                parameters.Add(new PascalMethodParameter(type, typeAndParameterName[0].Trim(' '), parameterSource));
            }
            return parameters;
        }

        private IEnumerable<string> ParseMethodSources(string classSources)
        {
            var methodSources = new List<string>();

            var matches = MethodRegex.Matches(classSources);
            foreach (var match in matches.Cast<Match>())
            {
                var startMethodIndex = match.Index;
                var methodOpeningBracketIndex = classSources.IndexOf("begin", startMethodIndex,
                    StringComparison.OrdinalIgnoreCase);
                if (methodOpeningBracketIndex == -1)
                    throw new ParsingException("No opening bracket after method declaration.");
                var methodClosingBracketIndex = FindClosingBracketIndex(classSources, "begin", "end;", methodOpeningBracketIndex);
                if (methodClosingBracketIndex == -1)
                    throw new ParsingException("No closing bracket for method.");
                var methodSource = classSources.Substring(startMethodIndex, methodClosingBracketIndex - startMethodIndex + 4);
                methodSources.Add(methodSource);
            }
            return methodSources;
        }

        private static bool IsMethodBelongToClass(string methodSource, string className)
        {
            var classFromMethod = MethodRegex.Matches(methodSource)[0].Groups[1].Value;
            return classFromMethod.Equals(className, StringComparison.OrdinalIgnoreCase);
        }

        private static string ParseMethodName(string methodSource)
        {
            return MethodRegex.Matches(methodSource)[0].Groups[2].Value;
        }
    }
}
