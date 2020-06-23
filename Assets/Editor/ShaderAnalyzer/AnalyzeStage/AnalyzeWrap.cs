/** 
 *FileName:       AnalyzeWrap.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    解析中间类，保存部分解析中间数据
 *History:        
*/
using System.Collections.Generic;

namespace GShaderAnalyzer
{
    public sealed class AnalyzeWrap
    {
        public AnalyzedData AnalyzedData;

        /// <summary>
        /// 所有Subshader的容器
        /// </summary>
        public List<AnalyzedSubshaderData> Subshaders = new List<AnalyzedSubshaderData>();
        /// <summary>
        /// 所有Pass的容器
        /// </summary>
        public List<AnalyzedPassData> Passes = new List<AnalyzedPassData>();
        /// <summary>
        /// 所有变体的容器
        /// </summary>
        public List<AnalyzedVariantData> Variants = new List<AnalyzedVariantData>();

        /// <summary>
        /// 缓存头
        /// </summary>
        public CacheHead Head;

        /// <summary>
        /// 编译后着色器文件路径
        /// </summary>
        public string CompiledShaderFilePath = string.Empty;
        /// <summary>
        /// 解析数据缓存地址
        /// </summary>
        public string AnalyzedDataCacheFilePath = string.Empty;
        /// <summary>
        /// 变体工作目录
        /// </summary>
        public Dictionary<AnalyzedVariantData, string> VariantDirectory = new Dictionary<AnalyzedVariantData, string>();
    }
}