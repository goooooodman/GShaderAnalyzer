/** 
 *FileName:       EShaderType.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    定义着色器类型枚举
 *History:        
*/
namespace GShaderAnalyzer
{
    /// <summary>
    /// 定义着色器类型枚举
    /// </summary>
    public enum EShaderType
    {
        None                    = -1,
        Vertex                  = 0,    // 顶点着色器
        Fragment                = 1,    // 片段着色器
        Compute                 = 2,    // 计算着色器
        Geometry                = 3,    // 几何着色器
        Tessellation_Control    = 4,    // 曲面细分（Hull）
        Tessellation_Evaluation = 5     // 曲面细分（shader）
    }
}