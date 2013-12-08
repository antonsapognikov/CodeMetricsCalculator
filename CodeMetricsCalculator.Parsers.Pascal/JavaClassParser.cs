using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Exceptions;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal
{
    public class PascalClassParser : PascalCodeParser<PascalCode, IReadOnlyCollection<PascalClass>>,
                                   IClassParser<PascalCode, PascalClass>
    {
        static PascalClassParser()
        {
            ClassNameRegex = new Regex(@"class +([a-zA-Z_][a-zA-Z0-9<>_]*) *" +
                                       @"(extends [a-zA-Z_][a-zA-Z0-9<,>_ ]*)? *" +
                                       @"(implements [a-zA-Z_][a-zA-Z0-9<,>_ ]*)?[\r\n ]*{",
                RegexOptions.Compiled);
        }

        private static readonly Regex ClassNameRegex;

        public override IReadOnlyCollection<PascalClass> Parse(PascalCode code)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            var classSources = ParseClassSources(code.NormalizedSource);
            var classes = classSources.Select(s => new PascalClass(ParseClassName(s), s)).ToList();
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
            var match = ClassNameRegex.Matches(classSource)[0];
            return match.Groups[1].Value;
        }
    }
}
