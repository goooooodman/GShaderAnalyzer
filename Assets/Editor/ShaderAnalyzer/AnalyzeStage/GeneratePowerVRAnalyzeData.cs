/** 
 *FileName:       GeneratePowerVRAnalyzeData.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    生成解析数据阶段，用于PowerVRGPU
 *History:        
*/
namespace GShaderAnalyzer
{
    [AnalyzeStage(EAnalyzeStageType.GenerateAnalyzeData)]
    [AnalyzeVendor(EGPUVendorType.PowerVR)]
    public sealed class GeneratePowerVRAnalyzeData : AnalyzeStageBase
    {
        protected override int init(AnalyzeWrap wrap, ShaderAnalyzeParams param)
        {
            return wrap.Variants.Count;
        }
        protected override EAnalyzeStageStatus execute(int index)
        {
            AnalyzedVariantData variant = m_AnalyzeWrap.Variants[index];

            //TODO 解析PowerVR的结果

            return EAnalyzeStageStatus.Success;
        }
    }
}