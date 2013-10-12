namespace CodeMetricsCalculator.Parsers.Java
{
    public class JavaCode : Code
    {
        public JavaCode(string originalSource)
            : base(originalSource)
        {
        }

        protected override string NormolizeSource(string originalSource)
        {
            //todo: удалить закомментированный код, очистить строковые литералы, удалить лишние пробелы и т.п.
            return originalSource;
        }
    }
}