/** 
 *FileName:       MaliAnalyzedShaderData.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    MaliGPU的着色器解析数据
 *History:        
*/
using System;

namespace GShaderAnalyzer
{
    [Serializable]
    [AnalyzeVendor(EGPUVendorType.Mali)]
    public sealed class MaliAnalyzedShaderData : AnalyzedShaderDataBase
    {
        /// <summary>
        /// 工作寄存器使用情况
        /// </summary>
        [ShaderAnalyzedDataTooltip("WRU", "工作寄存器使用情况")]
        public int WorkRegisterUsed = 0;
        /// <summary>
        /// Uniform寄存器使用情况
        /// </summary>
        [ShaderAnalyzedDataTooltip("URU", "Uniform寄存器使用情况")]
        public int UniformRegistersUsed = 0;
        /// <summary>
        /// 溢出到堆栈
        /// </summary>
        [ShaderAnalyzedDataTooltip("SS", "溢出到堆栈")]
        public bool StackSpilling = false;

        /// <summary>
        /// 总算数指令周期
        /// </summary>
        [ShaderAnalyzedDataTooltip("TAC", "总算数指令周期")]
        public float TotalArithmeticCycles;
        /// <summary>
        /// 总读取写入指令周期
        /// </summary>
        [ShaderAnalyzedDataTooltip("TLC", "总读取写入指令周期")]
        public float TotalLoad_StoreCycles;
        /// <summary>
        /// 总Varying指令周期
        /// </summary>
        [ShaderAnalyzedDataTooltip("TVC", "总Varying指令周期")]
        public float TotalVaryingCycles;
        /// <summary>
        /// 总纹理读取指令周期
        /// </summary>
        [ShaderAnalyzedDataTooltip("TTC", "总纹理读取指令周期")]
        public float TotalTextureCycles;
        /// <summary>
        /// 总着色器瓶颈
        /// </summary>
        [ShaderAnalyzedDataTooltip("TB", "总着色器瓶颈")]
        public EMaliBound TotalBound = EMaliBound.None;

        /// <summary>
        /// 最短路径算数指令周期
        /// </summary>
        [ShaderAnalyzedDataTooltip("SAC", "最短路径算数指令周期")]
        public float ShortestPathArithmeticCycles;
        /// <summary>
        /// 最短路径读取写入指令周期
        /// </summary>
        [ShaderAnalyzedDataTooltip("SLC", "最短路径读取写入指令周期")]
        public float ShortestPathLoad_StoreCycles;
        /// <summary>
        /// 最短路径Varying指令周期
        /// </summary>
        [ShaderAnalyzedDataTooltip("SVC", "最短路径Varying指令周期")]
        public float ShortestPathVaryingCycles;
        /// <summary>
        /// 最短路径纹理读取指令周期
        /// </summary>
        [ShaderAnalyzedDataTooltip("STC", "最短路径纹理读取指令周期")]
        public float ShortestPathTextureCycles;
        /// <summary>
        /// 最短路径着色器瓶颈
        /// </summary>
        [ShaderAnalyzedDataTooltip("SB", "最短路径着色器瓶颈")]
        public EMaliBound ShortestPathBound = EMaliBound.None;

        /// <summary>
        /// 最长路径算数指令周期
        /// </summary>
        [ShaderAnalyzedDataTooltip("LAC", "最长路径算数指令周期")]
        public float LongestPathArithmeticCycles;
        /// <summary>
        /// 最长路径读取写入指令周期
        /// </summary>
        [ShaderAnalyzedDataTooltip("LLC", "最长路径读取写入指令周期")]
        public float LongestPathLoad_StoreCycles;
        /// <summary>
        /// 最长路径Varying指令周期
        /// </summary>
        [ShaderAnalyzedDataTooltip("LVC", "最长路径Varying指令周期")]
        public float LongestPathVaryingCycles;
        /// <summary>
        /// 最长路径纹理读取指令周期
        /// </summary>
        [ShaderAnalyzedDataTooltip("LTC", "最长路径纹理读取指令周期")]
        public float LongestPathTextureCycles;
        /// <summary>
        /// 最长路径着色器瓶颈
        /// </summary>
        [ShaderAnalyzedDataTooltip("LB", "最长路径着色器瓶颈")]
        public EMaliBound LongestPathBound = EMaliBound.None;

        /// <summary>
        /// 包含常量计算
        /// </summary>
        [ShaderAnalyzedDataTooltip("UC", "包含常量计算")]
        public bool UniformComputation = false;
    }
}