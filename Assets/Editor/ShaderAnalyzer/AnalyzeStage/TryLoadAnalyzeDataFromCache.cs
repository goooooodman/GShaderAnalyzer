/** 
 *FileName:       TryLoadAnalyzeDataFromCache.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    尝试读取缓存阶段
 *History:        
*/
using System.IO;

namespace GShaderAnalyzer
{
    [AnalyzeStage(EAnalyzeStageType.TryLoadAnalyzedDataFromCache)]
    public sealed class TryLoadAnalyzeDataFromCache : AnalyzeStageBase
    {
        private string m_CompiledShaderPath = string.Empty;
        private string m_AnalyzedDataFilePath = string.Empty;
        private string m_MD5 = string.Empty;

        protected override int init(AnalyzeWrap wrap, ShaderAnalyzeParams param)
        {
            m_CompiledShaderPath = ShaderAnalyzerUtil.GetCompiledShaderPath(param);
            m_AnalyzedDataFilePath = ShaderAnalyzerUtil.GetChacheFileName(param);
            m_MD5 = ShaderAnalyzerUtil.GenerateMD5(m_CompiledShaderPath);

            if (!Directory.Exists(Constant.CacheFolder)) 
            {
                Directory.CreateDirectory(Constant.CacheFolder);
                return create();
            }
            else
            {

                if (!File.Exists(m_AnalyzedDataFilePath))
                {
                    return create();
                }
                else
                {
                    CacheHead head = ShaderAnalyzerUtil.GetHeadFromCache(m_AnalyzedDataFilePath);
                    if (head == null || Constant.Version != head.Version || head.MD5 != m_MD5)
                        return create();
                    else
                        return loadFromCache(head);
                }
            }
        }

        /// <summary>
        /// 从缓存中读取解析数据
        /// </summary>
        /// <param name="head"></param>
        private int loadFromCache(CacheHead head)
        {
            string json = File.ReadAllText(m_AnalyzedDataFilePath).Replace(head.Head, string.Empty);
            m_AnalyzeWrap.AnalyzedData = UnityEngine.JsonUtility.FromJson<AnalyzedData>(json);
             
            return InitComplete;
        }

        /// <summary>
        /// 创建解析数据准备解析
        /// </summary>
        private int create()
        {
            m_AnalyzeWrap.AnalyzedData = new AnalyzedData();
            m_AnalyzeWrap.AnalyzedData.GPUModelInfo = m_Params.GPUModel;
            m_AnalyzeWrap.CompiledShaderFilePath = m_CompiledShaderPath;
            m_AnalyzeWrap.AnalyzedDataCacheFilePath = m_AnalyzedDataFilePath;
            m_AnalyzeWrap.Head = new CacheHead(Constant.Version, m_MD5);

            if (File.Exists(m_AnalyzedDataFilePath))
                File.Delete(m_AnalyzedDataFilePath);

            return InitFinish;
        }

        protected override void clear()
        {
            m_CompiledShaderPath = string.Empty;
            m_AnalyzedDataFilePath = string.Empty;
            m_MD5 = string.Empty;
        }
    }
}