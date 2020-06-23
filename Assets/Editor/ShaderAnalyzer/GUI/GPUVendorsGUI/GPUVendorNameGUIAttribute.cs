/** 
 *FileName:     GPUVendorNameGUIAttribute.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-10 
 *Description:  用于标记GPUVendor的GPUVendorName字段
 *History: 
*/
using UnityEngine;

namespace GShaderAnalyzer
{
    public class GPUVendorNameGUIAttribute : PropertyAttribute
    {
        public string Tooltip = string.Empty;

        public GPUVendorNameGUIAttribute(string toolTip)
        {
            Tooltip = toolTip;
        }
    }
}