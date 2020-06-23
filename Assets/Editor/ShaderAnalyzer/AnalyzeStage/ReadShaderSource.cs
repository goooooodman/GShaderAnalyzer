/** 
 *FileName:       ReadShaderSource.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    读取着色器代码阶段
 *History:        
*/
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace GShaderAnalyzer
{
    [AnalyzeStage(EAnalyzeStageType.ReadShaderSource)]
    public sealed class ReadShaderSource : AnalyzeStageBase
    {
        private enum EBraceRegionType
        {
            ShaderMain = 0,
            Properties = 1,
            SubShader  = 2,
            Pass       = 3,
            Variant    = 4,
            Constant   = 5,
            Function   = 6,
        }

        private LinkedList<EBraceRegionType> m_BraceStack = new LinkedList<EBraceRegionType>();

        protected override int init(AnalyzeWrap wrap, ShaderAnalyzeParams param)
        { 
            int count = 0;
            bool shaderDisassembly = false;

            AnalyzedSubshaderData currentSubshader = null;
            AnalyzedPassData currentPass = null;
            AnalyzedVariantData currentVariant = null;

            using (StreamReader sr = new StreamReader(wrap.CompiledShaderFilePath))
            {
                string line = string.Empty;
                int lineCount = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    lineCount++;
                    //wrap.SourceCode.Add(line);
                    if (Regex.IsMatch(line, "^Shader \"\\S+\" {$"))
                    {
                        m_BraceStack.AddLast(EBraceRegionType.ShaderMain);
                        currentSubshader = new AnalyzedSubshaderData();
                        wrap.Subshaders.Add(currentSubshader);
                        wrap.AnalyzedData.Subshaders.Add(currentSubshader);
                    }
                    else if (line == "Properties {")
                    {
                        m_BraceStack.AddLast(EBraceRegionType.Properties);
                    }
                    else if (line == "SubShader { ")
                    {
                        m_BraceStack.AddLast(EBraceRegionType.SubShader);
                    }
                    else if (line == " Pass {")
                    {
                        m_BraceStack.AddLast(EBraceRegionType.Pass);

                        currentPass = new AnalyzedPassData();
                        currentSubshader.Passes.Add(currentPass);
                        wrap.Passes.Add(currentPass);
                    }
                    else if (line.StartsWith("Global Keywords: ")) 
                    {
                        m_BraceStack.AddLast(EBraceRegionType.Variant);

                        currentVariant = new AnalyzedVariantData();
                        currentPass.Variants.Add(currentVariant);
                        wrap.Variants.Add(currentVariant); 
                        count++; 
                    }
                    else if (line.StartsWith("Constant Buffer "))
                    {
                        m_BraceStack.AddLast(EBraceRegionType.Constant);
                    }
                    else if (line == "{")
                    {
                        m_BraceStack.AddLast(EBraceRegionType.Function);
                    }
                    else if (line == "}")
                    {
                        m_BraceStack.RemoveLast();
                    }
                    else if (line == "Shader Disassembly:")
                    {
                        if (shaderDisassembly)
                            m_BraceStack.RemoveLast();

                        shaderDisassembly = !shaderDisassembly;
                    }

                    if (m_BraceStack.Count > 0)
                    {
                        switch (m_BraceStack.Last.Value)
                        {
                            case EBraceRegionType.SubShader:
                                currentSubshader.SourceCodeLines.Add(line);
                                break;
                            case EBraceRegionType.Pass:
                                currentSubshader.SourceCodeLines.Add(line);
                                currentPass.SourceCodeLines.Add(line);
                                break;
                            case EBraceRegionType.Variant:
                            case EBraceRegionType.Constant:
                            case EBraceRegionType.Function:
                                currentSubshader.SourceCodeLines.Add(line);
                                currentPass.SourceCodeLines.Add(line);
                                currentVariant.SourceCodeLines.Add(line);
                                break;
                        }
                    }
                }
            }

            //TODO 这里可以改为逐帧读取
            return InitFinish;
        }

        protected override void clear()
        {
            m_BraceStack.Clear();
        }
    }
}