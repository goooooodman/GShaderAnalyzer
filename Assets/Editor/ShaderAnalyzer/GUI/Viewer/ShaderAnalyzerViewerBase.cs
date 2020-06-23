/** 
 *FileName:       ShaderAnalyzerViewerBase.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-22
 *Description:    ��ɫ��������ͼ����
 *History:         
*/
namespace GShaderAnalyzer
{
    public abstract class ShaderAnalyzerViewerBase
    {
        protected AnalyzedData m_AnalyzedData;

        public void Init(AnalyzedData data)
        {
            m_AnalyzedData = data;
            init();
        }

        protected abstract void init();

        public void Clear()
        {
            clear();
            m_AnalyzedData = null;
        }

        protected abstract void clear();

        public abstract void Draw();
    }
}