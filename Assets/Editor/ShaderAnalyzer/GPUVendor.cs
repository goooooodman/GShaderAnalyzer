/** 
 *FileName:     GPUVendor.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-21 
 *Description:  GPU厂商信息，用于序列化成配置文件
 *History: 
*/
using System;
using UnityEngine;

namespace GShaderAnalyzer
{
    /// <summary>
    /// GPU厂商信息
    /// </summary>
    [Serializable]
    public sealed class GPUVendor
    {
        [GPUVendorNameGUI("厂商名")]
        public string Name = string.Empty;

        [EGPUVendorTypeGUI("厂商类型")]
        public EGPUVendorType VendorType = EGPUVendorType.Mali;

        [EAPITypeGUI("兼容的API")]
        public EAPIType CompatibleAPI = EAPIType.OpenGLES;

        [Tooltip("GPU模型")]
        public GPUModelInfo[] Models = null;
    }
}