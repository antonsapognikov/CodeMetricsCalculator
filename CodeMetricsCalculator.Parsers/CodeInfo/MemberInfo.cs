using System;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public abstract class MemberInfo : IMemberInfo
    {
        private readonly string _originalSource;
        private string _normalizedSource;

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
            get { return _normalizedSource ?? (_normalizedSource = NormalizeSource(_originalSource)); }
        }

        protected abstract string NormalizeSource(string originalSource);
        
        public override string ToString()
        {
            return NormalizedSource;
        }
    }
}