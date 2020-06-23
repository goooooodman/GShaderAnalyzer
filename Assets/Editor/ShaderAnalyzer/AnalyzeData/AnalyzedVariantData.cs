/** 
 *FileName:       AnalyzedVariantData.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    变体解析数据
 *History:        
*/
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GShaderAnalyzer
{
    /// <summary>
    /// 变体解析数据
    /// </summary>
    [Serializable]
    public sealed class AnalyzedVariantData : AnalyzedBase, ISerializationCallbackReceiver
    {
        public string VariantName = string.Empty;

        public EGPUVendorType GPUVendorType = EGPUVendorType.Mali;

        [SerializeField]
        private string[] m_ShaderSerialized;

        [NonSerialized]
        public List<AnalyzedShaderDataBase> Shaders = new List<AnalyzedShaderDataBase>();
        [NonSerialized]
        internal Dictionary<EShaderType, string> ShaderSourceCodes = new Dictionary<EShaderType, string>();
        [NonSerialized]
        internal Dictionary<EShaderType, string[]> CompileResult = new Dictionary<EShaderType, string[]>();

        private static Dictionary<EGPUVendorType, Type> m_AnalyzedShaderDataTypeMap = null;
        private static Dictionary<EGPUVendorType, Type> AnalyzedShaderDataTypeMap
        {
            get
            {
                if (m_AnalyzedShaderDataTypeMap == null)
                {
                    m_AnalyzedShaderDataTypeMap = new Dictionary<EGPUVendorType, Type>();
                    Assembly assembly = Assembly.GetAssembly(typeof(AnalyzedVariantData));

                    Type[] types = assembly.GetTypes();
                    for (int i = 0; i < types.Length; i++)
                    {
                        Type type = types[i];

                        if (!type.IsSubclassOf(typeof(AnalyzedShaderDataBase)))
                            continue;

                        AnalyzeVendorAttribute attr = type.GetCustomAttribute<AnalyzeVendorAttribute>();
                        if (attr == null)
                            continue; 

                        AnalyzedShaderDataTypeMap.Add(attr.Vendor, type);
                    }
                }

                return m_AnalyzedShaderDataTypeMap;
            }
        }

        /// <summary>
        /// 序列化之后被Unity自动调用，将m_ShaderSerialized存储的Json数据反序列化成AnalyzedShaderDataBase存储在Shaders中
        /// </summary>
        public void OnAfterDeserialize()
        { 
            if (m_ShaderSerialized == null)
                return;
            Type type; 
            if (!AnalyzedShaderDataTypeMap.TryGetValue(GPUVendorType, out type))
                return;

            Shaders = new List<AnalyzedShaderDataBase>(m_ShaderSerialized.Length);

            for (int i = 0; i < m_ShaderSerialized.Length; i++)
            {
                string serialized = m_ShaderSerialized[i];

                Shaders.Add(JsonUtility.FromJson(serialized, type) as AnalyzedShaderDataBase);
            }
        }

        /// <summary>
        /// 序列化之前被Unity自动调用，将Shaders中存储的AnalyzedShaderDataBase基类序列化成Json存储在m_ShaderSerialized数组中
        /// </summary>
        public void OnBeforeSerialize()
        {
            if (Shaders == null)
                return;
            m_ShaderSerialized = new string[Shaders.Count];

            for (int i = 0; i < Shaders.Count; i++)
            {
                AnalyzedShaderDataBase asdb = Shaders[i];
                string json = JsonUtility.ToJson(asdb);
                m_ShaderSerialized[i] = json;
            }
        }
    }
}