/** 
 *FileName:     ShaderDataDrawBase.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-10 
 *Description:  着色器数据绘制器基类
 *History: 
*/

namespace GShaderAnalyzer
{
    public abstract class ShaderDataDrawerBase
    {
        /// <summary>
        /// 绘制Shader数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否绘制成功</returns>
        public abstract bool Draw(AnalyzedShaderDataBase data);
    }
}