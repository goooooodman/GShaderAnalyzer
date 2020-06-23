/** 
 *FileName:     GPUVendorTypeGUIAttribute.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-10 
 *Description:  Inspector面板的EGPUVendorType绘制器，自动将GPUVendor的Name字段设置VendorType的枚举名
 *History: 
*/
using System;
using UnityEditor;
using UnityEngine;

namespace GShaderAnalyzer
{
    [CustomPropertyDrawer(typeof(EGPUVendorTypeGUIAttribute))]
    public class EGPUVendorTypeGUIDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EGPUVendorTypeGUIAttribute attr = (EGPUVendorTypeGUIAttribute)attribute;

            Enum targetEnum = (EGPUVendorType)property.intValue;

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            Enum enumNew = EditorGUI.EnumPopup(position, new GUIContent(property.name, attr.Tooltip), targetEnum, (Enum index) =>
            {
                bool show = true;
                switch ((EGPUVendorType)index)
                {
                    case EGPUVendorType.None:
                    case EGPUVendorType.All:
                        show = false;
                        break;
                }

                return show;
            });
            property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
            if (EditorGUI.EndChangeCheck())
            {
                SerializedProperty nameProperty = property.serializedObject.FindProperty(property.propertyPath.Replace("VendorType", "Name"));
                if (nameProperty != null)
                    nameProperty.stringValue = enumNew.ToString();
            }
            EditorGUI.EndProperty();
        }
    }
}