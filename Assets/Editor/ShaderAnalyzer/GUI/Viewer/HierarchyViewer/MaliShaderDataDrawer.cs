/** 
 *FileName:     MaliShaderDataDrawer.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-10 
 *Description:  Mali着色器数据绘制器
 *History: 
*/
using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GShaderAnalyzer
{
    [AnalyzeVendor(EGPUVendorType.Mali)]
    public class MaliShaderDataDrawer : ShaderDataDrawerBase
    {
        public override bool Draw(AnalyzedShaderDataBase data)
        {
            MaliAnalyzedShaderData maliShader = data as MaliAnalyzedShaderData;

            if (maliShader == null)
            {
                Debug.LogError("数据无法转换成 MaliAnalyzedShaderData !");
                return false;
            }

            EditorGUILayout.LabelField("工作寄存器使用情况 : ", maliShader.WorkRegisterUsed.ToString());
            EditorGUILayout.LabelField("Uniform寄存器使用情况 : ", maliShader.UniformRegistersUsed.ToString());
            EditorGUILayout.LabelField("溢出到栈 : ", maliShader.StackSpilling.ToString());

            drawCycles("总指令周期 : ", maliShader.TotalArithmeticCycles, maliShader.TotalLoad_StoreCycles, maliShader.TotalVaryingCycles, maliShader.TotalTextureCycles, maliShader.TotalBound);
            drawCycles("最短路径指令周期 : ", maliShader.ShortestPathArithmeticCycles, maliShader.ShortestPathLoad_StoreCycles, maliShader.ShortestPathVaryingCycles, maliShader.ShortestPathTextureCycles, maliShader.ShortestPathBound);
            drawCycles("最长路径指令周期 : ", maliShader.LongestPathArithmeticCycles, maliShader.LongestPathLoad_StoreCycles, maliShader.LongestPathVaryingCycles, maliShader.LongestPathTextureCycles, maliShader.LongestPathBound);

            EditorGUILayout.LabelField("存在Uniform计算 : ", maliShader.UniformComputation.ToString());

            return true;
        }

        private static void drawCycles(string title, float a, float l, float v, float t, EMaliBound bound)
        {
            EditorGUILayout.LabelField(title);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(50f);

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("算数周期 : ", a.ToString());
            EditorGUILayout.LabelField("加载存储周期 : ", l.ToString());
            if (v >= 0)
                EditorGUILayout.LabelField("Varying周期 : ", v.ToString());
            EditorGUILayout.LabelField("纹理周期 : ", t.ToString());
            EditorGUILayout.LabelField("性能瓶颈 : ", getBoundString(bound));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal(); 
        }

        private static string getBoundString(EMaliBound bound)
        {
            Array values = Enum.GetValues(typeof(EMaliBound));

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < values.Length; i++)
            {
                EMaliBound value = (EMaliBound)values.GetValue(i);

                if (value == EMaliBound.None)
                    continue;

                if ((value & bound) == value)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(value.ToString());
                }
            }
            return sb.ToString();
        }
    }
}