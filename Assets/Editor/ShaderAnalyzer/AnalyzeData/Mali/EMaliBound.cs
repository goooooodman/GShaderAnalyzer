/** 
 *FileName:       EMaliBound.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    定义MaliGPU的着色器瓶颈类型枚举，因为MaliGPU同时有4个管线(旧GPU没有Varying管线)执行，所以周期最高的便是瓶颈
 *History:        
*/
namespace GShaderAnalyzer
{
    /// <summary>
    /// MaliGPU的着色器瓶颈类型
    /// </summary>
    public enum EMaliBound
    {
        None       = 0,
        Arithmetic = 1 << 0,    // 计算管线
        Load_Store = 1 << 1,    // 读取加载管线
        Varying    = 1 << 2,    // Varying管线
        Texture    = 1 << 3     // 纹理管线
    }
}