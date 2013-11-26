using System;
using System.Collections.Generic;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    internal class PrimaryOperator : JavaOperator
    {
        private static class PatternFor
        {
            public static readonly string MethodInvocation = string.Format("{0} ({1})", Pattern.Identifier, Pattern.Args);
        }

        private static readonly List<PrimaryOperator> AllOperators =
            new List<PrimaryOperator>
            {
                new PrimaryOperator(string.Format("{0} . {1}", Pattern.Operand, Pattern.Identifier), null),
                new PrimaryOperator(PatternFor.MethodInvocation, null),
                new PrimaryOperator(string.Format("new {0} ({1})", Pattern.Identifier, Pattern.Args), "new"),
                new PrimaryOperator(string.Format("{0} [{1}]", Pattern.Operand, Pattern.Args), null),
                new PrimaryOperator(string.Format("instanceof ({0})", Pattern.Operand), "instanceof"),
                new PrimaryOperator("return","return"),
                new PrimaryOperator("continue","continue"),
                new PrimaryOperator("break","break"),
                new PrimaryOperator(string.Format("goto {0}", Pattern.Identifier), "goto")
            };

        public static PrimaryOperator CreateMethodInvocationOperator(string methodName)
        {
            return new PrimaryOperator(PatternFor.MethodInvocation, methodName, methodName);
        }

        private PrimaryOperator(string pattern, string keyword)
            : base(new Pattern(pattern), keyword)
        {
        }

        private PrimaryOperator(string pattern, string source, string keyword)
            : base(new Pattern(pattern), source, keyword)
        {
        }

        public static IReadOnlyCollection<PrimaryOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
    }
}
