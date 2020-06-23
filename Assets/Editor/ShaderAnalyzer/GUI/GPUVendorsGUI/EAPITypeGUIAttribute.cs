/** 
 *FileName:     EAPITypeGUIAttribute.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-10 
 *Description:  用于标记GPUVendor的EAPIType类型字段
 *History: 
*/

using UnityEngine;

namespace GShaderAnalyzer
{
    public class EAPITypeGUIAttribute : PropertyAttribute
    {
        public string Tooltip = string.Empty;

        public EAPITypeGUIAttribute(string toolTip)
        {
            Tooltip = toolTip;
        }
    }
}