﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IMethodBodyInfo : IMemberInfo
    {
        IMethodInfo Method { get; }
    }
}
