namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IMemberInfo
    {
        string OriginalSource { get; }

        string NormalizedSource { get; }
    }
}