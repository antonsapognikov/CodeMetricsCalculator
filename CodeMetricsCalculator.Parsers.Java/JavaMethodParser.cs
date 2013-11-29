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
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal class JavaMethodParser : JavaCodeParser<JavaClass, IReadOnlyCollection<JavaMethod>>,
        IMethodParser<JavaClass, JavaMethod>
    {
        //{return type} {method name} ({parameters}) throws {Identifiers} {
        private const string MethodRegexString = @"([a-zA-Z_][a-zA-Z0-9<>,\.>_]*) +([a-zA-Z_][a-zA-Z0-9<>,\.>_]*) *" +
                                                 @"\(([a-zA-Z0-9<>,\.>_\[\] ]*)\) *(throws [a-zA-Z_][a-zA-Z0-9<>,\.>_ ]*)? *{";

        static JavaMethodParser()
        {
            //todo add generic method support
            MethodRegex = new Regex(MethodRegexString, RegexOptions.Compiled);

        }

        private static readonly Regex MethodRegex;

        public override IReadOnlyCollection<JavaMethod> Parse(JavaClass code)
        {
            if (code == null)
                throw new NullReferenceException("code");
            
            var classSource = code.NormalizedSource;
            var methodSources = ParseMethodSources(classSource);
            var methods =
                methodSources.
                    Select(s => new JavaMethod(ParseReturnType(s), ParseMethodName(s), ParseParameters(s), s, code)).
                    ToList();
            return methods.AsReadOnly();
        }

        private static ITypeInfo ParseReturnType(string methodSource)
        {
            var typeName = MethodRegex.Matches(methodSource)[0].Groups[1].Value;
            return new JavaType(typeName);
        }

        private static IEnumerable<IMethodParameterInfo> ParseParameters(string methodSource)
        {
            var parametersString = MethodRegex.Matches(methodSource)[0].Groups[3].Value;
            if (string.IsNullOrWhiteSpace(parametersString))
                return Enumerable.Empty<IMethodParameterInfo>();
            var parameterSources = parametersString.Split(',').Select(s => s.Trim(' '));
            var parameters = new List<IMethodParameterInfo>();
            foreach (var parameterSource in parameterSources)
            {
                var typeAndParameterName = parameterSource.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
                if (typeAndParameterName.Length != 2)
                    throw new ParsingException("Method parameter parsing error.");
                var type = new JavaType(typeAndParameterName[0]);
                parameters.Add(new JavaMethodParameter(type, typeAndParameterName[1], parameterSource));
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
                var methodOpeningBracketIndex = classSources.IndexOf('{', startMethodIndex);
                if (methodOpeningBracketIndex == -1)
                    throw new ParsingException("No opening bracket after method declaration.");
                var methodClosingBracketIndex = FindClosingBracketIndex(classSources, "{", "}", methodOpeningBracketIndex);
                if (methodClosingBracketIndex == -1)
                    throw new ParsingException("No closing bracket for method.");
                var methodSource = classSources.Substring(startMethodIndex, methodClosingBracketIndex - startMethodIndex + 1);
                methodSources.Add(methodSource);
            }
            return methodSources;
        }

        private static string ParseMethodName(string methodSource)
        {
            return MethodRegex.Matches(methodSource)[0].Groups[2].Value;
        }
    }
}
