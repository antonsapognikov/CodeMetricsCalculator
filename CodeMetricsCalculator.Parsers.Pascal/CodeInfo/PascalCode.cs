using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Common.Utils;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.CodeInfo
{
    public class PascalCode : MemberInfo
    {
        private const string SpacesAndTabsPattern = @"[ \t]+";
        private const string BaseMultilineCommentPattern = @"{[^}]*}";
        private const string AdditionalMultilineCommentPattern = @"\(\*[^(\*\))]*\*\)";
        
        private static readonly string StringLiteralPattern = string.Format(@"'[^'{0}]*'", Environment.NewLine);
        private static readonly string InlineCommentPattern = string.Format(@"//[^{0}]*{0}", Environment.NewLine);

        private readonly Dictionary<string, string> _stringLiterals = new Dictionary<string, string>(); 

        public PascalCode(string originalSource)
            : base(originalSource)
        {
        }

        protected override string NormalizeSource(string originalSource)
        {
            string normalizedSource = originalSource;
            ReplaceStringLiterals(normalizedSource, out normalizedSource);
            ReplaceSpacesAndTabs(normalizedSource, out normalizedSource);
            ReplaceInlineComments(normalizedSource, out normalizedSource);
            ReplaceMultilineComments(normalizedSource, out normalizedSource);
            ReplaceInvalidLines(normalizedSource, out normalizedSource);                         
            return normalizedSource;
        }

        private void ReplaceStringLiterals(string input, out string output)
        {
            _stringLiterals.Clear();
            output = Regex.Replace(input, StringLiteralPattern, match =>
            {
                if (_stringLiterals.ContainsValue(match.Value))
                {
                    return _stringLiterals.GetKey(match.Value).ToString().Quotes();
                }
                var guid = GuidEncoder.Encode(Guid.NewGuid());
                _stringLiterals.Add(guid, match.Value);
                return guid.ToString().Quotes();
            });
        }

        private void ReplaceSpacesAndTabs(string input, out string output)
        {
            output = Regex.Replace(input, SpacesAndTabsPattern, " ");
        }

        private void ReplaceInlineComments(string input, out string output)
        {
            output = Regex.Replace(input, InlineCommentPattern, Environment.NewLine);
        }

        private void ReplaceMultilineComments(string input, out string output)
        {
            output = Regex.Replace(input, BaseMultilineCommentPattern, string.Empty);
            output = Regex.Replace(output, AdditionalMultilineCommentPattern, string.Empty);
        }

        private void ReplaceInvalidLines(string intput, out string output)
        {
            var lines = intput
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim(' ', '\t'))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
            output = lines.Count > 0
                ? lines.Aggregate((s1, s2) => " " + s1 + Environment.NewLine + " " + s2 + " ")
                : string.Empty;
        }
    }
}