/** 
 *FileName:     GPUModelInfo.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-10 
 *Description:  用于描述GPU模型信息，并序列化成资源
 *History: 
*/
using System;

namespace GShaderAnalyzer
{
    /// <summary>
    /// GPU模型信息类型
    /// </summary>
    [Serializable]
    public class GPUModelInfo
    {
        /// <summary>
        /// GPU模型名
        /// </summary>
        public string ModelName = string.Empty;
    }
}