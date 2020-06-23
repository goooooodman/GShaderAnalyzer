/** 
 *FileName:       AnalyzePass.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    解析Pass阶段
 *History:        
*/
namespace GShaderAnalyzer
{
    [AnalyzeStage(EAnalyzeStageType.AnalyzePass)]
    public sealed class AnalyzePass : AnalyzeStageBase
    {
        protected override int init(AnalyzeWrap wrap, ShaderAnalyzeParams param)
        {
            return wrap.Passes.Count;
        }

        protected override EAnalyzeStageStatus execute(int index)
        {
            AnalyzedPassData pass = m_AnalyzeWrap.Passes[index];

            int currentLine = 0;

            while (currentLine < pass.SourceCodeLines.Count)
            {
                string line = pass.SourceCodeLines[currentLine++];

                if (line.StartsWith("  Name "))
                {
                    pass.PassName = line.Substring(7, line.Length - 7);
                    break;
                }
            }

            return EAnalyzeStageStatus.Success;
        }
    }
}