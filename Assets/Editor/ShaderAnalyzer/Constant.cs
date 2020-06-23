/** 
 *FileName:       Constant.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    插件使用的一些常量
 *History:        
*/
using UnityEngine;

namespace GShaderAnalyzer
{
    public static class Constant
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public const string Version = "0.0.1";

        public const string k_ArgumentKey = "kScriptEditorArgs";
        public const string k_DefaultArgument = "$(File)";
        public const string k_TranslatorArgument = "\"$(ProjectPath)\" \"$(File)\"";
        public const string k_ScriptsDefaultApp = "kScriptsDefaultApp";
        public const string k_ScriptEditorArgs = "kScriptEditorArgs_";

        /// <summary>
        /// Bridge程序进程名
        /// </summary>
        public const string BridgeAppProcessName = "CompiledShaderBridge";

        /// <summary>
        /// Bridge程序路径
        /// </summary>
        public static readonly string BridgeAppPath = Application.dataPath + "/../Tools/CompiledShaderBridge.exe";

        /// <summary>
        /// 生成的Shader代码路径格式
        /// </summary>
        public static readonly string CompiledShaderPathFormat = Application.dataPath + "/../Temp/Compiled-{0}.shader";

        /// <summary>
        /// 解析工作目录
        /// </summary>
        public static readonly string SAWorkSpace = Application.dataPath + "/../Temp/SAWorkSpace";
        /// <summary>
        /// 缓存目录
        /// </summary>
        public static readonly string CacheFolder = Application.dataPath + "/../Temp/SACacheSpace";
        /// <summary>
        /// 缓存文件路径格式
        /// ShaderName("/" replace with "-")_APIType_Vendor_GPUModel_SkipUnusedVariant
        /// </summary>
        public static readonly string AnalyzedDataCachePathFormat = CacheFolder + "/{0}_{1}_{2}_{3}_{4}.sad";

        /// <summary>
        /// 缓存文件头格式
        /// </summary>
        public const string AnalyzedDataCacheHeadFormat = "{0}|{1}";
    }
}