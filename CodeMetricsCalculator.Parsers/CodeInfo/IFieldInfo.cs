namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IFieldInfo : IIdentifierInfo
    {
        IClassInfo Class { get; }

        ITypeInfo Type { get; }
    }
}
