using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Common;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal
{
    internal class PascalVariablesParser : PascalCodeParser<PascalMethod, IReadOnlyDictionary<PascalVariable, int>>
    {
        private const string EmptyOrWhiteSpacePattern = @"[ \t]*";
        private const string WhiteSpacePattern = @"[ \t]+";
        private const string TypeIdentifierPattern = @"[a-zA-Z_][a-zA-Z0-9_]+(<[a-zA-Z0-9<,>_ \[\]]+>)?(\[\])?";
        private const string VariableIdentifierPattern = @"[a-zA-Z_][a-zA-Z0-9_]*";
        private const string VariableValuePattern = @"[^;]+";
        private const string BracketArgumentsPattern = @"\([^=]*\)";
        private const string PascalIdentifierPattern = "[^a-zA-Z0-9_]" + "{0}" + "[^a-zA-Z0-9_]";

        private static readonly string DeclarationPattern =
            string.Format(@"{0}{2}{1}{3}{0}(={0}{4}{0})?(,{0}{3}{0}(={0}{4}{0})?)*;", EmptyOrWhiteSpacePattern, WhiteSpacePattern, TypeIdentifierPattern, VariableIdentifierPattern, VariableValuePattern);

        private static readonly string DeclarationTypePattern =
            string.Format(@"^{0}{2}{1}", EmptyOrWhiteSpacePattern, WhiteSpacePattern, TypeIdentifierPattern);

        private static readonly string DeclarationVariableNamePattern =
            string.Format(@"(^|,){0}{1}", EmptyOrWhiteSpacePattern, VariableIdentifierPattern);

        private static readonly string ForeachLoopPattern =
            string.Format(@"for{0}\({0}{2}{1}{3}{0}:{0}{3}{0}\)", EmptyOrWhiteSpacePattern, WhiteSpacePattern, TypeIdentifierPattern, VariableIdentifierPattern);

        private static readonly string ForeachLoopTypePattern =
            string.Format(@"\({0}{1}", EmptyOrWhiteSpacePattern, TypeIdentifierPattern);

        private static readonly string ForeachLoopVariableNamePattern =
            string.Format(@"{1}{0}:", EmptyOrWhiteSpacePattern, VariableIdentifierPattern);

        private static readonly List<string> ReservedIdentifierPatterns = new List<string>
        {
            CreateReservedIdentifierPattern("return"), 
            CreateReservedIdentifierPattern("goto")
        };  

        public override IReadOnlyDictionary<PascalVariable, int> Parse(PascalMethod code)
        {
            Contract.Requires(code != null);

            var methodSource = code.GetBody().NormalizedSource;
            var identifiers = new Dictionary<PascalVariable, int>();

            //parsing method parameters as identifiers
            var parameters = code.Parameters.Cast<PascalMethodParameter>();
            foreach (var methodParameterInfo in parameters)
            {
                var regex = new Regex(string.Format(PascalIdentifierPattern, methodParameterInfo.Name));
                var usageCount = regex.Matches(methodSource).Count + 1; //+declaring
                identifiers.Add(methodParameterInfo, usageCount);
            }

            //parse declared variables
            var variables = ParseVariables(methodSource);
            foreach (var javaVariable in variables)
            {
                var regex = new Regex(string.Format(PascalIdentifierPattern, javaVariable.Name));
                var usageCount = regex.Matches(methodSource).Count;
                identifiers.Add(javaVariable, usageCount);
            }

            return identifiers;
        }

        private IEnumerable<PascalVariable> ParseVariables(string methodSource)
        {
            return ParseDeclarations(methodSource)
                .Concat(ParseForeachLoops(methodSource))
                .Distinct(new BaseEqualityComparer<PascalVariable>((first, second) => first.Name == second.Name, variable => variable.Name.GetHashCode()))
                .ToList()
                .AsReadOnly();
        }

        private IEnumerable<PascalVariable> ParseDeclarations(string normilizedSource)
        {
            return Regex.Matches(normilizedSource, DeclarationPattern).Cast<Match>()
                .Select(match => match.Value)
                .Where(value => ReservedIdentifierPatterns.All(pattern => !Regex.IsMatch(value, pattern)))
                .SelectMany(ParseDeclaration)               
                .ToList();
        }

        private IEnumerable<PascalVariable> ParseForeachLoops(string normilizedSource)
        {
            return Regex.Matches(normilizedSource, ForeachLoopPattern).Cast<Match>()
                .Select(match => ParseForeachLoop(match.Value))
                .ToList();
        }

        private IEnumerable<PascalVariable> ParseDeclaration(string declaration)
        {
            string type = Regex.Match(declaration, DeclarationTypePattern).Value.TrimStart(' ').TrimEnd(' ');
            string withoutType = Regex.Replace(declaration, DeclarationTypePattern, string.Empty);
            string withoutBrackets = Regex.Replace(withoutType, BracketArgumentsPattern, string.Empty);
            List<string> names = Regex.Matches(withoutBrackets, DeclarationVariableNamePattern).Cast<Match>()
                .Select(match => match.Value.TrimStart(',', ' '))
                .ToList();
            return names.Select(value => new PascalVariable(new PascalType(type), value, declaration)).ToList();
        }

        private PascalVariable ParseForeachLoop(string loop)
        {
            string type = Regex.Match(loop, ForeachLoopTypePattern).Value.TrimStart('(', ' ');
            string name = Regex.Match(loop, ForeachLoopVariableNamePattern).Value.TrimEnd(' ', ':');
            return new PascalVariable(new PascalType(type), name, loop);
        }

        private static string CreateReservedIdentifierPattern(string identifier)
        {
            return string.Format("^{0}{2}{1}{3}{0};$", EmptyOrWhiteSpacePattern, WhiteSpacePattern, identifier, VariableValuePattern);
        }
    }
}
