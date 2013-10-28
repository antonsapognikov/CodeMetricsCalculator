using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    public class JavaCode : MemberInfo
    {
        public JavaCode(string originalSource)
            : base(originalSource)
        {
        }

        protected override string NormalizeSource(string originalSource)
        {
            //todo: удалить закомментированный код, очистить строковые литералы, 
            //todo: удалить лишние пробелы, пустые строки и т.п.
            return originalSource;
        }
    }
}