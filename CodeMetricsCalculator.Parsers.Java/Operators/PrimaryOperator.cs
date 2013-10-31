using System;
using System.Collections.Generic;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    internal class PrimaryOperator : JavaOperator
    { 
        static PrimaryOperator()
        {
            AllOperators = new List<PrimaryOperator>
            {
                MemberAccess,
                MethodInvocation,
                Indexer,
                PostIncrement,
                PostDecrement,
                CreateObject,
                //CreateArray,
                Instanceof,
                Return,
                Continue,
                Break,
                GoTo
            };
        }

        private static readonly List<PrimaryOperator> AllOperators;

        public PrimaryOperator(string operatorString) : this(operatorString, null)
        {
        }

        public PrimaryOperator(string operatorString, string keyword)
            : base(operatorString, keyword)
        {
        }

        public static IReadOnlyCollection<PrimaryOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
        
        public static readonly PrimaryOperator MemberAccess = new PrimaryOperator("x.m");
        public static readonly PrimaryOperator MethodInvocation = new PrimaryOperator("x(...)");
        public static readonly PrimaryOperator Indexer = new PrimaryOperator("x[...]");
        public static readonly PrimaryOperator PostIncrement = new PrimaryOperator("x++");
        public static readonly PrimaryOperator PostDecrement = new PrimaryOperator("x--");
        public static readonly PrimaryOperator CreateObject = new PrimaryOperator("new T(...)", "new");
        //public static readonly PrimaryOperator CreateArray = new PrimaryOperator("new T[...]");
        public static readonly PrimaryOperator Instanceof = new PrimaryOperator("instanceof(T)", "instanceof");
        public static readonly PrimaryOperator Return = new PrimaryOperator("return", "return");
        public static readonly PrimaryOperator Continue = new PrimaryOperator("continue", "continue");
        public static readonly PrimaryOperator Break = new PrimaryOperator("break", "break");
        public static readonly PrimaryOperator GoTo = new PrimaryOperator("goto x", "goto");
    }
}
