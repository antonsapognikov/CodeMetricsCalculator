using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Exceptions;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal class JavaMethodParser : JavaCodeParser<JavaClass, IReadOnlyCollection<JavaMethod>>,
        IMethodParser<JavaClass, JavaMethod>
    {
        static JavaMethodParser()
        {
            //{return type} {method name} (..
            //todo add generic method support
            MethodNameRegex = new Regex(@"[a-zA-Z][a-zA-Z1-9<>]+ +([a-zA-Z][a-zA-Z1-9]+) *" +
                                        @"\([a-zA-Z1-9<> ,]+\) *{", RegexOptions.Compiled);

        }

        private static readonly Regex MethodNameRegex;

        public override IReadOnlyCollection<JavaMethod> Parse(JavaClass code)
        {
            if (code == null)
                throw new NullReferenceException("code");

            var classSource = code.NormalizedSource;
            var methodSources = ParseMethodSources(classSource);
            var methods = methodSources.Select(s => new JavaMethod(ParseMethodName(s), code, s)).ToList();
            return methods.AsReadOnly();
        }

        private IEnumerable<string> ParseMethodSources(string sources)
        {
            var methodSources = new List<string>();

            var matches = MethodNameRegex.Matches(sources);
            foreach (var match in matches.Cast<Match>())
            {
                var startMethodIndex = match.Index;
                var methodOpeningBracketIndex = sources.IndexOf('{', startMethodIndex);
                if (methodOpeningBracketIndex == -1)
                    throw new ParsingException("No opening bracket after method declaration.");
                var methodClosingBracketIndex = FindClosingBracketIndex(sources, "{", "}", methodOpeningBracketIndex);
                if (methodClosingBracketIndex == -1)
                    throw new ParsingException("No closing bracket for method.");
                var methodSource = sources.Substring(startMethodIndex, methodClosingBracketIndex - startMethodIndex + 1);
                methodSources.Add(methodSource);
            }
            return methodSources;
        }

        private static string ParseMethodName(string methodSource)
        {
            return MethodNameRegex.Matches(methodSource)[0].Groups[1].Value;
        }
    }
}
