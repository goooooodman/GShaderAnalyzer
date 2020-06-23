/** 
 *FileName:     GPUVendors.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-21 
 *Description:  GPU厂商信息集合，用于序列化成配置文件
 *History: 
*/
using UnityEditor;
using UnityEngine;

namespace GShaderAnalyzer
{
    /// <summary>
    /// GPU厂商信息集合
    /// </summary>
    public sealed class GPUVendors : ScriptableObject
    {
        public GPUVendor[] Vendors = null;

        public static GPUVendors Load()
        {
            GPUVendors models = AssetDatabase.LoadAssetAtPath<GPUVendors>("Assets/Editor/ShaderAnalyzer/GPUVendors.asset");
            return models;
        }

        /// <summary>
        /// 通过厂商类型获取GPU厂商信息
        /// </summary>
        /// <param name="vendorType">GPU厂商类型</param>
        /// <returns>GPU厂商信息</returns>
        public GPUVendor GetVendorModelsByVendorType(EGPUVendorType vendorType)
        {
            if (Vendors == null)
                return null;

            for (int i = 0; i < Vendors.Length; i++)
            {
                GPUVendor vendor = Vendors[i];
                if (vendor.VendorType == vendorType)
                    return vendor;
            }

            return null;
        }
    }
}

