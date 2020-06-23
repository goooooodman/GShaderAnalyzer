/** 
 *FileName:       CompilingTargetShaderSource.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    编译为目标着色器阶段，此阶段将IDE设置为Bridge并调用Unity内置函数编译着色器，并等待Bridge结束
 *History:        
*/
using System;
using System.Diagnostics;
using System.Threading;
using UnityEditor.Rendering;

namespace GShaderAnalyzer
{
    [AnalyzeStage(EAnalyzeStageType.CompilingTargetShader)]
    public sealed class CompilingTargetShaderSource : AnalyzeStageBase
    {
        private enum EBridgeResult
        {
            CantFoundBridgeError = -1,
            None                 = 0,
            Finish               = 1
        }
        private static EBridgeResult m_BridgeResult = EBridgeResult.None;

        private static object m_StepLocker = new object();

        protected override int init(AnalyzeWrap wrap, ShaderAnalyzeParams param)
        {
            ShaderAnalyzerUtil.ChangeIDEToBridge();

            ShaderCompilerPlatform platform = ShaderCompilerPlatform.GLES3x;
            switch (param.APIType)
            {
                case EAPIType.OpenGLES:
                    platform = ShaderCompilerPlatform.GLES3x;
                    break;
                case EAPIType.Vulkan:
                    platform = ShaderCompilerPlatform.Vulkan;
                    break;
            }

            ShaderAnalyzerUtil.OpenCompiledShader.Invoke(null, new object[] { param.Shader, 3, 1 << (int)platform, !param.SkipUnusedVariant });

            ShaderAnalyzerUtil.RevertIDESettings();

            Thread thread = new Thread(new ThreadStart(supervisor));
            thread.Start();

            return InitUnknown;
        }

        protected override EAnalyzeStageStatus execute(int index)
        {
            EAnalyzeStageStatus status = EAnalyzeStageStatus.Success;
            switch (m_BridgeResult)
            {
                case EBridgeResult.CantFoundBridgeError:
                    status = EAnalyzeStageStatus.Error;
                    break;
                case EBridgeResult.Finish:
                    status = EAnalyzeStageStatus.Finish;
                    break;
            }

            return status;
        }

        protected override void clear()
        {
            m_BridgeResult = EBridgeResult.None;
        }

        /// <summary>
        /// 监控Bridge进程
        /// </summary>
        private static void supervisor()
        {
            int count = 0;
            while (count < 1000)
            {
                Process[] pro = Process.GetProcessesByName(Constant.BridgeAppProcessName);
                if (pro != null && pro.Length > 0)
                {
                    for (int i = 0; i < pro.Length; i++)
                    {
                        Process process = pro[i];
                        process.EnableRaisingEvents = true;
                        process.Exited += new EventHandler(
                            (object o, EventArgs args) =>
                            {
                                lock (m_StepLocker)
                                {
                                    m_BridgeResult = EBridgeResult.Finish;
                                }
                            }
                        );
                    }
                    break;
                }

                count++;
            }
            if (count == 1000)
            {
                lock (m_StepLocker)
                {
                    m_BridgeResult = EBridgeResult.CantFoundBridgeError;
                }
            }
        }
    }
}