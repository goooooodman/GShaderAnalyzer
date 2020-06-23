/** 
 *FileName:       ShaderAnalyzerSettingsPanel.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-23
 *Description:    用于绘制用户设置界面
 *History:        
*/
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GShaderAnalyzer
{
    public static class ShaderAnalyzerSettingsPanel
    {
        [SettingsProvider]
        public static SettingsProvider PreferencesGUI()
        {
            SettingsProvider sp = new SettingsProvider("Preference", SettingsScope.User)
            {
                label = "ShaderAnalyzer设置",
                guiHandler = (string s) => 
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.BeginChangeCheck();
                    string maliPath = EditorGUILayout.TextField("Mali离线编译器路径", Settings.MaliOfflineCompilePath);
                    if (EditorGUI.EndChangeCheck())
                    {
                        verifyMaliOfflineCompiler(maliPath);
                    }
                    if (GUILayout.Button("..."))
                    {
                        string selectMaliPath = EditorUtility.OpenFilePanel("请选择", Application.dataPath, "exe");
                        if (!string.IsNullOrEmpty(selectMaliPath))
                        {
                            verifyMaliOfflineCompiler(selectMaliPath);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.HelpBox("请选择高于7.0版本的编译器，7.0及以上版本的离线编译器已经被集成在Mali Studio中", MessageType.Info);
                }
            };
            return sp;
        }

        private static void verifyMaliOfflineCompiler(string path)
        {
            if (!File.Exists(path))
            {
                EditorUtility.DisplayDialog("错误", "路径为空", "OK");
                return;
            }

            List<string> lines = new List<string>();

            ProcessStartInfo psi = new ProcessStartInfo(path, "--version");
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;

            Process process = Process.Start(psi);
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += new DataReceivedEventHandler(
                (object o, DataReceivedEventArgs e) =>
                {
                    lines.Add(e.Data);
                }
            );
            process.BeginOutputReadLine();

            process.WaitForExit();

            process.Dispose();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < lines.Count; i++)
            {
                sb.AppendLine(lines[i]);
            }

            if (!lines[0].StartsWith("Mali Offline Compiler"))
            {
                EditorUtility.DisplayDialog("错误", "编译器返回值错误，请检查所选编译器。\n " + sb.ToString(), "OK");
                return;
            }

            Match match = Regex.Match(lines[0], "([0-9]{1}\\.[0-9]{1}\\.[0-9]{1})");
            if (match == null)
            {
                EditorUtility.DisplayDialog("错误", "未能检索到版本号，请检查所选编译器。\n " + sb.ToString(), "OK");
                return;
            }

            string[] versions = match.Value.Split(new string[] { "." }, System.StringSplitOptions.RemoveEmptyEntries);

            if (int.Parse(versions[0]) < 7)
            {
                EditorUtility.DisplayDialog("错误", "所选版本为" + match.Value + "，请选择高于7.0版本的编译器！", "OK");
                return;
            }


            Settings.MaliOfflineCompilePath = path;
        }
    }
}