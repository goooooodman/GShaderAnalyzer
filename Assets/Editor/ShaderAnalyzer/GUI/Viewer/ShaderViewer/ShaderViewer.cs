/** 
*FileName:       ShaderViewer.cs
*Author:         yinlongfei@hotmail.com
*Date:           2020-06-21
*Description:    着色器列表视图
*History:         
*/
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GShaderAnalyzer
{
    public class ShaderViewer : ShaderAnalyzerViewerBase
    {
        private const float m_MINWIDTH = 100f;
        private List<AnalyzedShaderDataBase> m_List;

        private Dictionary<AnalyzedShaderDataBase, ShaderParem> m_ParamMap;

        private Dictionary<FieldInfo, ShaderAnalyzedDataTooltipAttribute> m_FieldInfoMap;

        private FieldInfo m_CurrentSortField = null;

        private bool m_DescendingOrder = false;

        private class ShaderParem
        {
            public AnalyzedVariantData Variant;
            public int VariantIndex = 0;

            public AnalyzedPassData Pass;
            public int PassIndex = 0;

            public AnalyzedSubshaderData Subshader;
            public int SubshaderIndex = 0;
        }

        protected override void init()
        {
            m_List = new List<AnalyzedShaderDataBase>();
            m_ParamMap = new Dictionary<AnalyzedShaderDataBase, ShaderParem>();

            for (int i = 0; i < m_AnalyzedData.Subshaders.Count; i++)
            {
                AnalyzedSubshaderData subshader = m_AnalyzedData.Subshaders[i];

                for (int j = 0; j < subshader.Passes.Count; j++)
                {
                    AnalyzedPassData pass = subshader.Passes[j];

                    for (int k = 0; k < pass.Variants.Count; k++)
                    {
                        AnalyzedVariantData variant = pass.Variants[k];

                        for (int l = 0; l < variant.Shaders.Count; l++)
                        {
                            AnalyzedShaderDataBase shader = variant.Shaders[l];

                            if (m_FieldInfoMap == null)
                            {
                                m_FieldInfoMap = new Dictionary<FieldInfo, ShaderAnalyzedDataTooltipAttribute>();

                                Type type = shader.GetType();

                                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                                for (int n = 0; n < fields.Length; n++)
                                {
                                    FieldInfo field = fields[n];
                                    ShaderAnalyzedDataTooltipAttribute attribute = field.GetCustomAttribute<ShaderAnalyzedDataTooltipAttribute>();
                                    m_FieldInfoMap.Add(field, attribute);
                                }
                            }

                            m_List.Add(shader);

                            m_ParamMap.Add(shader, new ShaderParem() { Subshader = subshader, SubshaderIndex = i, Pass = pass, PassIndex = j, Variant = variant, VariantIndex = k });
                        }
                    }
                }
            }
        }

        public override void Draw()
        {
            GUITools.BeginContents(GUITools.Styles.WizardBoxStyle);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(m_CurrentSortField != null ? "着色器类型" : (m_DescendingOrder ? "着色器类型 ▼" : "着色器类型 ▲"), GUILayout.MinWidth(m_MINWIDTH)))
            {
                if (m_CurrentSortField != null)
                {
                    m_DescendingOrder = false;
                    m_CurrentSortField = null;
                }
                else
                {
                    m_DescendingOrder = !m_DescendingOrder;
                }

                m_List.Sort(sort);
            }
			GUILayout.Button(new GUIContent("源代码", "点击着色器代码可以复制到剪贴板"), GUILayout.MinWidth(m_MINWIDTH));

            foreach (var item in m_FieldInfoMap)
            {
                bool click = false; 

                GUIContent buttonText;
                if (item.Value != null)
                    buttonText = new GUIContent(item.Value.Abbreviate, item.Value.Tooltip);
                else
                    buttonText = new GUIContent(item.Key.Name);

                if (m_CurrentSortField == item.Key)
                    buttonText.text = buttonText.text + (m_DescendingOrder ? " ▼" : " ▲");

                click = GUILayout.Button(buttonText, GUILayout.MinWidth(m_MINWIDTH));

                if (click)
                {
                    if (m_CurrentSortField != item.Key)
                        m_DescendingOrder = true;
                    else
                        m_DescendingOrder = !m_DescendingOrder;

                    m_CurrentSortField = item.Key;
                    m_List.Sort(sort);
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            for (int i = 0; i < m_List.Count; i++)
            {
                //TODO 这里需要分页，避免着色器过多导致GUI卡顿

                AnalyzedShaderDataBase shader = m_List[i];
                if (!string.IsNullOrEmpty(shader.ERROR))
                {
                    GUILayout.Button(shader.ERROR);
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();

                    ShaderParem param = m_ParamMap[shader];
                    GUILayout.Button(new GUIContent(shader.ShaderType.ToString(), getTooltip(param)), GUILayout.MinWidth(m_MINWIDTH));

                    if (GUILayout.Button(new GUIContent("着色器代码", shader.SourceCode), GUILayout.MinWidth(m_MINWIDTH)))
                    {
                        GUIUtility.systemCopyBuffer = shader.SourceCode;
                    }

                    foreach (var item in m_FieldInfoMap)
                    {
                        FieldInfo field = item.Key;
                        GUILayout.Button(field.GetValue(shader).ToString(), GUILayout.MinWidth(m_MINWIDTH));
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
            GUITools.EndContents();
        }

        private string getTooltip(ShaderParem param)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Subshader ");
            sb.Append(param.SubshaderIndex.ToString());
            sb.Append("\n");
            sb.Append("    Pass ");
            sb.Append(param.PassIndex.ToString());
            sb.Append(" ");
            sb.Append(param.Pass.PassName);
            sb.Append("\n");
            sb.Append("        Variant ");
            sb.Append(param.VariantIndex.ToString());
            sb.Append(" ");
            sb.Append(param.Variant.VariantName);

            return sb.ToString();
        }

        private int sort(AnalyzedShaderDataBase s0, AnalyzedShaderDataBase s1)
        {
            bool greater = false;
            if (m_CurrentSortField == null)
            {
                ShaderParem param0 = m_ParamMap[s0];
                ShaderParem param1 = m_ParamMap[s1];

                if (param0.SubshaderIndex == param1.SubshaderIndex)
                {
                    if (param0.PassIndex == param1.PassIndex)
                    {
                        if (param0.VariantIndex == param1.VariantIndex)
                        {
                            return 0;
                        }
                        else
                        {
                            greater = param0.VariantIndex > param1.VariantIndex;
                        }
                    }
                    else
                    {
                        greater = param0.PassIndex > param1.PassIndex;
                    }
                }
                else
                {
                    greater = param0.SubshaderIndex > param1.SubshaderIndex;
                }
            }
            else
            {
                object value0 = m_CurrentSortField.GetValue(s0);
                object value1 = m_CurrentSortField.GetValue(s1);
                if (m_CurrentSortField.FieldType == typeof(int))
                {
                    int intValue0 = (int)value0;
                    int intValue1 = (int)value1;
                    if (intValue0 == intValue1)
                        return 0;
                    greater = intValue0 > intValue1;
                }
                else if(m_CurrentSortField.FieldType == typeof(float))
                {
                    float floatValue0 = (float)value0;
                    float floatValue1 = (float)value1;
                    if (floatValue0 == floatValue1)
                        return 0;
                    greater = floatValue0 > floatValue1;
                }
                else if(m_CurrentSortField.FieldType == typeof(long))
                {
                    long longValue0 = (long)value0;
                    long longValue1 = (long)value1;
                    if (longValue0 == longValue1)
                        return 0;
                    greater = longValue0 > longValue1;
                }
                else if (m_CurrentSortField.FieldType == typeof(double))
                {
                    double doubleValue0 = (double)value0;
                    double doubleValue1 = (double)value1;
                    if (doubleValue0 == doubleValue1)
                        return 0;
                    greater = doubleValue0 > doubleValue1;
                }
                else if (m_CurrentSortField.FieldType == typeof(short))
                {
                    short shortValue0 = (short)value0;
                    short shortValue1 = (short)value1;
                    if (shortValue0 == shortValue1)
                        return 0;
                    greater = shortValue0 > shortValue1;
                }
                else if (m_CurrentSortField.FieldType == typeof(bool))
                {
                    bool boolValue0 = (bool)value0;
                    bool boolValue1 = (bool)value1;
                    if (boolValue0 == boolValue1)
                        return 0;
                    greater = boolValue0;
                }
                else if (m_CurrentSortField.FieldType.IsEnum)
                {
                    int enumValue0 = (int)value0;
                    int enumValue1 = (int)value1;
                    if (enumValue0 == enumValue1)
                        return 0;
                    greater = enumValue0 > enumValue1;
                }
                else if (m_CurrentSortField.FieldType == typeof(string))
                {
                    string stringValue0 = (string)value0;
                    string stringValue1 = (string)value1;
                    if (string.Compare(stringValue0, stringValue1, StringComparison.Ordinal) == 0)
                        return 0;
                    
                    greater = string.Compare(stringValue0, stringValue1, StringComparison.Ordinal) > 0;
                }
            }

            if (!m_DescendingOrder)
                return greater ? 1 : -1;
            else
                return greater ? - 1 : 1;
        }

        protected override void clear()
        {
            if (m_List != null)
                m_List.Clear();
            m_List = null;

            if (m_ParamMap != null)
                m_ParamMap.Clear();
            m_ParamMap = null;

            if (m_FieldInfoMap != null)
                m_FieldInfoMap.Clear();
            m_FieldInfoMap = null;

            m_CurrentSortField = null;

            m_DescendingOrder = false;
        }
    }
}