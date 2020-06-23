/** 
 *FileName:     EAPITypeFlagDrawer.cs 
 *Author:       yinlongfei@hotmail.com 
 *Date:         2020-06-10 
 *Description:  Inspector面板的EAPIType绘制器
 *History: 
*/
using System;
using UnityEditor;
using UnityEngine;

namespace GShaderAnalyzer
{
    [CustomPropertyDrawer(typeof(EAPITypeGUIAttribute))]
    public class EAPITypeFlagDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EAPITypeGUIAttribute flagSettings = (EAPITypeGUIAttribute)attribute;

            Enum targetEnum = (EAPIType)property.intValue;

            EditorGUI.BeginProperty(position, label, property);
            Enum enumNew = EditorGUI.EnumFlagsField(position, new GUIContent(property.name, flagSettings.Tooltip), targetEnum);
            property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
            EditorGUI.EndProperty();
        }
    }
}