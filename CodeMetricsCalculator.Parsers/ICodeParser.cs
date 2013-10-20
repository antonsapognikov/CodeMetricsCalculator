﻿using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers
{
    public interface ICodeParser<in TCode, out TParsingResult> where TCode : ICodeInfo
    {
        TParsingResult Parse(TCode code);
    }
}