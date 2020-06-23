/** 
 *FileName:       CacheHead.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    解析数据缓存文件头类型
 *History:        
*/
using UnityEngine;

namespace GShaderAnalyzer
{
    public sealed class CacheHead
    {
        /// <summary>
        /// ShaderAnalyzer版本号
        /// </summary>
        public string Version { private set; get; }
        /// <summary>
        /// 编译成GLSL的着色器文件MD5
        /// </summary>
        public string MD5 { private set; get; }
        /// <summary>
        /// 解析数据文件头字符串
        /// </summary>
        public string Head { private set; get; }

        /// <summary>
        /// 构造函数，由传入的缓存文件头字符串解析MD5以及版本号
        /// </summary>
        /// <param name="head">文件头字符串</param>
        public CacheHead(string head)
        {
            string[] dataInfos = head.Split('|');
            if (dataInfos == null || dataInfos.Length < 2)
            {
                Debug.LogError("解析缓存头失败，" + head);
                return;
            }
            Version = dataInfos[0];
            MD5 = dataInfos[1];
            Head = head;
        }

        /// <summary>
        /// 构造函数，有传入的版本号以及MD5计算缓存文件头字符串
        /// </summary>
        /// <param name="version">版本号</param>
        /// <param name="md5">MD5</param>
        public CacheHead(string version, string md5)
        {
            Version = version;
            MD5 = md5;
            Head = ShaderAnalyzerUtil.GetCacheHead(Version, MD5);
        }
    }
}