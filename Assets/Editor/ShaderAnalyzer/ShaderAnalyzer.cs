/** 
 *FileName:     ShaderAnalyzer.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-21 
 *Description:  着色器解析类
 *History: 
*/
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GShaderAnalyzer
{
    public sealed class ShaderAnalyzer
    {
        /// <summary>
        /// 解析完成回调
        /// </summary>
        /// <param name="data"></param>
        public delegate void AnalyzeFinish(AnalyzedData data);
        
        private EAnalyzeStageType m_CurrentStageType = EAnalyzeStageType.None;
        /// <summary>
        /// 当前解析阶段类型
        /// </summary>
        public EAnalyzeStageType CurrentStageType { get; }
        /// <summary>
        /// 当前阶段
        /// </summary>
        private AnalyzeStageBase m_CurrentStage = null;
        /// <summary>
        /// 参数
        /// </summary>
        private ShaderAnalyzeParams m_Params;
        /// <summary>
        /// 解析中间数据包
        /// </summary>
        private AnalyzeWrap m_AnalyzeWrap;

        /// <summary>
        /// 解析完成回调函数
        /// </summary>
        private AnalyzeFinish m_AnalyzeFinish = null;

        private static Dictionary<EAnalyzeStageType, Dictionary<EGPUVendorType, AnalyzeStageBase>> m_AnalyzedStageMap;
        /// <summary>
        /// 解析阶段类型
        /// </summary>
        private static Dictionary<EAnalyzeStageType, Dictionary<EGPUVendorType, AnalyzeStageBase>> AnalyzedStageMap
        {
            get
            {
                if (m_AnalyzedStageMap == null)
                {
                    m_AnalyzedStageMap = new Dictionary<EAnalyzeStageType, Dictionary<EGPUVendorType, AnalyzeStageBase>>();

                    Assembly assembly = Assembly.GetAssembly(typeof(ShaderAnalyzer));
                    Type[] types = assembly.GetTypes();
                    for (int i = 0; i < types.Length; i++)
                    {
                        Type type = types[i];

                        if (!type.IsSubclassOf(typeof(AnalyzeStageBase)))
                            continue;

                        AnalyzeStageAttribute attribute = type.GetCustomAttribute<AnalyzeStageAttribute>(false);

                        if (attribute == null)
                            continue;

                        Dictionary<EGPUVendorType, AnalyzeStageBase> stageDic;
                        if (!m_AnalyzedStageMap.TryGetValue(attribute.Stage, out stageDic))
                        {
                            stageDic = new Dictionary<EGPUVendorType, AnalyzeStageBase>();
                            m_AnalyzedStageMap.Add(attribute.Stage, stageDic);
                        }

                        
                        AnalyzeStageBase instance = Activator.CreateInstance(type) as AnalyzeStageBase;
                        AnalyzeVendorAttribute vendor = type.GetCustomAttribute<AnalyzeVendorAttribute>();
                        EGPUVendorType vendorType = EGPUVendorType.None;
                        if (vendor != null)
                            vendorType = vendor.Vendor;
                        stageDic.Add(vendorType, instance);
                    }
                }

                return m_AnalyzedStageMap;
            }
        }

        private static ShaderAnalyzer m_Instance = null;
        public static ShaderAnalyzer Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new ShaderAnalyzer();

                return m_Instance;
            }
        }

        private ShaderAnalyzer() { }

        /// <summary>
        /// 解析着色器
        /// </summary>
        /// <param name="param">解析参数</param>
        /// <param name="callback">解析完成回调</param>
        public void Analyze(ShaderAnalyzeParams param, AnalyzeFinish callback)
        {
            if (m_CurrentStageType != EAnalyzeStageType.None)
            {
                Debug.LogError("当前正在进行解析，请耐心等待。");
                return;
            }
            if (param.Shader == null)
            {
                Debug.LogError("请指定要解析的着色器。");
                return;
            }
            if (callback == null)
            {
                Debug.LogError("请指定解析完整的回调函数。");
                return;
            }


            m_Params = param;
            m_AnalyzeFinish = callback;

            m_AnalyzeWrap = new AnalyzeWrap();

            executeNext();

            EditorApplication.update += update;
        }

        private void executeNext()
        {
            if (m_CurrentStage != null)
                m_CurrentStage.Clear();

            m_CurrentStageType = m_CurrentStageType + 1;

            if (m_CurrentStageType > EAnalyzeStageType.EndAnalyze)
            {
                m_AnalyzeFinish?.Invoke(m_AnalyzeWrap.AnalyzedData);
                Clear();
                return;
            }

            EditorUtility.DisplayProgressBar(m_CurrentStageType.ToString(), m_CurrentStageType.ToString(), (float)m_CurrentStageType / (float)EAnalyzeStageType.EndAnalyze);

            m_CurrentStage = getAnalyzeStage(m_CurrentStageType, m_Params.Vendor);
            if (m_CurrentStage == null)
            {
                Debug.LogErrorFormat("未能找到{0}阶段", m_CurrentStageType.ToString());
                Clear();
                return;
            }
            EAnalyzeStageStatus status = m_CurrentStage.Init(m_AnalyzeWrap, m_Params);
            handleStageStatus(status);
        }

        private AnalyzeStageBase getAnalyzeStage(EAnalyzeStageType stageType, EGPUVendorType vendorType)
        {
            Dictionary<EGPUVendorType, AnalyzeStageBase> stageDic;
            if (!AnalyzedStageMap.TryGetValue(m_CurrentStageType, out stageDic) || stageDic.Count == 0)
                return null;

            if (stageDic.Count == 1)
                return stageDic[EGPUVendorType.None];

            AnalyzeStageBase stage;
            if (!stageDic.TryGetValue(vendorType, out stage))
                return null;

            return stage;
        }

        private void update()
        { 
            EAnalyzeStageStatus status = m_CurrentStage.Execute();
            handleStageStatus(status);
        }

        private void handleStageStatus(EAnalyzeStageStatus status)
        { 
            switch (status)
            {
                case EAnalyzeStageStatus.Error:
                    //TODO 处理错误
                    break;
                case EAnalyzeStageStatus.Finish:
                    executeNext();
                    break;
                case EAnalyzeStageStatus.Complete:
                    m_CurrentStageType = EAnalyzeStageType.EndAnalyze;
                    executeNext();
                    break;
            }
        }

        private void Clear()
        {
            EditorUtility.ClearProgressBar();
            EditorApplication.update -= update;
            m_CurrentStageType = EAnalyzeStageType.None;
            m_CurrentStage = null;
            m_AnalyzeWrap = null;
            m_Params = null;
            m_AnalyzeFinish = null;
            foreach (var item in AnalyzedStageMap)
            {
                foreach (var item1 in item.Value)
                {
                    item1.Value.Clear();
                }
            }
        }
    }
}