/** 
 *FileName:       AnalyzedBase.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    解析数据基类，用于声明着色器源代码，但此源代码只用于解析过程
 *History:        
*/
using System;
using System.Collections.Generic;

namespace GShaderAnalyzer
{
    /// <summary>
    /// 解析数据基类
    /// </summary>
    public abstract class AnalyzedBase
    {
        [NonSerialized]
        internal List<string> SourceCodeLines = new List<string>();
    }
}