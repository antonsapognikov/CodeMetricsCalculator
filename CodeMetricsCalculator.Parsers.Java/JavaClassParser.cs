using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Exceptions;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    public class JavaClassParser : JavaCodeParser<JavaCode, IReadOnlyCollection<JavaClass>>,
                                   IClassParser<JavaCode, JavaClass>
    {
        static JavaClassParser()
        {
            ClassNameRegex = new Regex(@"class +([a-zA-Z][a-zA-Z1-9]+) +{", RegexOptions.Compiled);
        }

        private static readonly Regex ClassNameRegex;

        public override IReadOnlyCollection<JavaClass> Parse(JavaCode code)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            var classSources = ParseClassSources(code.NormalizedSource);
            var classes = classSources.Select(s => new JavaClass(ParseClassName(s), s)).ToList();
            return classes.AsReadOnly();
        }

        private IEnumerable<string> ParseClassSources(string sources)
        {
            var classSources = new List<string>();

            var startClassIndex = sources.IndexOf("class ", StringComparison.Ordinal);

            while (startClassIndex != -1)
            {
                var classOpeningBracketIndex = sources.IndexOf('{', startClassIndex);
                if (classOpeningBracketIndex == -1)
                    throw new ParsingException("No opening bracket after keyword 'class'.");
                var classClosingBracketIndex = FindClosingBracketIndex(sources, "{", "}", classOpeningBracketIndex);
                if (classClosingBracketIndex == -1)
                    throw new ParsingException("No closing bracket for class.");
                var classSource = sources.Substring(startClassIndex, classClosingBracketIndex - startClassIndex + 1);
                classSources.Add(classSource);
                startClassIndex = sources.IndexOf("class ", classClosingBracketIndex, StringComparison.Ordinal);
            }
            return classSources;
        }

        private static string ParseClassName(string classSource)
        {
            return ClassNameRegex.Matches(classSource)[0].Groups[1].Value;
        }
    }
}
