/** 
 *FileName:       AnalyzedShaderDataBase.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    着色器解析数据(指某一个阶段的着色器，例如顶点着色器)
 *History:        
*/
using System;

namespace GShaderAnalyzer
{
    /// <summary>
    /// 着色器解析数据
    /// </summary>
    [Serializable]
    public abstract class AnalyzedShaderDataBase : AnalyzedBase
    {
        /// <summary>
        /// 若无报错则为空，
        /// </summary>
        public string ERROR = string.Empty;

        /// <summary>
        /// 着色器类型
        /// </summary>
        public EShaderType ShaderType = EShaderType.None;

        /// <summary>
        /// 着色器源代码
        /// </summary>
        public string SourceCode;
    }
}