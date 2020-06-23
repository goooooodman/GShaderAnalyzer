/** 
 *FileName:       AnalyzeStageAttribute.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    阶段类的阶段属性，用于标记阶段类作用于哪个阶段，阶段类必须拥有此属性才可以被解析器识别
 *History:        
*/
using System;

namespace GShaderAnalyzer
{
    /// <summary>
    /// 阶段类的阶段属性
    /// </summary>
    public sealed class AnalyzeStageAttribute : Attribute
    {
        public EAnalyzeStageType Stage = EAnalyzeStageType.None;
        public AnalyzeStageAttribute(EAnalyzeStageType stage) { Stage = stage; }
    }
}