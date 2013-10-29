using System.Text.RegularExpressions;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public enum OperationType
    {
        Unary,
        Binary,
        Ternary
    }

    public enum OperatorSyntax
    {
        /// <summary>
        /// Префиксная (польская) (+ab) нотация.
        /// </summary>
        Prefix,

        /// <summary>
        /// Постфиксная (обратная польская) (ab+) нотация.
        /// </summary>
        Postfix,

        /// <summary>
        /// Инфиксная (a+b) нотация.
        /// </summary>
        Infix
    }

    public interface IOperatorInfo : IMemberInfo
    {
        string Name { get; }

        /// <summary>
        /// Блочный ли оператор. Например: if (...) {...}
        /// </summary>
        bool IsBlock { get; }

        /// <summary>
        /// Первичный ли оператор. Например: instanceof, new T(...), x(...)
        /// </summary>
        bool IsPrimary { get; }
    }
}