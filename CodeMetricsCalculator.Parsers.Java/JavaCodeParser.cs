using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    public abstract class JavaCodeParser<TCode, TParsingResult> : ICodeParser<TCode, TParsingResult>
        where TCode : JavaCode
    {
        public abstract TParsingResult Parse(TCode code);
    }
}