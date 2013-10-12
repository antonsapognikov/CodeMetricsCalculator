using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.Java.Operators;

namespace CodeMetricsCalculator.Parsers.Java
{
    public abstract class JavaOperator : JavaCode, IOperator
    {
        public static readonly JavaOperator Assignment = new AssignmentOperator();

        public static readonly JavaOperator Additiv = new ArithmeticOperator("+");
        public static readonly JavaOperator Subtraction = new ArithmeticOperator("-");

        public static readonly JavaOperator Multiplication =
            new ArithmeticOperator("*");

        public static readonly JavaOperator Division = new ArithmeticOperator("/");
        public static readonly JavaOperator Remainder = new ArithmeticOperator("%");

        public static readonly JavaOperator UnaryPlus = new UnaryOperator("+");
        public static readonly JavaOperator UnaryMinus = new UnaryOperator("-");

        public static readonly JavaOperator Increment =
            new UnaryOperator("++", new Regex(@"[^+]\+\+[^+=]", RegexOptions.Compiled));

        public static readonly JavaOperator Decrement = new UnaryOperator("--");
        public static readonly JavaOperator LogicalComplement = new UnaryOperator("!");

        public static readonly JavaOperator Equal = new RelationalOperator("==");
        public static readonly JavaOperator NotEqual = new RelationalOperator("!=");
        public static readonly JavaOperator Greater = new RelationalOperator(">");
        public static readonly JavaOperator GreaterOrEqual = new RelationalOperator(">=");
        public static readonly JavaOperator Less = new RelationalOperator("<");
        public static readonly JavaOperator LessOrEqual = new RelationalOperator("<=");

        public static readonly JavaOperator ConditionalAnd = new ConditionalOperator("&&");

        public static readonly JavaOperator ConditionalOr =
            new ConditionalOperator("||", new Regex(@"\|\|", RegexOptions.Compiled));

        public static readonly JavaOperator IfThenElse =
            new ConditionalOperator("?:", new Regex(@".*?.*:", RegexOptions.Compiled)); //todo

        public static readonly JavaOperator Instanceof = new InstanceofOperator();


        public static readonly JavaOperator UnaryBitwiseComplement = new BitwiseOperator("~");
        public static readonly JavaOperator SignedLeftShift = new BitwiseOperator("<<");
        public static readonly JavaOperator SignedRightShift = new BitwiseOperator(">>");
        public static readonly JavaOperator UnsignedRightShift = new BitwiseOperator(">>>");
        public static readonly JavaOperator BitwiseAnd = new BitwiseOperator("&");
        public static readonly JavaOperator BitwiseExclusiveOr = new BitwiseOperator("^");

        public static readonly JavaOperator BitwiseInclusiveOr = new BitwiseOperator("|",
                                                                                     new Regex(@"[^|]\|[^|=]",
                                                                                               RegexOptions.Compiled));

        private static readonly IReadOnlyCollection<JavaOperator> AllOperators =
            new ReadOnlyCollection<JavaOperator>(new List<JavaOperator>
                {
                    Assignment,
                    Additiv,
                    Subtraction,
                    Multiplication,
                    Remainder,
                    UnaryPlus,
                    UnaryMinus,
                    Increment,
                    Decrement,
                    LogicalComplement,
                    Equal,
                    NotEqual,
                    Greater,
                    GreaterOrEqual,
                    Less,
                    LessOrEqual,
                    ConditionalAnd,
                    ConditionalOr,
                    IfThenElse,
                    Instanceof,
                    UnaryBitwiseComplement,
                    SignedLeftShift,
                    SignedRightShift,
                    UnsignedRightShift,
                    BitwiseAnd,
                    BitwiseExclusiveOr,
                    BitwiseInclusiveOr
                });

        private readonly Regex _parsingRegex;

        protected JavaOperator(string operatorString, Regex parsingRegex)
            : base(operatorString)
        {
            if (parsingRegex == null)
                throw new ArgumentNullException("parsingRegex");
            _parsingRegex = parsingRegex;
        }

        public static IReadOnlyCollection<JavaOperator> Operators
        {
            get { return AllOperators; }
        }

        public Regex ParsingRegex
        {
            get { return _parsingRegex; }
        }
    }
}