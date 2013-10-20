using System;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public abstract class Code : ICodeInfo
    {
        private readonly string _originalSource;
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

        protected abstract string NormolizeSource(string originalSource);
        
        public override string ToString()
        {
            return NormolizedSource;
        }
    }
}