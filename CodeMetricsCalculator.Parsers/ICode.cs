namespace CodeMetricsCalculator.Parsers
{
    public interface ICode
    {
        string OriginalSource { get; }

        string NormolizedSource { get; }

        string[] Lines { get; }
    }
}