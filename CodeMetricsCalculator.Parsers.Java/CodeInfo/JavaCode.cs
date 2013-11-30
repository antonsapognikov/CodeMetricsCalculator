using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Common.Utils;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    public class JavaCode : MemberInfo
    {
        private const string StringLiteralPattern = "\"[^\"]*\""; 
        private const string MultilineCommentPattern = "/\\*([^\\*]|(\\*+[^\\*/]))*\\*+/";      
        private readonly string _inlineCommentPattern = string.Format("//[^{0}]*{0}", Environment.NewLine);
        private const string SpacesAndTabsPattern = @"[ \t]+";
        private readonly Dictionary<Guid, string> _stringLiterals = new Dictionary<Guid, string>(); 

        public JavaCode(string originalSource)
            : base(originalSource)
        {
        }

        protected override string NormalizeSource(string originalSource)
        {
            //Replacing string literals...
            var normalizedSource = Regex.Replace(originalSource, StringLiteralPattern, match =>
                {
                    if (_stringLiterals.ContainsValue(match.Value))
                    {
                        return _stringLiterals.GetKey(match.Value).ToString().Quotes();
                    }
                    var guid = Guid.NewGuid();
                    _stringLiterals.Add(guid, match.Value);
                    return GuidEncoder.Encode(guid).ToString(CultureInfo.InvariantCulture).Quotes();
                });
            //Removing excess spaces and tabs
            normalizedSource = Regex.Replace(normalizedSource, SpacesAndTabsPattern, " ");
            //Removing inline comments...
            normalizedSource = Regex.Replace(normalizedSource, _inlineCommentPattern, Environment.NewLine);
            //Removing multiline comments...
            normalizedSource = Regex.Replace(normalizedSource, MultilineCommentPattern, string.Empty);
            //Removing empty lines and trim
            var lines = normalizedSource
                .Split(new [] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim(' ', '\t'))
                .Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            normalizedSource = lines.Count > 0
                ? lines.Aggregate((s1, s2) => " " + s1 + Environment.NewLine + " " + s2 + " ")
                : string.Empty;
                
            return normalizedSource;
        }
        
        protected string DecodeLiteral(string encodedGuid)
        {
            return _stringLiterals[GuidEncoder.Decode(encodedGuid)];
        }
    }
}