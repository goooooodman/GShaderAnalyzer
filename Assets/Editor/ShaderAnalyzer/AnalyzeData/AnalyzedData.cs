/** 
 *FileName:       AnalyzedData.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    解析数据
 *History:        
*/
using System;
using System.Collections.Generic;

namespace GShaderAnalyzer
{
    /// <summary>
    /// 解析数据
    /// </summary>
    [Serializable]
    public sealed class AnalyzedData
    {
        public GPUModelInfo GPUModelInfo;

        public string Hardware = string.Empty;

        public string Driver = string.Empty;

        public string ShaderName = string.Empty;

        public List<AnalyzedSubshaderData> Subshaders = new List<AnalyzedSubshaderData>();
    }
}