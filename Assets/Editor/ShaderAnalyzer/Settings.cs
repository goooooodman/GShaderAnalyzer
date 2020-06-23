/** 
 *FileName:       Settings.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-23
 *Description:    用于定义用户设置
 *History:         
*/
using UnityEditor;

namespace GShaderAnalyzer
{
    public static class Settings
    {
        public const string MaliOfflineCompilePathKey = "GShaderAnalyzerMaliOfflineCompilePath";
        public const string PowerVROfflineCompilePathKey = "GShaderAnalyzerPowerVROfflineCompilePath";
        public static string MaliOfflineCompilePath
        {
            get
            {
                string path = EditorPrefs.GetString(MaliOfflineCompilePathKey, string.Empty);

                return path;
            }
            set
            {
                EditorPrefs.SetString(MaliOfflineCompilePathKey, value);
            }
        }

        public static string PowerVROfflineCompilePath
        {
            get
            {
                return EditorPrefs.GetString(PowerVROfflineCompilePathKey, string.Empty);
            }
            set
            {
                EditorPrefs.SetString(PowerVROfflineCompilePathKey, value);
            }
        }
    }
}