using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeMetricsCalculator.Parsers.Java
{
    public static class PatternToRegexExtension
    {
        private const string IdentifierRegex = @"[a-zA-Z0-9\(\)_]+";
        private const string ArgsRegex = @"['a-zA-Z0-9,_\(\)\. \r\n " + "\"]*";
        private const string ParamsRegex = "[\"'a-zA-Z0-9,<>_ \r\n]*";

        public static Regex ToRegex(this Pattern pattern)
        {
            Contract.Requires<ArgumentNullException>(pattern != null, "pattern");

            var patternString = pattern.ToString();
            patternString = Escape(patternString)
                .Replace(" ", @"[ \r\n]*")
                .Replace(Pattern.Args, ArgsRegex)
                .Replace(Pattern.Identifier, IdentifierRegex)
                .Replace(Pattern.Params, ParamsRegex)
                .Replace(@"\(\.\.\.\)", @"\([^\)]*\)")
                .Replace(@"\[\.\.\.\]", @"\[[^\]]*\]")
                .Replace(@"{\.\.\.}", @"{[^}]*}");
            if (!pattern.BracesRequires)
                patternString = patternString.Replace("{", "{?").Replace("}", "}?");
            return new Regex(patternString, RegexOptions.Compiled);
        }



        private static bool IsMetachar(char ch)
        {
            return Metachars.Contains(ch);
        }

        private static readonly IReadOnlyCollection<char> Metachars = new ReadOnlyCollection<char>(
            new List<char>
            {
                '\t',
                '\r',
                '\n',
                '+',
                '*',
                '|',
                '(',
                ')',
                '[',
                ']',
                '.'
            });

        private static string Escape(string input)
        {
            for (int count = 0; count < input.Length; ++count)
            {
                if (IsMetachar(input[count]))
                {
                    var stringBuilder = new StringBuilder();
                    char ch = input[count];
                    stringBuilder.Append(input, 0, count);
                    do
                    {
                        stringBuilder.Append('\\');
                        switch (ch)
                        {
                            case '\t':
                                ch = 't';
                                break;
                            case '\r':
                                ch = 'r';
                                break;
                            case '\n':
                                ch = 'n';
                                break;
                        }
                        stringBuilder.Append(ch);
                        ++count;
                        int startIndex = count;
                        for (; count < input.Length; ++count)
                        {
                            ch = input[count];
                            if (IsMetachar(ch))
                                break;
                        }
                        stringBuilder.Append(input, startIndex, count - startIndex);
                    }
                    while (count < input.Length);
                    return (stringBuilder).ToString();
                }
            }
            return input;
        }
    }
}
