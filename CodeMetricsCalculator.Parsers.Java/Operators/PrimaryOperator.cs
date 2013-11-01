using System;
using System.Collections.Generic;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    internal class PrimaryOperator : JavaOperator
    {
        private static readonly List<PrimaryOperator> AllOperators =
            new List<PrimaryOperator>
            {
                new PrimaryOperator(string.Format("{0} . {0}", Pattern.Identifier)),
                new PrimaryOperator(string.Format("{0} ({1})", Pattern.Identifier, Pattern.Args)),
                new PrimaryOperator(string.Format("new {0} ({1})", Pattern.Identifier, Pattern.Args)),
                new PrimaryOperator(string.Format("new {0} [{1}]", Pattern.Identifier, Pattern.Args)),
                new PrimaryOperator(string.Format("instanceof ({0})", Pattern.Identifier)),
                new PrimaryOperator("return"),
                new PrimaryOperator("continue"),
                new PrimaryOperator("break"),
                new PrimaryOperator(string.Format("goto {0}", Pattern.Identifier))
            };

        public PrimaryOperator(string pattern)
            : base(new Pattern(pattern))
        {
        }

        public static IReadOnlyCollection<PrimaryOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
    }
}
