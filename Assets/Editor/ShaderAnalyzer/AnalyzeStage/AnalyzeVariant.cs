/** 
 *FileName:       AnalyzeVariant.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    变体解析阶段
 *History:        
*/
using System.Text;

namespace GShaderAnalyzer
{
    [AnalyzeStage(EAnalyzeStageType.AnalyzeVariant)]
    public sealed class AnalyzeVariant : AnalyzeStageBase
    {
        public static readonly string[] ShaderBeginFlag = new string[] { "#ifdef VERTEX", "#ifdef FRAGMENT" };
        public static readonly string ShaderEndFlag = "#endif";

        private StringBuilder m_TextContainer = new StringBuilder();

        protected override int init(AnalyzeWrap wrap, ShaderAnalyzeParams param)
        {
            return wrap.Variants.Count;
        }

        protected override EAnalyzeStageStatus execute(int index)
        {
            AnalyzedVariantData variant = m_AnalyzeWrap.Variants[index];

            int currentLine = 0;

            bool afterEmpty = false;
            EShaderType currentShaderType = EShaderType.None;

            while (currentLine < variant.SourceCodeLines.Count)
            {
                string line = variant.SourceCodeLines[currentLine++];

                if (string.IsNullOrEmpty(line))
                {
                    afterEmpty = true;
                    continue;
                }
                else
                {
                    if (afterEmpty && line == "//////////////////////////////////////////////////////")
                    {
                        currentShaderType = EShaderType.None;
                        break;
                    }
                    else if (string.IsNullOrEmpty(variant.VariantName))
                    {
                        if (line.StartsWith("Global Keywords: "))
                        {
                            variant.VariantName = line.Replace("Global Keywords: ", string.Empty);
                            continue;
                        }
                    }
                    else if (currentShaderType == EShaderType.None)
                    {
                        for (int i = 0; i < ShaderBeginFlag.Length; i++)
                        {
                            if (line == ShaderBeginFlag[i])
                            {
                                currentShaderType = (EShaderType)i;
                                break;
                            }
                        }
                    }
                    else if (currentShaderType != EShaderType.None)
                    {
                        if (afterEmpty && line == ShaderEndFlag)
                        {
                            variant.ShaderSourceCodes.Add(currentShaderType, m_TextContainer.ToString());
                            m_TextContainer.Clear();
                            currentShaderType = EShaderType.None;
                            continue;
                        }

                        m_TextContainer.AppendLine(line);
                    }

                    afterEmpty = false;
                }
            }

            return EAnalyzeStageStatus.Success;
        }

        protected override void clear()
        {
            m_TextContainer.Clear();
        }
    }
}