using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal
{
    class PascalIdentifiersInClassParser : PascalCodeParser<PascalClass, IReadOnlyDictionary<PascalIdentifier, int>>
    {
        private const string PascalIdentifierPattern = "[^a-zA-Z0-9_]" + "{0}" + "[^a-zA-Z0-9_]"; 

        public override IReadOnlyDictionary<PascalIdentifier, int> Parse(PascalClass code)
        {
            Contract.Requires(code != null);

            var methodSource = code.NormalizedSource;
            var identifiers = new Dictionary<PascalIdentifier, int>();

            //parsing fields as identifiers
            var allIdentifiers = new List<PascalIdentifier>();
            allIdentifiers.AddRange(code.GetFields().Cast<PascalIdentifier>());
            allIdentifiers.AddRange(code.GetMethods().Cast<PascalIdentifier>());
            //allIdentifiers.AddRange(GetUsedTypes(code));

            foreach (var identifier in allIdentifiers)
            {
                var regex = new Regex(string.Format(PascalIdentifierPattern, identifier.Name));
                var usageCount = regex.Matches(methodSource).Count;
                identifiers.Add(identifier, usageCount);
            }

            return identifiers;
        }
    }
}
