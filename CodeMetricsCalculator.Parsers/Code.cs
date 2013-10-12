using System;

namespace CodeMetricsCalculator.Parsers
{
    public abstract class Code : ICode
    {
        private readonly string _originalSource;
        private string[] _lines;
        private string _normolizedSource;

        protected Code(string originalSource)
        {
            if (originalSource == null)
                throw new ArgumentNullException("originalSource");
            _originalSource = originalSource;
        }


        public string OriginalSource
        {
            get { return _originalSource; }
        }

        public string NormolizedSource
        {
            get { return _normolizedSource ?? (_normolizedSource = NormolizeSource(_originalSource)); }
        }

        public string[] Lines
        {
            get { return _lines ?? (_lines = SplitSource()); }
        }

        protected abstract string NormolizeSource(string originalSource);

        protected virtual string[] SplitSource()
        {
            return NormolizedSource.Split(new[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);
        }

        public override string ToString()
        {
            return NormolizedSource;
        }
    }
}