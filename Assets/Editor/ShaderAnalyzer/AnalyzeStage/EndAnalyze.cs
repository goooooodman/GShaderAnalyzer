/** 
 *FileName:       EndAnalyze.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    完成解析阶段，此阶段保存解析缓存
 *History:        
*/
using System.IO;
using System.Text;
using UnityEngine;

namespace GShaderAnalyzer
{
    [AnalyzeStage(EAnalyzeStageType.EndAnalyze)]
    public class EndAnalyze : AnalyzeStageBase
    {
        protected override int init(AnalyzeWrap wrap, ShaderAnalyzeParams param)
        {
            if (Directory.Exists(Constant.SAWorkSpace))
                Directory.Delete(Constant.SAWorkSpace, true);

            string json = JsonUtility.ToJson(m_AnalyzeWrap.AnalyzedData, true);

            using (StreamWriter sw = new StreamWriter(wrap.AnalyzedDataCacheFilePath))
            {
                sw.WriteLine(wrap.Head.Head);
                sw.WriteLine(json);
            }
            return InitComplete;
        }
    }
}