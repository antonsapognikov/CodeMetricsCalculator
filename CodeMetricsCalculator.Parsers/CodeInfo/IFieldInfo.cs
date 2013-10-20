namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IFieldInfo : ICodeInfo
    {
        IClassInfo Class { get; }

        string Name { get; }

        IClassInfo DeclaredInClass { get; }
    }
}
