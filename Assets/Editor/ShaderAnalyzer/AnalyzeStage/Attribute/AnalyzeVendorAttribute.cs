/** 
 *FileName:       AnalyzeVendorAttribute.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    阶段类的厂商属性，用于标记该阶段类作用于哪个GPU厂商，可选
 *History:        
*/
using System;

namespace GShaderAnalyzer
{
    /// <summary>
    /// 阶段类的厂商属性
    /// </summary>
    public sealed class AnalyzeVendorAttribute : Attribute
    {
        public EGPUVendorType Vendor = EGPUVendorType.Mali;
        public AnalyzeVendorAttribute(EGPUVendorType vendor) { Vendor = vendor; }
    }
}