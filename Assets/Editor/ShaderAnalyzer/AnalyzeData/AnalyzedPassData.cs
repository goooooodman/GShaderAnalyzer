/** 
 *FileName:       AnalyzedPassData.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    Pass解析数据
 *History:        
*/
using System;
using System.Collections.Generic;

namespace GShaderAnalyzer
{
    /// <summary>
    /// Pass解析数据
    /// </summary>
    [Serializable]
    public sealed class AnalyzedPassData : AnalyzedBase
    {
        public string PassName = string.Empty;

        public List<AnalyzedVariantData> Variants = new List<AnalyzedVariantData>();
    }
}