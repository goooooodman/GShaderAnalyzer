/** 
 *FileName:       GenerateShaderFile.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    生成着色器文件阶段
 *History:        
*/
using System.IO;

namespace GShaderAnalyzer
{
    [AnalyzeStage(EAnalyzeStageType.GenerateShaderFile)]
    public sealed class GenerateShaderFile : AnalyzeStageBase
    {
        protected override int init(AnalyzeWrap wrap, ShaderAnalyzeParams param)
        {
            createFolder(Constant.SAWorkSpace);
            for (int i = 0; i < wrap.Subshaders.Count; i++)
            {
                AnalyzedSubshaderData subshader = wrap.Subshaders[i];

                string subshaderFolder = Constant.SAWorkSpace + "/Subshader_" + i.ToString();

                createFolder(subshaderFolder);
                for (int j = 0; j < subshader.Passes.Count; j++)
                {
                    AnalyzedPassData pass = subshader.Passes[j];

                    string passFolder = subshaderFolder + "/Pass_" + j.ToString();

                    createFolder(passFolder);

                    for (int k = 0; k < pass.Variants.Count; k++)
                    {
                        AnalyzedVariantData variant = pass.Variants[k];

                        string variantFolder = passFolder + "/" + ShaderAnalyzerUtil.ReplaceBadCharOfFileName(variant.VariantName);

                        createFolder(variantFolder);

                        wrap.VariantDirectory.Add(variant, variantFolder);
                    }
                }
            }

            return wrap.Variants.Count;
        }

        private void createFolder(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }

        protected override EAnalyzeStageStatus execute(int index)
        {
            AnalyzedVariantData variant = m_AnalyzeWrap.Variants[index];

            foreach (var item in variant.ShaderSourceCodes)
            {
                string path;
                if (m_AnalyzeWrap.VariantDirectory.TryGetValue(variant, out path))
                {
                    File.WriteAllText(path + "/shader" + ShaderAnalyzerUtil.GetFileExtensionByShaderType(item.Key), item.Value);
                }
            }

            //TODO 这里要处理IO异常

            return EAnalyzeStageStatus.Success;
        }
    }
}