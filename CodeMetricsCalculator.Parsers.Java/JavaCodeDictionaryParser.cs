using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal class JavaCodeDictionaryParser : JavaCodeParser<JavaMethod, CodeDictionary>, ICodeDictionaryParser<JavaMethod>
    {
        private const string IdentifierRegex = @"[a-zA-Z_][a-zA-Z0-9_]*";
        private const string StringLiteralPattern = "\"[^\"]*\"";
        private const string NumberPatter = @"[-+]?([0-9]*\.[0-9]+|[0-9]+)";
        private const string JavaIdentifierPattern = "[^a-zA-Z0-9_]" + "({0})" + "[^a-zA-Z0-9_]";
        private const string JavaOperatorPattern = "[^!+-=/&|%]*" + "({0})" + "[^!+-=/&|%]*"; 
        private static readonly string MethodCallStartRegex = string.Format(@"{0}\(", IdentifierRegex);

        public override CodeDictionary Parse(JavaMethod javaMethod)
        {
            Contract.Requires(javaMethod != null);
            
            var operatorsDictionary = ParseOperators(javaMethod);
            var operandsDictionary = ParseOperands(javaMethod);
            var codeDictionary = new CodeDictionary(operatorsDictionary, operandsDictionary);
            return codeDictionary;
        }

        private IReadOnlyDictionary<string, int> ParseOperands(JavaMethod javaMethod)
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
                var fieldAsVariableRegex = new Regex(string.Format(JavaIdentifierPattern, fieldInfo.Name));
                var count = fieldAsVariableRegex.Matches(source).Count;
                if (count > 0 && !operands.ContainsKey(fieldInfo.Name))
                    operands.Add(fieldInfo.Name, count);
            }

            var literalRegex = new Regex(StringLiteralPattern);
            var literals = literalRegex.Matches(source).Cast<Match>().Select(match => match.Value);
            foreach (var literal in literals)
            {
                source = literalRegex.Replace(source, " ");
                if (!operands.ContainsKey(literal))
                    operands.Add(literal, 1);
                else
                    operands[literal] += 1;
            }

            var numberRegex = new Regex("[+-=/&|% ](" + NumberPatter + ")[+-=/&|%; ]");
            var numbers = numberRegex.Matches(source).Cast<Match>().Select(match => match.Groups[1].Value);
            foreach (var number in numbers)
            {
                if (!operands.ContainsKey(number))
                    operands.Add(number, 1);
                else
                    operands[number] += 1;
            }
            return operands;
        }

        private IReadOnlyDictionary<string, int> ParseOperators(JavaMethod javaMethod)
        {
            var javaMethodSource = javaMethod.GetBody().NormalizedSource;
            var operatorsDictionary = new Dictionary<string, int>();


            var parsedOperators = new List<string>();
            parsedOperators.AddRange(ParseMethodInvocationOperator(javaMethodSource, out javaMethodSource));
            parsedOperators.AddRange(ParseTernaryOperator(javaMethodSource, out javaMethodSource));
            parsedOperators.AddRange(ParseMemberAccessOperator(javaMethodSource, out javaMethodSource));
            parsedOperators.AddRange(ParseIndexerOperator(javaMethodSource, out javaMethodSource));
            //Не знаю надо ли их приводить к форме (), так как сейчас сюда идет вся скобка
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
            var keywordBasedOperators = JavaOperator.Operators.Where(@operator => @operator.IsKeywordBase).ToList();
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
            var keywordRegex = new Regex(string.Format(JavaOperatorPattern, Regex.Escape(keyword)));
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
            var matches = Regex.Matches(source, MethodCallStartRegex).Cast<Match>().ToList();
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
            var matches = Regex.Matches(source, pattern).Cast<Match>().ToList();
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

        private IEnumerable<string> ParseTernaryOperator(string source, out string modifiedSource)
        {
            modifiedSource = source;
            var pattern = @"\?";
            var matches = Regex.Matches(source, pattern).Cast<Match>().ToList();
            return matches
                .Select(match => match.Index)
                .Select(index => "...?...:..."/*source.Substring(index, FindClosingBracketIndex(source, "?", ":", index) - index + 1)*/)
                .ToList();
        }

        private IEnumerable<string> ParseBracketOperator(string source, out string modifiedSource)
        {
            modifiedSource = source;
            var pattern = @"\(|{";
            var matches = Regex.Matches(source, pattern).Cast<Match>().ToList();
            return matches
                .Select(match => match.Value == "(" ? "(...)" : "{...}")
                //.Select(index => "(...)"/*source.Substring(index, FindClosingBracketIndex(source, "(", ")", index) - index + 1)*/)
                .ToList();
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
