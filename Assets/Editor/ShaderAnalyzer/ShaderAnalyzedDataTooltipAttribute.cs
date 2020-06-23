/** 
 *FileName:       ShaderAnalyzedDataTooltipAttribute.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-22
 *Description:    
 *History:         
*/
using System;

namespace GShaderAnalyzer
{
    public class ShaderAnalyzedDataTooltipAttribute : Attribute
    {
        public string Abbreviate;
        public string Tooltip;

        public ShaderAnalyzedDataTooltipAttribute(string abbreviate, string tooltip)
        {
            Abbreviate = abbreviate;
            Tooltip = tooltip;
        }
    }
}