using System.Collections.Generic;
using System.Collections.ObjectModel;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.Operators;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    public abstract class JavaOperator : JavaCode, IOperatorInfo
    {
        public static readonly JavaOperator Assignment = new AssignmentOperator();

        public static readonly JavaOperator Additiv = new ArithmeticOperator("+");
        public static readonly JavaOperator Subtraction = new ArithmeticOperator("-");
        public static readonly JavaOperator Multiplication = new ArithmeticOperator("*");
        public static readonly JavaOperator Division = new ArithmeticOperator("/");
        public static readonly JavaOperator Remainder = new ArithmeticOperator("%");

        public static readonly JavaOperator UnaryPlus = new UnaryOperator("+", OperatorSyntax.Prefix);
        public static readonly JavaOperator UnaryMinus = new UnaryOperator("-", OperatorSyntax.Prefix);
        public static readonly JavaOperator PrefixIncrement = new UnaryOperator("++", OperatorSyntax.Prefix);
        public static readonly JavaOperator PrefixDecrement = new UnaryOperator("--", OperatorSyntax.Prefix);
        public static readonly JavaOperator PostfixIncrement = new UnaryOperator("++", OperatorSyntax.Postfix);
        public static readonly JavaOperator PostfixDecrement = new UnaryOperator("--", OperatorSyntax.Postfix);
        public static readonly JavaOperator LogicalComplement = new UnaryOperator("!", OperatorSyntax.Prefix);

        public static readonly JavaOperator Equal = new RelationalOperator("==");
        public static readonly JavaOperator NotEqual = new RelationalOperator("!=");
        public static readonly JavaOperator Greater = new RelationalOperator(">");
        public static readonly JavaOperator GreaterOrEqual = new RelationalOperator(">=");
        public static readonly JavaOperator Less = new RelationalOperator("<");
        public static readonly JavaOperator LessOrEqual = new RelationalOperator("<=");

        public static readonly JavaOperator ConditionalAnd = new ConditionalOperator("&&", OperationType.Binary);
        public static readonly JavaOperator ConditionalOr = new ConditionalOperator("||", OperationType.Binary);
        public static readonly JavaOperator IfThenElse = new ConditionalOperator("?:", OperationType.Ternary);

        public static readonly JavaOperator Instanceof = new InstanceofOperator();
        
        public static readonly JavaOperator UnaryBitwiseComplement = new BitwiseOperator("~");
        public static readonly JavaOperator SignedLeftShift = new BitwiseOperator("<<");
        public static readonly JavaOperator SignedRightShift = new BitwiseOperator(">>");
        public static readonly JavaOperator UnsignedRightShift = new BitwiseOperator(">>>");
        public static readonly JavaOperator BitwiseAnd = new BitwiseOperator("&");
        public static readonly JavaOperator BitwiseExclusiveOr = new BitwiseOperator("^");
        public static readonly JavaOperator BitwiseInclusiveOr = new BitwiseOperator("|");

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
                    PrefixIncrement,
                    PrefixDecrement,
                    PostfixIncrement,
                    PostfixDecrement,
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

        private readonly OperationType _operationType;
        private readonly OperatorSyntax _syntax;

        protected JavaOperator(string operatorString, OperationType operationType, OperatorSyntax syntax)
            : base(operatorString)
        {
            _operationType = operationType;
            _syntax = syntax;
        }

        public static IReadOnlyCollection<JavaOperator> Operators
        {
            get { return AllOperators; }
        }

        public string Name
        {
            get { return NormolizedSource; }
        }

        public OperationType OperationType
        {
            get { return _operationType; }
        }

        public OperatorSyntax Syntax
        {
            get { return _syntax; }
        }
    }
}