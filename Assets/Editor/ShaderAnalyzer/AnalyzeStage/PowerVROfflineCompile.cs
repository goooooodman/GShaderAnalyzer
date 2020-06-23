/** 
 *FileName:       PowerVROfflineCompile.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    编译着色器阶段，用于PowerVRGPU
 *History:        
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GShaderAnalyzer
{
    [AnalyzeStage(EAnalyzeStageType.VendorOfflineCompile)]
    [AnalyzeVendor(EGPUVendorType.PowerVR)]
    public sealed class PowerVROfflineCompile : AnalyzeStageBase
    {
        protected override int init(AnalyzeWrap wrap, ShaderAnalyzeParams param)
        {
            return wrap.Variants.Count;
        }

        protected override EAnalyzeStageStatus execute(int index)
        {
            AnalyzedVariantData avd = m_AnalyzeWrap.Variants[index];

            foreach (var item in avd.ShaderSourceCodes)
            {
                string path;
                if (m_AnalyzeWrap.VariantDirectory.TryGetValue(avd, out path))
                {
                    List<string> lines = new List<string>(32);

                    ProcessStartInfo psi = new ProcessStartInfo(UnityEngine.Application.dataPath + "/../Tools/mali_offline_compiler/malioc.exe", "\"" + path + "/shader" + ShaderAnalyzerUtil.GetFileExtensionByShaderType(item.Key) + "\"");
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
                    process.Exited += new EventHandler(
                        (object o, EventArgs args) =>
                        {
                            avd.CompileResult.Add(item.Key, lines.ToArray());
                        }
                    );
                    process.WaitForExit();

                    process.Dispose();
                }


                
            }

            //TODO 这里要处理进程异常

            return EAnalyzeStageStatus.Success;
        }
    }
}