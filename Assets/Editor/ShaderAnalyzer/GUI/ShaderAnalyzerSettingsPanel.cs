/** 
 *FileName:       ShaderAnalyzerSettingsPanel.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-23
 *Description:    ���ڻ����û����ý���
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
                label = "ShaderAnalyzer����",
                guiHandler = (string s) => 
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.BeginChangeCheck();
                    string maliPath = EditorGUILayout.TextField("Mali���߱�����·��", Settings.MaliOfflineCompilePath);
                    if (EditorGUI.EndChangeCheck())
                    {
                        verifyMaliOfflineCompiler(maliPath);
                    }
                    if (GUILayout.Button("..."))
                    {
                        string selectMaliPath = EditorUtility.OpenFilePanel("��ѡ��", Application.dataPath, "exe");
                        if (!string.IsNullOrEmpty(selectMaliPath))
                        {
                            verifyMaliOfflineCompiler(selectMaliPath);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.HelpBox("��ѡ�����7.0�汾�ı�������7.0�����ϰ汾�����߱������Ѿ���������Mali Studio��", MessageType.Info);
                }
            };
            return sp;
        }

        private static void verifyMaliOfflineCompiler(string path)
        {
            if (!File.Exists(path))
            {
                EditorUtility.DisplayDialog("����", "·��Ϊ��", "OK");
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
                EditorUtility.DisplayDialog("����", "����������ֵ����������ѡ��������\n " + sb.ToString(), "OK");
                return;
            }

            Match match = Regex.Match(lines[0], "([0-9]{1}\\.[0-9]{1}\\.[0-9]{1})");
            if (match == null)
            {
                EditorUtility.DisplayDialog("����", "δ�ܼ������汾�ţ�������ѡ��������\n " + sb.ToString(), "OK");
                return;
            }

            string[] versions = match.Value.Split(new string[] { "." }, System.StringSplitOptions.RemoveEmptyEntries);

            if (int.Parse(versions[0]) < 7)
            {
                EditorUtility.DisplayDialog("����", "��ѡ�汾Ϊ" + match.Value + "����ѡ�����7.0�汾�ı�������", "OK");
                return;
            }


            Settings.MaliOfflineCompilePath = path;
        }
    }
}