/** 
 *FileName:       EAPIType.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    定义渲染API枚举
 *History:        
*/
namespace GShaderAnalyzer
{
    /// <summary>
    /// 渲染API类型
    /// </summary>
    public enum EAPIType
    {
        None     = 0,
        OpenGLES = 1 << 1,
        Vulkan   = 1 << 2,
        All      = ~None
    }
}
