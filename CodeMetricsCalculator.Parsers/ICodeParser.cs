namespace CodeMetricsCalculator.Parsers
{
    public interface ICodeParser<in TCode, out TParsingResult> where TCode : ICode
    {
        TParsingResult Parse(TCode code);
    }
}