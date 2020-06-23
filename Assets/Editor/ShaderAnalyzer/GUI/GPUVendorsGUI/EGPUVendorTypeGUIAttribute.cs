/** 
 *FileName:     GPUVendorTypeGUIAttribute.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-10 
 *Description:  用于标记GPUVendor的EGPUVendorType类型字段
 *History: 
*/
using UnityEngine;

namespace GShaderAnalyzer
{
    public class EGPUVendorTypeGUIAttribute : PropertyAttribute
    {
        public string Tooltip = string.Empty;

        public EGPUVendorTypeGUIAttribute(string toolTip)
        {
            Tooltip = toolTip;
        }
    }
}