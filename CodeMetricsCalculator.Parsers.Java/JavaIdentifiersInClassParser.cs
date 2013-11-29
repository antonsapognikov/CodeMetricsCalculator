using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    class JavaIdentifiersInClassParser : JavaCodeParser<JavaClass, IReadOnlyDictionary<JavaIdentifier, int>>
    {
        private const string JavaIdentifierPattern = "[^a-zA-Z0-9_]*" + "{0}" + "[^a-zA-Z0-9_]*"; 

        public override IReadOnlyDictionary<JavaIdentifier, int> Parse(JavaClass code)
        {
            Contract.Requires(code != null);

            var methodSource = code.NormalizedSource;
            var identifiers = new Dictionary<JavaIdentifier, int>();

            //parsing fields as identifiers
            var allIdentifiers = new List<JavaIdentifier>();
            allIdentifiers.AddRange(code.GetFields().Cast<JavaIdentifier>());
            allIdentifiers.AddRange(code.GetMethods().Cast<JavaIdentifier>());
            allIdentifiers.AddRange(GetUsedTypes(code));

            foreach (var identifier in allIdentifiers)
            {
                var regex = new Regex(string.Format(JavaIdentifierPattern, identifier.Name));
                var usageCount = regex.Matches(methodSource).Count;
                identifiers.Add(identifier, usageCount);
            }

            return identifiers;
        }
    }
}
