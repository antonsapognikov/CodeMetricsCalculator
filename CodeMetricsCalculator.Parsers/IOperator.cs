using System.Text.RegularExpressions;

namespace CodeMetricsCalculator.Parsers
{
    public interface IOperator : ICode
    {
        Regex ParsingRegex { get; }
    }
}