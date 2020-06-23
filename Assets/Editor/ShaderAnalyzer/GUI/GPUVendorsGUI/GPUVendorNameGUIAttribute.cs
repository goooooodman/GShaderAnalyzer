/** 
 *FileName:     GPUVendorNameGUIAttribute.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-10 
 *Description:  ���ڱ��GPUVendor��GPUVendorName�ֶ�
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