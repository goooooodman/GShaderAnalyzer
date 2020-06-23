/** 
 *FileName:       EGPUVendorType.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    定义GPU厂商类型枚举
 *History:        
*/
namespace GShaderAnalyzer
{
    /// <summary>
    /// GPU厂商类型
    /// </summary>
    public enum EGPUVendorType
    {
        None    = 0,
        Mali    = 1 << 0,
        PowerVR = 1 << 1,
        //Adreno  = 2 << 2,
        All     = ~None
    }
}