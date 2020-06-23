/** 
 *FileName:       ShaderAnalyzerWindow.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-23
 *Description:    编辑器窗口
 *History:         
*/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GShaderAnalyzer
{
    public sealed class ShaderAnalyzerWindow : EditorWindow
    {
        /// <summary>
        /// 要解析的Shader
        /// </summary>
        private Shader m_CurrentShader = null;

        /// <summary>
        /// 是否编译成vulkan着色器
        /// </summary>
        private EAPIType m_APIType = EAPIType.OpenGLES;
        /// <summary>
        /// 是否跳过未使用的变体
        /// </summary>
        private bool m_SkipUnused = false;

        private AnalyzedData m_AnalyzedData;

        private GPUVendors m_GPUVendors;

        private string[] m_Vendors;
        private string[] m_ModelNames;
        private int m_VendorsSelected = 0;

        private int m_GPUModelsSelected = 0;

        private GPUVendor m_CurrentVendor = null;

        private Vector2 m_ScrollPosition = Vector2.zero;

        private bool m_SettingsFolderOpen = true;

        private EAnalyzedDataViewType m_AnalyzedDataDrawType = EAnalyzedDataViewType.Hierarchy;

        private static Dictionary<EAnalyzedDataViewType, ShaderAnalyzerViewerBase> m_ViewerMap = null;
        private static Dictionary<EAnalyzedDataViewType, ShaderAnalyzerViewerBase> ViewerMap
        {
            get
            {
                if (m_ViewerMap == null)
                {
                    m_ViewerMap = new Dictionary<EAnalyzedDataViewType, ShaderAnalyzerViewerBase>();
                    m_ViewerMap.Add(EAnalyzedDataViewType.Hierarchy, new HierarchyViewer());
                    m_ViewerMap.Add(EAnalyzedDataViewType.ShaderList, new ShaderViewer());
                }

                return m_ViewerMap;
            }
        }

        [MenuItem("ShaderTest/AnalyzeShaderWindow")]
        public static void OpenWindow()
        {
            ShaderAnalyzerWindow builderWindow = GetWindow<ShaderAnalyzerWindow>(false, "ShaderAnalyzer", true);
            builderWindow.minSize = new Vector2(800f, 800f);
            builderWindow.position = new Rect(0, 0, builderWindow.minSize.x, builderWindow.minSize.y);
            builderWindow.titleContent = new GUIContent("ShaderAnalyzer");
            builderWindow.Show();
        }

        private void OnEnable()
        {
            m_GPUVendors = GPUVendors.Load();

            initVendors();
        }

        private void initVendors()
        {
            List<string> vendors = new List<string>();
            for (int i = 0; i < m_GPUVendors.Vendors.Length; i++)
            {
                GPUVendor vendor = m_GPUVendors.Vendors[i];
                if ((vendor.CompatibleAPI & m_APIType) == m_APIType)
                {
                    vendors.Add(vendor.VendorType.ToString());
                }
            }

            m_Vendors = vendors.ToArray();

            m_VendorsSelected = 0;

            initVendor();
        }

        private void initVendor()
        {
            string vendorName = m_Vendors[m_VendorsSelected];
            for (int i = 0; i < m_GPUVendors.Vendors.Length; i++)
            {
                GPUVendor vendor = m_GPUVendors.Vendors[i];
                if (vendor.VendorType.ToString() == vendorName)
                {
                    m_CurrentVendor = vendor;

                    m_ModelNames = new string[m_CurrentVendor.Models.Length];
                    for (int j = 0; j < m_CurrentVendor.Models.Length; j++)
                    {
                        m_ModelNames[i] = m_CurrentVendor.Models[i].ModelName;
                    }
                    break;
                }
            }

            m_GPUModelsSelected = 0;
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition, false, true);
            m_SettingsFolderOpen =  GUITools.DrawHeader("设置", false, m_SettingsFolderOpen, true);
            if (m_SettingsFolderOpen)
            {
                GUITools.BeginContents(GUITools.Styles.WizardBoxStyle);

                EditorGUI.BeginDisabledGroup(ShaderAnalyzer.Instance.CurrentStageType != EAnalyzeStageType.None);
                m_CurrentShader = EditorGUILayout.ObjectField(new GUIContent("Shader"), m_CurrentShader, typeof(Shader), false) as Shader;
                m_SkipUnused = EditorGUILayout.Toggle(new GUIContent("是否跳过当前场景未使用的变体", ""), m_SkipUnused);
                EditorGUI.BeginChangeCheck();
                m_APIType = (EAPIType)EditorGUILayout.EnumPopup(new GUIContent("要编译的着色器语言类型"), m_APIType,
                    (Enum index) =>
                    {
                        bool show = true;
                        switch ((EAPIType)index)
                        {
                            case EAPIType.None:
                            case EAPIType.All:
                            case EAPIType.Vulkan:
                                show = false;
                                break;
                        }

                        return show;
                    },
                    false
                    );
                if (EditorGUI.EndChangeCheck())
                    initVendors();

                EditorGUI.BeginChangeCheck();
                m_VendorsSelected = EditorGUILayout.Popup(new GUIContent("GPU厂商"), m_VendorsSelected, m_Vendors);
                if (EditorGUI.EndChangeCheck())
                    initVendor();

                m_GPUModelsSelected = EditorGUILayout.Popup(new GUIContent("目标GPU模型"), m_GPUModelsSelected, m_ModelNames);

                EditorGUI.BeginDisabledGroup(m_CurrentShader == null);

                if (GUILayout.Button("解析"))
                {
                    if (m_CurrentVendor.VendorType == EGPUVendorType.Mali)
                    {
                        if (string.IsNullOrEmpty(Settings.MaliOfflineCompilePath))
                        {
                            Debug.LogError("请设置Mali离线编译器路径!");
                            return;
                        }
                    }
                    ShaderAnalyzer.Instance.Analyze(this.getAnalyzeParams(), analyzeCallback);
                    GUITools.ClearContentDepth();
                    GUIUtility.ExitGUI();
                }

                EditorGUI.EndDisabledGroup();
                EditorGUI.EndDisabledGroup();

                GUITools.EndContents();
            }

            if (m_AnalyzedData != null)
            {
                EditorGUILayout.Space();

                EditorGUI.BeginChangeCheck();

                m_AnalyzedDataDrawType = (EAnalyzedDataViewType)EditorGUILayout.EnumPopup(m_AnalyzedDataDrawType);
                if (EditorGUI.EndChangeCheck())
                    ViewerMap[m_AnalyzedDataDrawType].Init(m_AnalyzedData);

                EditorGUILayout.LabelField("解析数据");

                GUITools.BeginContents(GUITools.Styles.WizardBoxStyle);
                EditorGUILayout.BeginVertical();

                EditorGUILayout.LabelField("GPU信息 : ", m_AnalyzedData.GPUModelInfo.ModelName);
                EditorGUILayout.LabelField("硬件信息 : ", m_AnalyzedData.Hardware);
                EditorGUILayout.LabelField("驱动信息 : ", m_AnalyzedData.Driver);

                EditorGUILayout.EndVertical();
                GUITools.EndContents();
                ViewerMap[m_AnalyzedDataDrawType].Draw();
            }
            GUILayout.EndScrollView();
        }

        private void analyzeCallback (AnalyzedData data)
        {
            m_AnalyzedData = data;
            ViewerMap[m_AnalyzedDataDrawType].Init(m_AnalyzedData);
        }

        private ShaderAnalyzeParams getAnalyzeParams()
        {
            ShaderAnalyzeParams param = new ShaderAnalyzeParams()
            {
                Shader = m_CurrentShader,
                APIType = m_APIType,
                SkipUnusedVariant = m_SkipUnused,
                Vendor = m_CurrentVendor != null ? m_CurrentVendor.VendorType : EGPUVendorType.Mali,
                GPUModel = m_CurrentVendor != null ? m_CurrentVendor.Models[m_GPUModelsSelected] : null
            };

            return param;
        }

        private void OnDestroy()
        {
            foreach (var item in ViewerMap)
            {
                item.Value.Clear();
            }

            
            m_CurrentShader = null;

            m_APIType = EAPIType.OpenGLES;
        
            m_SkipUnused = false;

            m_AnalyzedData = null;

            m_GPUVendors = null;

            m_Vendors = null;
            m_ModelNames = null;
            m_VendorsSelected = 0;

            m_GPUModelsSelected = 0;

            m_CurrentVendor = null;

            m_ScrollPosition = Vector2.zero;

            m_SettingsFolderOpen = true;

            m_AnalyzedDataDrawType = EAnalyzedDataViewType.Hierarchy;
        }
    }
}