/** 
 *FileName:       GenerateMaliAnalyzeData.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    生成解析数据阶段，用于MaliGPU
 *History:        
*/
using System;

namespace GShaderAnalyzer
{
    [AnalyzeStage(EAnalyzeStageType.GenerateAnalyzeData)]
    [AnalyzeVendor(EGPUVendorType.Mali)]
    public sealed class GenerateMaliAnalyzeData : AnalyzeStageBase
    {
        protected override int init(AnalyzeWrap wrap, ShaderAnalyzeParams param)
        {
            return wrap.Variants.Count;
        }

        protected override EAnalyzeStageStatus execute(int index)
        {
            AnalyzedVariantData varint = m_AnalyzeWrap.Variants[index];

            foreach (var item in varint.CompileResult)
            {
                MaliAnalyzedShaderData shaderData = new MaliAnalyzedShaderData();

                shaderData.SourceCode = varint.ShaderSourceCodes[item.Key];
                shaderData.ShaderType = item.Key;

                varint.Shaders.Add(shaderData);

                for (int i = 0; i < item.Value.Length; i++)
                {
                    string line = item.Value[i];

                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    else if (string.IsNullOrEmpty(m_AnalyzeWrap.AnalyzedData.Hardware) && line.StartsWith("Hardware: "))
                    {
                        int spaceIndex = line.LastIndexOf(' ');
                        m_AnalyzeWrap.AnalyzedData.Hardware = line.Substring(spaceIndex + 1, line.Length - spaceIndex - 1);
                    }
                    else if (string.IsNullOrEmpty(m_AnalyzeWrap.AnalyzedData.Driver) && line.StartsWith("Driver: "))
                    {
                        m_AnalyzeWrap.AnalyzedData.Driver = line.Replace("Driver: ", string.Empty);
                    }
                    else if (line.StartsWith("ERROR: "))
                    {
                        shaderData.ERROR = line;
                        break;
                    }
                    else if (line.StartsWith("Work registers: "))
                    {
                        int.TryParse(line.Replace("Work registers: ", string.Empty), out shaderData.WorkRegisterUsed);
                    }
                    else if (line.StartsWith("Uniform registers: "))
                    {
                        int.TryParse(line.Replace("Uniform registers: ", string.Empty), out shaderData.UniformRegistersUsed);
                    }
                    else if (line.StartsWith("Stack spilling: "))
                    {
                        shaderData.StackSpilling = line != "Stack spilling: False";
                    }
                    else if (line.StartsWith("Total instruction cycles:"))
                    {
                        getInstructionNormFormLine(line, "Total instruction cycles:", out shaderData.TotalArithmeticCycles, out shaderData.TotalLoad_StoreCycles, out shaderData.TotalVaryingCycles, out shaderData.TotalTextureCycles, out shaderData.TotalBound);
                    }
                    else if (line.StartsWith("Shortest path cycles:"))
                    {
                        getInstructionNormFormLine(line, "Shortest path cycles:", out shaderData.ShortestPathArithmeticCycles, out shaderData.ShortestPathLoad_StoreCycles, out shaderData.ShortestPathVaryingCycles, out shaderData.ShortestPathTextureCycles, out shaderData.ShortestPathBound);
                    }
                    else if (line.StartsWith("Longest path cycles:"))
                    {
                        getInstructionNormFormLine(line, "Longest path cycles:", out shaderData.LongestPathArithmeticCycles, out shaderData.LongestPathLoad_StoreCycles, out shaderData.LongestPathVaryingCycles, out shaderData.LongestPathTextureCycles, out shaderData.LongestPathBound);
                    }
                    else if (line.StartsWith("Uniform computation: "))
                    {
                        shaderData.UniformComputation = line != "Uniform computation: False";
                    }
                }
            }

            //TODO 这里处理各种异常

            return EAnalyzeStageStatus.Success;
        }

        private static void getInstructionNormFormLine(string line, string title, out float a, out float l, out float v, out float t, out EMaliBound bound)
        {
            line = line.Trim();
            string[] datas = line.Replace(title, string.Empty).Split(new string[] { "  " }, StringSplitOptions.RemoveEmptyEntries);

            string boundString = string.Empty;

            if (datas.Length == 4)
            {
                a = float.Parse(datas[0]);
                l = float.Parse(datas[1]);
                v = -1.0f;
                t = float.Parse(datas[2]);
                boundString = datas[3];
            }
            else
            {
                a = float.Parse(datas[0]);
                l = float.Parse(datas[1]);
                v = float.Parse(datas[2]);
                t = float.Parse(datas[3]);
                boundString = datas[4];
            }

            bound = EMaliBound.None;

            string[] bounds = boundString.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < bounds.Length; i++)
            {
                string b = bounds[i].Trim();
                switch (b)
                {
                    case "A":
                        bound |= EMaliBound.Arithmetic;
                        break;
                    case "LS":
                        bound |= EMaliBound.Load_Store;
                        break;
                    case "V":
                        bound |= EMaliBound.Varying;
                        break;
                    case "T":
                        bound |= EMaliBound.Texture;
                        break;
                }
            }
        }
    }
}