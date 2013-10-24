namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IFieldInfo : IMemberInfo
    {
        IClassInfo Class { get; }

        string Name { get; }
    }
}
