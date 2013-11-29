using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal class JavaIdentifiersInMethodParser : JavaCodeParser<JavaMethod, IReadOnlyDictionary<JavaIdentifier, int>>
    {
        private const string JavaIdentifierPattern = "[^a-zA-Z0-9_]" + "{0}" + "[^a-zA-Z0-9_]"; 
        private const string VariableDeclaringPattern =
            @"[a-zA-Z_][a-zA-Z0-9<,>_]* +([a-zA-Z_][a-zA-Z0-9_]*)( *= *[a-zA-Z0-9_\+\-\*\\<,>\.\(\)" + "\" ]+)? *;";

        static JavaIdentifiersInMethodParser()
        {
            VariableDeclaringRegex = new Regex(VariableDeclaringPattern, RegexOptions.Compiled);
        }

        public override IReadOnlyDictionary<JavaIdentifier, int> Parse(JavaMethod code)
        {
            Contract.Requires(code != null);

            var methodSource = code.GetBody().NormalizedSource;
            var identifiers = new Dictionary<JavaIdentifier, int>();

            //parsing method parameters as identifiers
            var parameters = code.Parameters.Cast<JavaMethodParameter>();
            foreach (var methodParameterInfo in parameters)
            {
                var regex = new Regex(string.Format(JavaIdentifierPattern, methodParameterInfo.Name));
                var usageCount = regex.Matches(methodSource).Count + 1; //+declaring
                identifiers.Add(methodParameterInfo, usageCount);
            }

            //parse declared variables
            var variables = ParseVariables(methodSource);
            foreach (var javaVariable in variables)
            {
                var regex = new Regex(string.Format(JavaIdentifierPattern, javaVariable.Name));
                var usageCount = regex.Matches(methodSource).Count;
                identifiers.Add(javaVariable, usageCount);
            }

            return identifiers;
        }
        private static readonly Regex VariableDeclaringRegex;

        private IReadOnlyCollection<JavaVariable> ParseVariables(string methodSource)
        {
            var variables = new List<JavaVariable>();
            var matches = VariableDeclaringRegex.Matches(methodSource).Cast<Match>();
            foreach (var match in matches)
            {
                var variableDeclaring = match.Value;
                if (variableDeclaring.StartsWith("return") || variableDeclaring.StartsWith("goto"))
                    continue;
                var groups = match.Groups;
                variables.Add(new JavaVariable(match.Groups[1].Value, match.Value));
            }
            return variables.AsReadOnly();
        } 
    }
}
