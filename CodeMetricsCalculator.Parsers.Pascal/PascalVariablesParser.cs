using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal
{
    internal class PascalVariablesParser : PascalCodeParser<PascalMethod, IReadOnlyDictionary<PascalVariable, int>>
    {
        private const string VariablePattern = @"var";
        private const string TypePattern = @"type";
        private const string ConstantPattern = @"const";
        private const string BeginPattern = @"begin";
        private const string PascalIdentifierPattern = "[^a-zA-Z0-9_]" + "{0}" + "[^a-zA-Z0-9_]";

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
            int variablesStartIndex = methodSource.IndexOf(VariablePattern, 0,
                StringComparison.InvariantCultureIgnoreCase);
            int typesStartIndex = methodSource.IndexOf(TypePattern, 0,
                StringComparison.InvariantCultureIgnoreCase);
            int contantsStartIndex = methodSource.IndexOf(ConstantPattern, 0,
                StringComparison.InvariantCultureIgnoreCase);
            int methodBodyStartIndex = methodSource.IndexOf(BeginPattern, 0,
                StringComparison.InvariantCultureIgnoreCase);
            string variables = string.Empty;
            if (variablesStartIndex != -1)
            {
                var possibleEnds = new List<int> { typesStartIndex, contantsStartIndex, methodBodyStartIndex };
                int startIndex = variablesStartIndex + VariablePattern.Length;
                int endIndex = possibleEnds.Where(end => end > variablesStartIndex).Min();
                variables = methodSource.Substring(startIndex, endIndex - startIndex);
            }
            string contants = string.Empty;
            if (contantsStartIndex != -1)
            {
                var possibleEnds = new List<int> { typesStartIndex, variablesStartIndex, methodBodyStartIndex };
                int startIndex = contantsStartIndex + ConstantPattern.Length;
                int endIndex = possibleEnds.Where(end => end > contantsStartIndex).Min();
                contants = methodSource.Substring(startIndex, endIndex - startIndex);
            }
            string allVariables = variables + contants;
            allVariables = allVariables.Replace("\r", string.Empty);
            allVariables = allVariables.Replace("\n", string.Empty);
            allVariables = allVariables.Replace(" ", string.Empty);
            allVariables = Regex.Replace(allVariables, @"=[^;]*;", ";");
            string[] expressions = allVariables.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new Dictionary<string, string>();
            foreach (var expression in expressions)
            {
                string[] values = expression.Split(':');
                if (values.Length == 1)
                {
                    result.Add(values[0], ConstantPattern);
                    continue;
                }
                if (values.Length == 2)
                {
                    result.Add(values[0], values[1]);
                }
            }
            return result.Select(pair => new PascalVariable(new PascalType(pair.Value), pair.Key, allVariables));
        }
    }
}
