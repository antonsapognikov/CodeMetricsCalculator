using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.Operators;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal class JavaCodeDictionaryParser : JavaCodeParser<JavaMethod, CodeDictionary>, ICodeDictionaryParser<JavaMethod>
    {
        private const string IdentifierRegex = @"[a-zA-Z_][a-zA-Z0-9_]*";
        private static readonly string MethodCallStartRegex = string.Format(@"{0}\(", IdentifierRegex);

        public override CodeDictionary Parse(JavaMethod javaMethod)
        {
            Contract.Requires(javaMethod != null);
            
            var source = javaMethod.GetBody().NormalizedSource;
            var operators = JavaOperator.Operators;
            var operatorsDictionary = ParseOperators(source, operators, out source);
            var codeDictionary = new CodeDictionary(operatorsDictionary, new Dictionary<string, int>());
            return codeDictionary;
        }

        private IReadOnlyDictionary<string, int> ParseOperators(string javaMethodSource, IReadOnlyCollection<JavaOperator> operators, out string modifiedSource)
        {
            var operatorsDictionary = new Dictionary<string, int>();

            var notKeywordBasedOperators = operators.Where(@operator => !@operator.IsKeywordBase).ToList();
            foreach (var notKeywordBasedOperator in notKeywordBasedOperators)
            {
                var parsedOperators = new List<string>();
                if (notKeywordBasedOperator == PrimaryOperator.VariableDeclaring)
                    parsedOperators.AddRange(ParseVariableDeclaringOperator(javaMethodSource, out javaMethodSource));
                if (notKeywordBasedOperator == PrimaryOperator.VariableDeclaringWithAssignment)
                    parsedOperators.AddRange(ParseVariableDeclaringWithAssignmentOperator(javaMethodSource, out javaMethodSource));
                if (notKeywordBasedOperator == PrimaryOperator.MaltyVariableDeclaring)
                    parsedOperators.AddRange(ParseMaltyVariableDeclaringOperator(javaMethodSource, out javaMethodSource));
                if (notKeywordBasedOperator == PrimaryOperator.MethodInvocation)
                    parsedOperators.AddRange(ParseMethodInvocationOperator(javaMethodSource, out javaMethodSource));
                if (notKeywordBasedOperator == PrimaryOperator.Indexer)
                    parsedOperators.AddRange(ParseIndexerOperator(javaMethodSource, out javaMethodSource));
                if (notKeywordBasedOperator == PrimaryOperator.MemberAccess)
                    parsedOperators.AddRange(ParseMemberAccessOperator(javaMethodSource, out javaMethodSource));
                if (notKeywordBasedOperator is ConditionalOperator &&
                    (notKeywordBasedOperator as ConditionalOperator).OperationType == OperationType.Ternary)
                    parsedOperators.AddRange(ParseTernaryOperator(javaMethodSource, out javaMethodSource));
                foreach (var parsedOperator in parsedOperators)
                {
                    AddToDictionary(parsedOperator, 1, operatorsDictionary);
                }
            }

            var keywordBasedOperators = operators.Where(@operator => @operator.IsKeywordBase).ToList();
            foreach (var keywordBasedOperator in keywordBasedOperators)
            {
                var keyword = keywordBasedOperator.Keyword;
                var count = ParseKeywordBasedOperator(javaMethodSource, keyword, out javaMethodSource);
                AddToDictionary(keywordBasedOperator.Name, count, operatorsDictionary);
            }

            modifiedSource = javaMethodSource;
            return operatorsDictionary;
        }

        private static int ParseKeywordBasedOperator(string source, string keyword, out string modifiedSource)
        {
            var keywordIndex = 0;
            var count = 0;

            while ((keywordIndex = source.IndexOf(keyword, StringComparison.Ordinal)) != -1)
            {
                count++;
                source = source.Remove(keywordIndex, keyword.Length).Insert(keywordIndex, " ");
            }
            modifiedSource = source;
            return count;
        }

        private IEnumerable<string> ParseMethodInvocationOperator(string source, out string modifiedSource)
        {
            var matches = Regex.Matches(source, MethodCallStartRegex).Cast<Match>().ToList();
            var startIndexes = matches
            .Select(match => new { CallIndex = match.Index, BrackerIndex = match.Index + match.Value.Length - 1 })
            .ToList();
            var operators =
                startIndexes.Select(
                    item =>
                        source.Substring(item.CallIndex,
                            FindClosingBracketIndex(source, "(", ")", item.BrackerIndex) - item.CallIndex + 1)).ToList();
            modifiedSource = source; //todo remove invocations from source
            return operators;
        }

        private static IEnumerable<string> ParseIndexerOperator(string source, out string modifiedSource)
        {
            modifiedSource = source;
            return Enumerable.Empty<string>();
        }

        private static IEnumerable<string> ParseMemberAccessOperator(string source, out string modifiedSource)
        {
            modifiedSource = source;
            return Enumerable.Empty<string>();
        }

        private static IEnumerable<string> ParseTernaryOperator(string source, out string modifiedSource)
        {
            modifiedSource = source;
            return Enumerable.Empty<string>();
        }

        private static IEnumerable<string> ParseVariableDeclaringOperator(string source, out string modifiedSource)
        {
            modifiedSource = source;
            return Enumerable.Empty<string>();
        }

        private static IEnumerable<string> ParseVariableDeclaringWithAssignmentOperator(string source, out string modifiedSource)
        {
            modifiedSource = source;
            return Enumerable.Empty<string>();
        }

        private static IEnumerable<string> ParseMaltyVariableDeclaringOperator(string source, out string modifiedSource)
        {
            modifiedSource = source;
            return Enumerable.Empty<string>();
        }


        private static void AddToDictionary(string operatorName, int count, IDictionary<string, int> operators)
        {
            if (!operators.ContainsKey(operatorName))
            {
                if (count > 0)
                    operators.Add(operatorName, count);
            }
            else
            {
                operators[operatorName] += count;
            }
        }

    }
}
