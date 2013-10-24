using System;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public abstract class MemberInfo : IMemberInfo
    {
        private readonly string _originalSource;
        private string _normolizedSource;

        protected MemberInfo(string originalSource)
        {
            if (originalSource == null)
                throw new ArgumentNullException("originalSource");

            _originalSource = originalSource;
        }
        
        public string OriginalSource
        {
            get { return _originalSource; }
        }

        public string NormalizedSource
        {
            get { return _normolizedSource ?? (_normolizedSource = NormolizeSource(_originalSource)); }
        }

        protected abstract string NormolizeSource(string originalSource);
        
        public override string ToString()
        {
            return NormalizedSource;
        }
    }
}