/** 
 *FileName:       EAnalyzeStageStatus.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    定义解析阶段状态枚举
 *History:        
*/
namespace GShaderAnalyzer
{
    /// <summary>
    /// 解析阶段状态
    /// </summary>
    public enum EAnalyzeStageStatus
    {
        Error       = -1,
        Success     = 0,
        Finish      = 1,
        Complete    = 2,
    }
}