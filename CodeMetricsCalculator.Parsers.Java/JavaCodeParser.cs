namespace CodeMetricsCalculator.Parsers.Java
{
    public abstract class JavaCodeParser<TParsingResult> : ICodeParser<JavaCode, TParsingResult>
    {
        public abstract TParsingResult Parse(JavaCode code);
    }
}