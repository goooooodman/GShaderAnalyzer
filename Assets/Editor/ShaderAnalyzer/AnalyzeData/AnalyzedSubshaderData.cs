/** 
 *FileName:       AnalyzedSubshaderData.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    Subshader解析数据
 *History:        
*/
using System;
using System.Collections.Generic;

namespace GShaderAnalyzer
{
    /// <summary>
    /// Subshader解析数据
    /// </summary>
    [Serializable]
    public sealed class AnalyzedSubshaderData : AnalyzedBase
    {
        public int LOD;

        public List<AnalyzedPassData> Passes = new List<AnalyzedPassData>();
    }
}