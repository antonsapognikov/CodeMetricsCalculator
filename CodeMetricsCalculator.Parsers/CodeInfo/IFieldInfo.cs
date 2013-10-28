namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IFieldInfo : IVariableInfo
    {
        IClassInfo Class { get; }
    }
}
