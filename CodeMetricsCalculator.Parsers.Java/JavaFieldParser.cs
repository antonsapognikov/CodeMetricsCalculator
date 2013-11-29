using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal class JavaFieldParser : JavaCodeParser<JavaClass, IReadOnlyCollection<JavaField>>,
                                     IFieldParser<JavaClass, JavaField>
    {
        private const string FieldRegexString =
            @"([a-zA-Z_][a-zA-Z0-9<,>_]*) +([a-zA-Z_][a-zA-Z0-9_]*)( *= *[a-zA-Z0-9_\+\-\*\\<,>\.\(\)" + "\" ]+)? *;";

        static JavaFieldParser()
        {
            FieldRegex = new Regex(FieldRegexString, RegexOptions.Compiled);
        }

        private static readonly Regex FieldRegex;

        public override IReadOnlyCollection<JavaField> Parse(JavaClass code)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            var source = code.NormalizedSource;
            foreach (var methodInfo in code.GetMethods())
            {
                source = source.Replace(methodInfo.OriginalSource, string.Empty);
            }
            var matches = FieldRegex.Matches(source).Cast<Match>();
            var fields =
                matches.Select(
                    match =>
                        new JavaField(new JavaType(match.Groups[1].Value), match.Groups[2].Value,
                            code, match.Value)).ToList();
            return new ReadOnlyCollection<JavaField>(fields);
        }
    }
}
