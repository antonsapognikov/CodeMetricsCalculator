using System;
using System.Collections.Generic;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.Operators
{
    internal class PrimaryOperator : PascalOperator
    {
        private static class PatternFor
        {
            public static readonly string MethodInvocation = string.Format("{0} ({1})", Pattern.Identifier, Pattern.Args);
        }

        public static readonly PrimaryOperator MethodInvocation = new PrimaryOperator(PatternFor.MethodInvocation, null);

        public static readonly PrimaryOperator MemberAccess =
            new PrimaryOperator(string.Format("{0} . {1}", Pattern.Operand, Pattern.Identifier), null);
        
        public static readonly PrimaryOperator Indexer =
            new PrimaryOperator(string.Format("{0} [{1}]", Pattern.Operand, Pattern.Args), null);

        public static readonly PrimaryOperator VariableDeclaring =
            new PrimaryOperator(string.Format("{0}: {1};", Pattern.Operand, Pattern.Identifier), null);

        public static readonly PrimaryOperator VariableDeclaringWithAssignment =
            new PrimaryOperator(string.Format("{0}: {1} = {0};", Pattern.Operand, Pattern.Identifier), null);

        public static readonly PrimaryOperator MaltyVariableDeclaring =
            new PrimaryOperator(string.Format("{0}, {0}: {1};", Pattern.Operand, Pattern.Identifier), null);



        private static readonly List<PrimaryOperator> AllOperators =
            new List<PrimaryOperator>
            {
                MemberAccess,
                MethodInvocation,
                Indexer,
                VariableDeclaring,
                VariableDeclaringWithAssignment,
                MaltyVariableDeclaring,
                new PrimaryOperator("Result","Result"),
                new PrimaryOperator("raise","raise"),
                new PrimaryOperator("inc","inc"),
                new PrimaryOperator("dec","dec"),
                new PrimaryOperator("as","as"),
                new PrimaryOperator("break","break"),
                new PrimaryOperator("continue","continue"),
                new PrimaryOperator("exit","exit"),
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
