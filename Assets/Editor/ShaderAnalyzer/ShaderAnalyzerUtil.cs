/** 
 *FileName:     ShaderAnalyzerUtil.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-21 
 *Description:  着色器解析工具类
 *History: 
*/
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Unity.CodeEditor;
using UnityEditor;
using UnityEngine;

namespace GShaderAnalyzer
{
    public static class ShaderAnalyzerUtil
    {
        public static readonly MethodInfo OpenCompiledShader = typeof(ShaderUtil).GetMethod("OpenCompiledShader", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// 通过着色器类型获取着色器文件扩展名
        /// </summary>
        /// <param name="type">着色器类型</param>
        /// <returns>着色器文件扩展名</returns>
        public static string GetFileExtensionByShaderType(EShaderType type)
        {
            string extension = string.Empty;
            switch (type)
            {
                case EShaderType.Vertex:
                    extension = ".vert";
                    break;
                case EShaderType.Fragment:
                    extension = ".frag";
                    break;
                case EShaderType.Compute:
                    extension = ".comp";
                    break;
                case EShaderType.Geometry:
                    extension = ".geom";
                    break;
                case EShaderType.Tessellation_Control:
                    extension = ".tesc";
                    break;
                case EShaderType.Tessellation_Evaluation:
                    extension = ".tese";
                    break;
            }
            return extension;
        }

        /// <summary>
        /// 去掉文件名中的无效字符,如 \ / : * ? " < > | 
        /// </summary>
        /// <param name="fileName">待处理的文件名</param>
        /// <returns>处理后的文件名</returns>
        public static string ReplaceBadCharOfFileName(string fileName)
        {
            string str = fileName;
            str = str.Replace("\\", string.Empty);
            str = str.Replace("/", string.Empty);
            str = str.Replace(":", string.Empty);
            str = str.Replace("*", string.Empty);
            str = str.Replace("?", string.Empty);
            str = str.Replace("\"", string.Empty);
            str = str.Replace("<", string.Empty);
            str = str.Replace(">", string.Empty);
            str = str.Replace("|", string.Empty);
            str = str.Replace(" ", string.Empty);
            return str;
        }

        /// <summary>
        /// 获取枚举字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumString<T>(this T enumValue)
        {
            return Enum.GetName(typeof(T), enumValue);
        }

        private static string m_ChosenInstallation;
        private static string Installation
        {
            get
            {
                if (string.IsNullOrEmpty(m_ChosenInstallation))
                    m_ChosenInstallation = CodeEditor.CurrentEditorInstallation;
                return m_ChosenInstallation;
            }
        }

        private static string Arguments
        {
            get
            {
                // Starting in Unity 5.5, we support setting script editor arguments on OSX and
                // use then when opening the script editor.
                // Before Unity 5.5, we would still save the default script editor args in EditorPrefs,
                // even though we never used them. This means that the user potentially has some
                // script editor args saved and once he upgrades to 5.5, they will be used when
                // open the script editor. Which unintended and causes a regression in behaviour.
                // So on OSX we change the key for per application for script editor args,
                // to avoid reading the one from previous versions.
                // The year 2021: Delete mac hack.
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    var oldMac = EditorPrefs.GetString(Constant.k_ScriptEditorArgs + Installation);
                    if (!string.IsNullOrEmpty(oldMac))
                        EditorPrefs.SetString(Constant.k_ArgumentKey, oldMac);
                }

                return EditorPrefs.GetString(Constant.k_ArgumentKey + Installation, Constant.k_DefaultArgument);
            }
            set
            {
                if (Application.platform == RuntimePlatform.OSXEditor)
                    EditorPrefs.SetString(Constant.k_ScriptEditorArgs + Installation, value);

                EditorPrefs.SetString(Constant.k_ArgumentKey + Installation, value);
            }
        }

        /// <summary>
        /// 用户设置的IDE参数格式
        /// </summary>
        private static string m_OldArgsFormat;
        /// <summary>
        /// 用户设置的IDE
        /// </summary>
        private static string m_OldScriptEditor;

        /// <summary>
        /// 切换当前IDE为Bridge
        /// </summary>
        public static void ChangeIDEToBridge()
        {
            m_OldArgsFormat = Arguments;
            m_OldScriptEditor = EditorPrefs.GetString(Constant.k_ScriptsDefaultApp);

            CodeEditor.SetExternalScriptEditor(Constant.BridgeAppPath);
        }

        /// <summary>
        /// 还原IDE设置
        /// </summary>
        public static void RevertIDESettings()
        {
            CodeEditor.SetExternalScriptEditor(m_OldScriptEditor);
            Arguments = m_OldArgsFormat;
        }

        /// <summary>
        /// 从缓存中读取文件头
        /// </summary>
        /// <param name="cachePath"></param>
        /// <returns></returns>
        public static CacheHead GetHeadFromCache(string cachePath)
        {
            if (!File.Exists(cachePath))
                return null;
            
            StreamReader sr = new StreamReader(cachePath);

            string head = sr.ReadLine();
            sr.Dispose();

            return new CacheHead(head);
        }

        /// <summary>
        /// 获取编译后着色器的路径
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetCompiledShaderPath(ShaderAnalyzeParams param)
        {
            return string.Format(Constant.CompiledShaderPathFormat, param.Shader.name.Replace('/', '-'));
        }

        /// <summary>
        /// 获取解析数据缓存文件名
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetChacheFileName(ShaderAnalyzeParams param)
        {
            string shaderName = param.Shader.name.Replace("/", "-");
            return string.Format(Constant.AnalyzedDataCachePathFormat, shaderName, param.APIType, param.Vendor, param.GPUModel, param.SkipUnusedVariant);
        }

        /// <summary>
        /// 获取缓存文件头
        /// </summary>
        /// <param name="version">版本号</param>
        /// <param name="md5">编译后着色器的MD5</param>
        /// <returns></returns>
        public static string GetCacheHead(string version, string md5)
        {
            return string.Format(Constant.AnalyzedDataCacheHeadFormat, version, md5);
        }

        /// <summary>
        /// 计算文件MD5
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string GenerateMD5(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                using (MD5 mi = MD5.Create())
                {
                    byte[] newBuffer = mi.ComputeHash(stream);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < newBuffer.Length; i++)
                    {
                        sb.Append(newBuffer[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }
    }
}