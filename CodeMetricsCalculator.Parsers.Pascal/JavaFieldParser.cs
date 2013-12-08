using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal
{
    internal class PascalFieldParser : PascalCodeParser<PascalClass, IReadOnlyCollection<PascalField>>, IFieldParser<PascalClass, PascalField>
    {
        private const string EmptyOrWhiteSpacePattern = @"[ \t]*";
        private const string WhiteSpacePattern = @"[ \t]+";
        private const string TypeIdentifierPattern = @"[a-zA-Z_][a-zA-Z0-9_]+(<[a-zA-Z0-9<,>_ \[\]]+>)?(\[\])?";
        private const string VariableIdentifierPattern = @"[a-zA-Z_][a-zA-Z0-9_]*";
        private const string VariableValuePattern = @"[^;]+";
        private const string BracketArgumentsPattern = @"\([^=]*\)";

        private static readonly string FieldPattern =
            string.Format(@"{0}{2}{1}{3}{0}(={0}{4}{0})?(,{0}{3}{0}(={0}{4}{0})?)*;", EmptyOrWhiteSpacePattern, WhiteSpacePattern, TypeIdentifierPattern, VariableIdentifierPattern, VariableValuePattern);

        private static readonly string FieldTypePattern =
            string.Format(@"^{0}{2}{1}", EmptyOrWhiteSpacePattern, WhiteSpacePattern, TypeIdentifierPattern);

        private static readonly string FieldVariableNamePattern =
            string.Format(@"(^|,){0}{1}", EmptyOrWhiteSpacePattern, VariableIdentifierPattern);

        private static readonly Regex FieldRegex;

        static PascalFieldParser()
        {
            FieldRegex = new Regex(FieldPattern, RegexOptions.Compiled);
        }

        public override IReadOnlyCollection<PascalField> Parse(PascalClass code)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            var source = code.GetMethods().Aggregate(code.NormalizedSource, (current, methodInfo) => current.Replace(methodInfo.OriginalSource, string.Empty));
            return FieldRegex.Matches(source).Cast<Match>()
                .Select(match => match.Value)
                .SelectMany(value => ParseFields(code, value))
                .ToList()
                .AsReadOnly();
        }

        private IEnumerable<PascalField> ParseFields(PascalClass code, string fields)
        {
            string type = Regex.Match(fields, FieldTypePattern).Value.TrimStart(' ').TrimEnd(' ');
            string withoutType = Regex.Replace(fields, FieldTypePattern, string.Empty);
            string withoutBrackets = Regex.Replace(withoutType, BracketArgumentsPattern, string.Empty);
            List<string> names = Regex.Matches(withoutBrackets, FieldVariableNamePattern).Cast<Match>()
                .Select(match => match.Value.TrimStart(',', ' '))
                .ToList();
            return names.Select(value => new PascalField(new PascalType(type), value, code, fields)).ToList();
        }
    }
}
