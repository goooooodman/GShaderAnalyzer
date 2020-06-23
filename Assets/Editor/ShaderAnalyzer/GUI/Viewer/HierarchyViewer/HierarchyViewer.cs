/** 
*FileName:       HierarchyViewer.cs
*Author:         yinlongfei@hotmail.com
*Date:           2020-06-21
*Description:    层次结构绘制器
*History:         
*/
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GShaderAnalyzer
{

    public class HierarchyViewer : ShaderAnalyzerViewerBase
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        private enum EDataType
        {
            Subshader = 0,
            Pass      = 1,
            Variant   = 2,
            Shader    = 3
        }

        private static Dictionary<EGPUVendorType, ShaderDataDrawerBase> m_ShaderDrawer;
        private static Dictionary<EGPUVendorType, ShaderDataDrawerBase> ShaderDrawer
        {
            get
            {
                if (m_ShaderDrawer == null)
                {
                    m_ShaderDrawer = new Dictionary<EGPUVendorType, ShaderDataDrawerBase>();
                    Assembly assembly = Assembly.GetAssembly(typeof(ShaderAnalyzerWindow));
                    Type[] types = assembly.GetTypes();
                    for (int i = 0; i < types.Length; i++)
                    {
                        Type type = types[i];
                        if (!type.IsSubclassOf(typeof(ShaderDataDrawerBase)))
                            continue;

                        AnalyzeVendorAttribute attribute = type.GetCustomAttribute<AnalyzeVendorAttribute>();
                        if (attribute == null)
                        {
                            Debug.LogError("ShaderDataDrawerBase的派生类" + type.ToString() + "未添加AnalyzeVendorAttribute");
                            continue;
                        }

                        ShaderDataDrawerBase instance = Activator.CreateInstance(type) as ShaderDataDrawerBase;
                        m_ShaderDrawer.Add(attribute.Vendor, instance);
                    }
                }
                return m_ShaderDrawer;
            }
        }

        private Dictionary<AnalyzedBase, bool> m_FolderMap;

        protected override void init()
        {
            m_FolderMap = new Dictionary<AnalyzedBase, bool>();

            for (int i = 0; i < m_AnalyzedData.Subshaders.Count; i++)
            {
                AnalyzedSubshaderData subshader = m_AnalyzedData.Subshaders[i];

                m_FolderMap.Add(subshader, true);

                for (int j = 0; j < subshader.Passes.Count; j++)
                {
                    AnalyzedPassData pass = subshader.Passes[j];

                    m_FolderMap.Add(pass, true);

                    for (int k = 0; k < pass.Variants.Count; k++)
                    {
                        AnalyzedVariantData variant = pass.Variants[k];

                        m_FolderMap.Add(variant, true);

                        for (int l = 0; l < variant.Shaders.Count; l++)
                        {
                            AnalyzedShaderDataBase shader = variant.Shaders[l];

                            m_FolderMap.Add(shader, true);
                        }
                    }
                }
            }
        }

        public override void Draw()
        {
            for (int i = 0; i < m_AnalyzedData.Subshaders.Count; i++)
            {
                AnalyzedSubshaderData subshader = m_AnalyzedData.Subshaders[i];
                
                if (drawHeader(subshader, EDataType.Subshader, i.ToString(), string.Empty))
                {
                    EditorGUILayout.BeginVertical();
                    GUITools.BeginContents(GUITools.Styles.HelpBoxStyle);
                    EditorGUILayout.LabelField("LOD : ", subshader.LOD.ToString());

                    for (int j = 0; j < subshader.Passes.Count; j++)
                    {
                        AnalyzedPassData pass = subshader.Passes[j];
                        
                        if (drawHeader(pass, EDataType.Pass, j.ToString(), pass.PassName))
                        {
                            GUITools.BeginContents(GUITools.Styles.HelpBoxStyle);

                            EditorGUILayout.LabelField("Name : ", pass.PassName);

                            for (int k = 0; k < pass.Variants.Count; k++)
                            {
                                AnalyzedVariantData variant = pass.Variants[k];

                                //TODO 这里需要分页，避免变体过多导致GUI卡顿

                                if (drawHeader(variant, EDataType.Variant, k.ToString(), getShortVariantName(variant.VariantName)))
                                {
                                    GUITools.BeginContents(GUITools.Styles.HelpBoxStyle);
                                    EditorGUILayout.LabelField("Name : ", variant.VariantName);

                                    ShaderDataDrawerBase drawer;
                                    if (!ShaderDrawer.TryGetValue(variant.GPUVendorType, out drawer))
                                    {
                                        Debug.LogError("无法获取到" + variant.GPUVendorType.ToString() + "的绘制器");
                                    }
                                    else
                                    {
                                        for (int l = 0; l < variant.Shaders.Count; l++)
                                        {
                                            AnalyzedShaderDataBase shader = variant.Shaders[l];

                                            if (drawHeader(shader, EDataType.Shader, shader.ShaderType.ToString(), string.Empty))
                                            {
                                                GUITools.BeginContents(GUITools.Styles.HelpBoxStyle);

                                                if (!string.IsNullOrEmpty(shader.ERROR))
                                                    EditorGUILayout.HelpBox(shader.ERROR, MessageType.Error);
                                                else
                                                    drawer.Draw(shader);

                                                GUITools.EndContents();
                                            }
                                        }
                                    }
                                    GUITools.EndContents();
                                }
                            }

                            GUITools.EndContents();
                        }
                    }
                    GUITools.EndContents();
                    EditorGUILayout.EndVertical();
                }
            } 
        }

        private bool drawHeader(AnalyzedBase analyzed, EDataType type, string index, string name)
        {
            string subshaderFolderName = string.Format("{0} {1} {2}", type.ToString(), index, name);
            AnalyzedShaderDataBase shader = analyzed as AnalyzedShaderDataBase;
            GUIContent content = new GUIContent(subshaderFolderName, shader != null ? shader.SourceCode : string.Empty);
            m_FolderMap[analyzed] = GUITools.DrawHeader(content, false, m_FolderMap[analyzed], true);

            return m_FolderMap[analyzed];
        }

        private string getShortVariantName(string variantName)
        {
            if (variantName.Length <= 20)
                return variantName;

            return variantName.Substring(0, 20) + "...";
        }

        protected override void clear()
        {
            if (m_FolderMap != null)
                m_FolderMap.Clear();
            m_FolderMap = null;
        }
    }
}