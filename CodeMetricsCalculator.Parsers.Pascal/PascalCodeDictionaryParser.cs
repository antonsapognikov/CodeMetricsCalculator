using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal
{
    internal class PascalCodeDictionaryParser : PascalCodeParser<PascalMethod, CodeDictionary>, ICodeDictionaryParser<PascalMethod>
    {
        private const string IdentifierRegex = @"[a-z_][a-z0-9_]*";
        private static readonly string StringLiteralPattern = string.Format(@"'[^'{0}]*'", Environment.NewLine);
        private const string NumberPatter = @"[-+]?([0-9]+(x|\.)?[0-9]*)";
        private const string PascalIdentifierPattern = "[^a-z0-9_]" + "({0})" + "[^a-z0-9_]";
        private const string PascalOperatorPattern = "[^!+-=/&|%]*" + "({0})" + "[^!+-=/&|%]*"; 
        private static readonly string MethodCallStartRegex = string.Format(@"{0}\(", IdentifierRegex);

        private static readonly List<string> ReservedOperands =
            new List<string>
            {
                "nil",
                "true",
                "false",
                "self"
            };

        public override CodeDictionary Parse(PascalMethod javaMethod)
        {
            Contract.Requires(javaMethod != null);
            
            var operatorsDictionary = ParseOperators(javaMethod);
            var operandsDictionary = ParseOperands(javaMethod);
            var codeDictionary = new CodeDictionary(operatorsDictionary, operandsDictionary);
            return codeDictionary;
        }

        private IReadOnlyDictionary<string, int> ParseOperands(PascalMethod javaMethod)
        {
            var source = javaMethod.GetBody().NormalizedSource;
            var operands = new Dictionary<string, int>();
            var variables = javaMethod.GetVariables();
            foreach (var keyValuePair in variables)
            {
                source = source.Replace(keyValuePair.Key.Type.Name, " ");
                operands.Add(keyValuePair.Key.Name, keyValuePair.Value);
            }

            var fields = javaMethod.Class.GetFields();
            foreach (var fieldInfo in fields)
            {
                var fieldAsVariableRegex = new Regex(string.Format(PascalIdentifierPattern, fieldInfo.Name), RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var count = fieldAsVariableRegex.Matches(source).Count;
                if (count > 0 && !operands.ContainsKey(fieldInfo.Name))
                    operands.Add(fieldInfo.Name, count);
            }

            var literalRegex = new Regex(StringLiteralPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var literals = literalRegex.Matches(source).Cast<Match>().Select(match => match.Value);
            foreach (var literal in literals)
            {
                source = literalRegex.Replace(source, " ");
                if (!operands.ContainsKey(literal))
                    operands.Add(literal, 1);
                else
                    operands[literal] += 1;
            }

            var numberRegex = new Regex("[+-=/&|% ](" + NumberPatter + ")[+-=/&|%; ]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var numbers = numberRegex.Matches(source).Cast<Match>().Select(match => match.Groups[1].Value);
            foreach (var number in numbers)
            {
                if (!operands.ContainsKey(number))
                    operands.Add(number, 1);
                else
                    operands[number] += 1;
            }
            foreach (var operand in ReservedOperands)
            {
                var pattern = string.Format(PascalIdentifierPattern, operand);
                var count = Regex.Matches(source, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase).Count;
                if (count == 0)
                    continue;
                if (!operands.ContainsKey(operand))
                    operands.Add(operand, count);
                else
                    operands[operand] += count;
            }


            return operands;
        }

        private IReadOnlyDictionary<string, int> ParseOperators(PascalMethod javaMethod)
        {
            var javaMethodSource = javaMethod.GetBody().NormalizedSource;
            var operatorsDictionary = new Dictionary<string, int>();


            var parsedOperators = new List<string>();
            parsedOperators.AddRange(ParseMethodInvocationOperator(javaMethodSource, out javaMethodSource));
            parsedOperators.AddRange(ParseMemberAccessOperator(javaMethodSource, out javaMethodSource));
            parsedOperators.AddRange(ParseIndexerOperator(javaMethodSource, out javaMethodSource));
            parsedOperators.AddRange(ParseBracketOperator(javaMethodSource, out javaMethodSource));
            var variables = javaMethod.GetVariables();
            parsedOperators.AddRange(variables.Select(pair => pair.Key.NormalizedSource));

            foreach (var parsedOperator in parsedOperators)
            {
                AddToDictionary(parsedOperator, 1, operatorsDictionary);
            }
            
            foreach (var keyValuePair in variables)
            {
                var typeName = keyValuePair.Key.Type.Name;
                javaMethodSource = javaMethodSource.Replace(typeName, " ");
            }
            var keywordBasedOperators = PascalOperator.Operators.Where(@operator => @operator.IsKeywordBase).ToList();
            foreach (var keywordBasedOperator in keywordBasedOperators)
            {
                var keyword = keywordBasedOperator.Keyword;
                var count = ParseKeywordBasedOperator(javaMethodSource, keyword, out javaMethodSource);
                AddToDictionary(keywordBasedOperator.Name, count, operatorsDictionary);
            }
            return operatorsDictionary;
        }

        private static int ParseKeywordBasedOperator(string source, string keyword, out string modifiedSource)
        {
            var keywordRegex = new Regex(string.Format(PascalOperatorPattern, Regex.Escape(keyword)));
            var matches = keywordRegex.Matches(source);
            int count = matches.Count;
            modifiedSource = source;
            foreach (var match in matches.Cast<Match>())
            {
                modifiedSource = modifiedSource.Remove(match.Groups[1].Index, match.Groups[1].Length);
                modifiedSource = modifiedSource.Insert(match.Groups[1].Index, new string('~', match.Groups[1].Value.Length));
            }
            modifiedSource = modifiedSource.Replace("~", string.Empty);
            return count;
        }

        private IEnumerable<string> ParseMethodInvocationOperator(string source, out string modifiedSource)
        {
            var matches = Regex.Matches(source, MethodCallStartRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase).Cast<Match>().ToList();
            var startIndexes = matches
                .Select(match => new { CallIndex = match.Index, BrackerIndex = match.Index + match.Value.Length - 1 })
                .ToList();
            var operators = startIndexes.Select(item => source.Substring(item.CallIndex, item.BrackerIndex - item.CallIndex) + "(...)"/*source.Substring(item.CallIndex,
                                FindClosingBracketIndex(source, "(", ")", item.BrackerIndex) - item.CallIndex + 1)*/).ToList();
            modifiedSource = source; //todo remove invocations from source
            return operators;
        }

        private IEnumerable<string> ParseIndexerOperator(string source, out string modifiedSource)
        {
            modifiedSource = source;
            var pattern = @"\[[^\]]";
            var matches = Regex.Matches(source, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase).Cast<Match>().ToList();
            return matches
                .Select(match => match.Index)
                .Select(index => source.Substring(index, FindClosingBracketIndex(source, "[", "]", index) - index + 1))
                .ToList();
        }

        private static IEnumerable<string> ParseMemberAccessOperator(string source, out string modifiedSource)
        {
            modifiedSource = source;
            return Enumerable.Empty<string>();
        }


        private IEnumerable<string> ParseBracketOperator(string source, out string modifiedSource)
        {
            modifiedSource = source;
            var pattern = @"begin";
            var matches = Regex.Matches(source, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase).Cast<Match>().ToList();
            return matches.Select(match => "begin...end;").ToList();
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
