/** 
 *FileName:     GPUVendorNameGUIAttribute.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-10 
 *Description:  用于禁止修改GPUVendor的Name字段
 *History: 
*/
using System;
using UnityEditor;
using UnityEngine;

namespace GShaderAnalyzer
{
    [CustomPropertyDrawer(typeof(GPUVendorNameGUIAttribute))]
    public class GPUVendorNameGUIDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GPUVendorNameGUIAttribute attr = (GPUVendorNameGUIAttribute)attribute;

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(position, new GUIContent(property.stringValue, attr.Tooltip));
            EditorGUI.EndProperty();
        }
    }
}