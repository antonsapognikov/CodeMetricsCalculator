namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface ICodeInfo
    {
        string OriginalSource { get; }

        string NormolizedSource { get; }
    }
}