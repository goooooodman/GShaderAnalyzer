/** 
 *FileName:     ShaderAnalyzeParams.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-21 
 *Description:  着色器解析参数集
 *History: 
*/
using UnityEngine;

namespace GShaderAnalyzer
{
    /// <summary>
    /// 着色器解析参数集
    /// </summary>
    public sealed class ShaderAnalyzeParams
    {
        /// <summary>
        /// 指定的着色器
        /// </summary>
        public Shader Shader          = null;
        /// <summary>
        /// API类型
        /// </summary>
        public EAPIType APIType       = EAPIType.OpenGLES;
        /// <summary>
        /// 是否跳过当前场景未使用的变体
        /// </summary>
        public bool SkipUnusedVariant = true;
        /// <summary>
        /// GPU厂商类型
        /// </summary>
        public EGPUVendorType Vendor  = EGPUVendorType.Mali;
        /// <summary>
        /// GPU类型
        /// </summary>
        public GPUModelInfo GPUModel  = null;
    }
}