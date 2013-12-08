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
            ClassRegex = new Regex(@"([a-z_][a-z0-9_]*) *= *class *\(",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        private static readonly Regex ClassRegex;

        public override IReadOnlyCollection<PascalClass> Parse(PascalCode code)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            var source = code.NormalizedSource;
            var declarations = ParseClassDeclarations(source).ToArray();
            var implementationSection = ParseImplementation(source);

            return
                declarations.Select(s => new PascalClass(ParseClassName(s), s, implementationSection))
                    .ToList()
                    .AsReadOnly();
        }

        private static IEnumerable<string> ParseClassDeclarations(string source)
        {
            var typeDeclarationsSource = ParseTypeSection(source);
            var classDeclarations = ClassRegex.Matches(typeDeclarationsSource);
            foreach (var classDeclaration in classDeclarations.Cast<Match>())
            {
                var endIndex = typeDeclarationsSource.IndexOf("end;", classDeclaration.Index,
                    StringComparison.Ordinal);
                if (endIndex == -1)
                    throw new ParsingException("There is no end key.");
                yield return
                    typeDeclarationsSource.Substring(classDeclaration.Index, endIndex + 3 - classDeclaration.Index);
            }
        }

        private static string ParseTypeSection(string source)
        {
            var typeKeywordIndex = source.IndexOf("type", StringComparison.OrdinalIgnoreCase);
            var implementationKeywordIndex = source.IndexOf("implementation", StringComparison.OrdinalIgnoreCase);
            if (typeKeywordIndex == -1 || implementationKeywordIndex == -1)
                throw new ParsingException("There is no typeKeyword or implementationKeyword.");
            var typeDeclarationsSource = source.Substring(typeKeywordIndex + 4,
                implementationKeywordIndex - (typeKeywordIndex + 4));
            return typeDeclarationsSource;
        }

        private static string ParseImplementation(string source)
        {
            var implementationKeywordIndex = source.IndexOf("implementation", StringComparison.OrdinalIgnoreCase);
            if (implementationKeywordIndex == -1)
                throw new ParsingException("There is no implementationKeyword.");
            return source.Substring(implementationKeywordIndex + "implementation".Length);
        }

        private static string ParseClassName(string declarationSource)
        {
            var match = ClassRegex.Matches(declarationSource)[0];
            return match.Groups[1].Value;
        }
    }
}
