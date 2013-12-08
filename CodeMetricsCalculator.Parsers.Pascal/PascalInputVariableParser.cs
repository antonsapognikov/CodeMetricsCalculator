using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal
{
    class PascalInputVariableParser : PascalCodeParser<PascalMethod, IReadOnlyCollection<PascalInputVariable>>
    {
        private const string EmptyOrWhiteSpacePattern = @" *";
        private const string PascalArgumentIdentifierPattern = "[^a-zA-Z0-9_]*" + "{0}" + "[^a-zA-Z0-9_]*";
        private const string PascalIdentifierPattern = "[^a-zA-Z0-9_]+" + "{0}" + "[^a-zA-Z0-9_]+";

        private static readonly List<Regex> ControlConstructions =
            new List<Regex>
            {
                new Regex(@"if[^;]+then", RegexOptions.IgnoreCase | RegexOptions.Compiled),
                new Regex(@"case[^;]+of", RegexOptions.IgnoreCase | RegexOptions.Compiled),
                new Regex(@"until[^;]+;", RegexOptions.IgnoreCase | RegexOptions.Compiled),
                new Regex(@"while[^;]+do", RegexOptions.IgnoreCase | RegexOptions.Compiled),
                new Regex(@"for[^;]+do", RegexOptions.IgnoreCase | RegexOptions.Compiled)
            };

        private static readonly List<string> InputMethods =
           new List<string>
            {
                "read",
                "readln"
            };

        private static readonly List<string> OutputMethods =
            new List<string>
            {
                "write",
                "writeln"
            };

        public override IReadOnlyCollection<PascalInputVariable> Parse(PascalMethod code)
        {
            string source = code.NormalizedSource;
            IReadOnlyDictionary<IVariableInfo, int> variables = code.GetVariables();
            List<PascalVariable> inputVariables = ParseInputVariables(source, variables.Keys.Cast<PascalVariable>()).ToList();
            List<PascalVariable> outputVariables = ParseOuputVariables(source, inputVariables).ToList();

            List<PascalInputVariable> result = inputVariables
                .Select(variable => new PascalInputVariable(variable.Name, variable.OriginalSource))
                .ToList();

            List<string> controlArguments = ControlConstructions
                .Select(pattern => pattern.Matches(source))
                .SelectMany(mathes => mathes.Cast<Match>())
                .Select(match => match.Value)
                .ToList();

            foreach (var variable in result)
            {
                int modifiedCount = Regex.Matches(source, CreateModifiedPattern(variable.Name)).Count;
                int contolCount = controlArguments.Sum(args => Regex.Matches(args, CreatePascalIdentifierPattern(variable.Name)).Count);
                int allUsedCount = variables.First(pair => pair.Key.Name == variable.Name).Value;
                variable.IsControl = contolCount > 0;
                variable.IsModified = modifiedCount > 0;
                variable.IsCalculationOrOutput = allUsedCount - contolCount - modifiedCount - 2 > 0;
                variable.IsUsed = allUsedCount != modifiedCount + 2;
            }
            return result;
        }

        private IEnumerable<PascalVariable> ParseInputVariables(string source, IEnumerable<PascalVariable> variables)
        {
            List<string> inputMethods = InputMethods
                .Select(CreateMethodCallPattern)
                .Select(method => Regex.Matches(source, method, RegexOptions.IgnoreCase))
                .SelectMany(matches => matches.Cast<Match>())
                .Select(match => match.Index + match.Length)
                .Select(index => source.Substring(index, FindClosingBracketIndex(source, "(", ")", index) - index))
                .ToList();
            return variables
                .Where(variable => inputMethods.Any(method => Regex.Matches(method, CreatePascalArgumentIdentifierPattern(variable.Name), RegexOptions.IgnoreCase).Count != 0))
                .ToList();
        }

        private IEnumerable<PascalVariable> ParseOuputVariables(string source, IEnumerable<PascalVariable> variables)
        {
            List<string> outputMethods = OutputMethods
                .Select(CreateMethodCallPattern)
                .Select(method => Regex.Matches(source, method, RegexOptions.IgnoreCase))
                .SelectMany(matches => matches.Cast<Match>())
                .Select(match => match.Index + match.Length)
                .Select(index => source.Substring(index, FindClosingBracketIndex(source, "(", ")", index) - index))
                .ToList();
            return variables
                .Where(variable => outputMethods.Any(method => Regex.Matches(method, CreatePascalArgumentIdentifierPattern(variable.Name), RegexOptions.IgnoreCase).Count != 0))
                .ToList();
        }

        private static string CreatePascalArgumentIdentifierPattern(string identifier)
        {
            return string.Format(PascalArgumentIdentifierPattern, identifier);
        }

        private static string CreatePascalIdentifierPattern(string identifier)
        {
            return string.Format(PascalIdentifierPattern, identifier);
        }

        private static string CreateModifiedPattern(string variableName)
        {
            return string.Format(@"{1}{0}:=", EmptyOrWhiteSpacePattern, variableName);
        } 

        private static string CreateMethodCallPattern(string methodName)
        {
            return string.Format(@"{0}\(", methodName);
        }
    }
}
