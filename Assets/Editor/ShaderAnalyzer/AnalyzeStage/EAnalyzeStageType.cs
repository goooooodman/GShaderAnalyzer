/** 
 *FileName:       EAnalyzeStageType.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    定义解析阶段类型枚举
 *History:        
*/
namespace GShaderAnalyzer
{
    /// <summary>
    /// 解析阶段类型
    /// </summary>
    public enum EAnalyzeStageType
    {
        None                             = 0,         // 未开始解析
        CompilingTargetShader            = 1,         // 正在编译成目标着色器
        TryLoadAnalyzedDataFromCache     = 2,         // 尝试在缓存中读取解析数据
        ReadShaderSource                 = 3,         // 读取着色器代码
        AnalyzeSubshader                 = 4,         // 解析Subshader
        AnalyzePass                      = 5,         // 解析Pass
        AnalyzeVariant                   = 6,         // 解析Variant
        GenerateShaderFile               = 7,         // 生成着色器文件
        VendorOfflineCompile             = 8,         // 调用厂商编译器编译并生成报告
        GenerateAnalyzeData              = 9,         // 生成解析数据
        EndAnalyze                       = 10,        // 结束解析
    }
}