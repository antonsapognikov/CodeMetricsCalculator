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
                CreateArray,
                Instanceof
            };
        }
        
        private static readonly List<PrimaryOperator> AllOperators;
        
        public PrimaryOperator(string operatorString) : base(operatorString, false, true)
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
        public static readonly PrimaryOperator CreateObject = new PrimaryOperator("new T(...)");
        public static readonly PrimaryOperator CreateArray = new PrimaryOperator("new T[...]");
        public static readonly PrimaryOperator Instanceof = new PrimaryOperator("instanceof(T)");
    }
}
