/** 
 *FileName:       AnalyzeSubshader.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    Subshader解析阶段
 *History:        
*/
namespace GShaderAnalyzer
{
    [AnalyzeStage(EAnalyzeStageType.AnalyzeSubshader)]
    public sealed class AnalyzeSubshader : AnalyzeStageBase
    {
        protected override int init(AnalyzeWrap wrap, ShaderAnalyzeParams param)
        {
            return wrap.Subshaders.Count;
        }

        protected override EAnalyzeStageStatus execute(int index)
        {
            AnalyzedSubshaderData subshader = m_AnalyzeWrap.Subshaders[index];

            int currentLine = 0;

            while (currentLine < subshader.SourceCodeLines.Count)
            {
                string line = subshader.SourceCodeLines[currentLine++];

                if (line.StartsWith(" LOD "))
                {
                    if (!int.TryParse(line.Substring(5, line.Length - 5), out subshader.LOD))
                        return EAnalyzeStageStatus.Error;
                    else
                        break;
                }
            }

            return EAnalyzeStageStatus.Success;
        }
    }
}