using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    class JavaInputVariableParser : JavaCodeParser<JavaMethod, IReadOnlyCollection<JavaInputVariable>>
    {
        private const string ScannerType = "Scanner";
        private const string ReaderType = "Reader";
        private const string BufferedReaderType = "BufferedReader";
        private const string EmptyOrWhiteSpacePattern = @"[ \t]*";
        private const string VariableIdentifierPattern = @"[a-zA-Z_][a-zA-Z0-9_]*";
        private const string VariableValuePattern = @"[^;]+";
        private const string MethodArgumentPattern = @"[^;]*";
        private const string JavaIdentifierPattern = "[^a-zA-Z0-9_]" + "{0}" + "[^a-zA-Z0-9_]";

        private static readonly string MethodResultVariableNamePattern = string.Format(@"^{0}", VariableIdentifierPattern);

        private static readonly List<string> ScannerInputMethods =
            new List<string>
            {
                "next",
                "nextBigDecimal",
                "nextBigInteger",
                "nextBoolean",
                "nextByte",
                "nextDouble",
                "nextFloat",
                "nextInt",
                "nextLine",
                "nextLong",
                "nextShort"
            };

        private static readonly List<string> ReaderMethods =
            new List<string>
            {
                "read",
                "readLine"
            };

        private static readonly List<string> ContolConstructions =
            new List<string>
            {
                "if",
                "switch",
                "for",
                "while"
            };

        private static readonly List<string> OutputMethods =
            new List<string>
            {
                "println"
            };

        public override IReadOnlyCollection<JavaInputVariable> Parse(JavaMethod code)
        {
            string source = code.NormalizedSource;
            IReadOnlyDictionary<IVariableInfo, int> variables = code.GetVariables();
            IEnumerable<JavaVariable> scannerResult = ParseMethodResultInputVariables(source, variables, ScannerType, ScannerInputMethods);
            IEnumerable<JavaVariable> readerResult = ParseMethodResultInputVariables(source, variables, ReaderType, ReaderMethods);
            IEnumerable<JavaVariable> bufferedReaderResult = ParseMethodResultInputVariables(source, variables, BufferedReaderType, ReaderMethods);
            List<JavaInputVariable> result = scannerResult
                .Concat(readerResult)
                .Concat(bufferedReaderResult)
                .Select(variable => new JavaInputVariable(variable.Name, variable.OriginalSource))
                .ToList();

            List<string> outputArguments = OutputMethods
                .Select(CreateMethodCallPattern)
                .Select(pattern => Regex.Matches(source, pattern))
                .SelectMany(mathes => mathes.Cast<Match>())
                .Select(match => match.Index + match.Value.Length)
                .Select(index => source.Substring(index, FindClosingBracketIndex(source, "(", ")", index) - index + 1))
                .ToList();

            List<string> controlArguments = ContolConstructions
                .Select(CreateControlConstructionPattern)
                .Select(pattern => Regex.Matches(source, pattern))
                .SelectMany(mathes => mathes.Cast<Match>())
                .Select(match => match.Index + match.Value.Length)
                .Select(index => source.Substring(index, FindClosingBracketIndex(source, "(", ")", index) - index + 1))
                .ToList();

            foreach (var variable in result)
            {
                int modifiedCount = Regex.Matches(source, CreateModifiedPattern(variable.Name)).Count;
                int outputCount = outputArguments.Sum(args => Regex.Matches(args, CreateJavaIdentifierPattern(variable.Name)).Count);
                int contolCount = controlArguments.Sum(args => Regex.Matches(args, CreateJavaIdentifierPattern(variable.Name)).Count);
                int allUsedCount = variables.First(pair => pair.Key.Name == variable.Name).Value;
                int declarationCount = IsAssignedInDeclaration(variable) ? 1 : 2;
                variable.IsControl = contolCount > 0;
                variable.IsModified = modifiedCount > 1;
                variable.IsCalculationOrOutput = allUsedCount - contolCount - modifiedCount - declarationCount + 1 > 0;
                variable.IsUsed = allUsedCount != modifiedCount + declarationCount - 1;
            }
            return result;
        }

        private bool IsAssignedInDeclaration(JavaInputVariable variable)
        {
            if (variable == null)
                throw new ArgumentNullException("variable");
            return Regex.IsMatch(variable.OriginalSource, CreateModifiedPattern(variable.Name));
        }

        private IEnumerable<JavaVariable> ParseMethodResultInputVariables(string source, IReadOnlyDictionary<IVariableInfo, int> variables, string readerType, List<string> methods)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (variables == null)
                throw new ArgumentNullException("variables");
            if (readerType == null)
                throw new ArgumentNullException("readerType");
            if (methods == null)
                throw new ArgumentNullException("methods");

            if (variables.All(pair => pair.Key.Type.Name != readerType))
                return Enumerable.Empty<JavaVariable>();
            var reader = (JavaVariable) variables.First(pair => pair.Key.Type.Name == readerType).Key;
            List<string> multilineResults = methods
                .Select(methodName => CreateMultilineInputPattern(reader.Name, methodName))
                .Select(pattern => Regex.Matches(source, pattern))
                .SelectMany(matches => matches.Cast<Match>())
                .Select(match => match.Value)
                .ToList();
            List<string> inlineResults = methods
                .Select(CreateInlineInputPattern)
                .Select(pattern => Regex.Matches(source, pattern))
                .SelectMany(matches => matches.Cast<Match>())
                .Select(match => match.Value)
                .ToList();
            return multilineResults
                .Concat(inlineResults)
                .Select(value => Regex.Match(value, MethodResultVariableNamePattern).Value)
                .Select(name => (JavaVariable) variables.First(pair => pair.Key.Name == name).Key)
                .ToList();
        }

        private static string CreateJavaIdentifierPattern(string identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException("identifier");
            return string.Format(JavaIdentifierPattern, identifier);
        }

        private static string CreateModifiedPattern(string variableName)
        {
            if (variableName == null)
                throw new ArgumentNullException("variableName");
            return string.Format(@"{1}{0}=", EmptyOrWhiteSpacePattern, variableName);
        } 

        private static string CreateMethodCallPattern(string methodName)
        {
            if (methodName == null)
                throw new ArgumentNullException("methodName");
            return string.Format(@"{0}\(", methodName);
        }

        private static string CreateControlConstructionPattern(string controlConstruction)
        {
            if (controlConstruction == null)
                throw new ArgumentNullException("controlConstruction");
            return string.Format(@"{1}{0}\(", EmptyOrWhiteSpacePattern, controlConstruction);
        }

        private static string CreateMultilineInputPattern(string readerName, string methodName)
        {
            if (readerName == null)
                throw new ArgumentNullException("readerName");
            if (methodName == null)
                throw new ArgumentNullException("methodName");
            return string.Format(@"{1}{0}={0}{2}\.{3}\(", EmptyOrWhiteSpacePattern, VariableIdentifierPattern, readerName, methodName);
        }

        private static string CreateInlineInputPattern(string methodName)
        {
            if (methodName == null)
                throw new ArgumentNullException("methodName");
            return string.Format(@"{1}{0}={0}{2}\)\.{4}\({3}\);", EmptyOrWhiteSpacePattern, VariableIdentifierPattern, VariableValuePattern, MethodArgumentPattern, methodName);
        }
    }
}
