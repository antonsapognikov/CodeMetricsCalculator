using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Common.Utils;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    public class JavaCode : MemberInfo
    {
        private const string StringLiteralPattern = "\"[^\"]*\"";
        private const string MultilineCommentPattern = "/\\*[^\\*/]*\\*/";       
        private readonly string _inlineCommentPattern = string.Format("//[^{0}]*{0}", Environment.NewLine);

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
                    return guid.ToString().Quotes();
                });
            //Removing inline comments...
            normalizedSource = Regex.Replace(normalizedSource, _inlineCommentPattern, Environment.NewLine);
            //Removing multiline comments...
            normalizedSource = Regex.Replace(normalizedSource, MultilineCommentPattern, string.Empty);
            return normalizedSource;
        }

        protected string DecodeLiteral(Guid guid)
        {
            return _stringLiterals[guid];
        }
    }
}