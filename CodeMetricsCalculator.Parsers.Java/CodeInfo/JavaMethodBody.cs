using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    class JavaMethodBody : JavaCode, IMethodBodyInfo
    {
        private readonly IMethodInfo _method;

        public JavaMethodBody(IMethodInfo method, string originalSource) : base(originalSource)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            _method = method;
        }

        public IMethodInfo Method
        {
            get { return _method; }
        }
    }
}
