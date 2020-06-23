/** 
 *FileName:       GUITools.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    GUI绘制函数以及样式
 *History:        
*/
using System;
using UnityEditor;
using UnityEngine;

namespace GShaderAnalyzer
{
    public class GUITools
    {
        public static class Styles
        {
            private static GUIStyle mGroupBoxStyle;
            public static GUIStyle GroupBoxStyle
            {
                get
                {
                    if (mGroupBoxStyle == null) mGroupBoxStyle = new GUIStyle("GroupBox");
                    return mGroupBoxStyle;
                }
            }

            private static GUIStyle mHelpBoxStyle;
            public static GUIStyle HelpBoxStyle
            {
                get
                {
                    if (mHelpBoxStyle == null)
                    {
                        mHelpBoxStyle = new GUIStyle("HelpBox");
                        mHelpBoxStyle.padding = new RectOffset(4, 4, 4, 4);
                        mHelpBoxStyle.padding = new RectOffset(0, 0, 4, 4);
                    }
                    return mHelpBoxStyle;
                }
            }

            private static GUIStyle mWizardBoxStyle;
            public static GUIStyle WizardBoxStyle
            {
                get
                {
                    if (mWizardBoxStyle == null)
                    {
                        mWizardBoxStyle = new GUIStyle("Wizard Box");
                    }
                    return mWizardBoxStyle;
                }
            }

            private static GUIStyle mAssetLabelStyle;
            public static GUIStyle AssetLabelStyle
            {
                get
                {
                    if (mAssetLabelStyle == null)
                    {
                        mAssetLabelStyle = new GUIStyle("AssetLabel");
                    }
                    return mAssetLabelStyle;
                }
            }

            private static GUIStyle mAssetLabelPartialStyle;
            public static GUIStyle AssetLabelPartialStyle
            {
                get
                {
                    if (mAssetLabelPartialStyle == null)
                    {
                        mAssetLabelPartialStyle = new GUIStyle("AssetLabel Partial");
                    }

                    return mAssetLabelPartialStyle;
                }
            }

            private static GUIStyle mTextFieldCenterStyle;
            public static GUIStyle TextFieldCenterStyle
            {
                get
                {
                    if (mTextFieldCenterStyle == null)
                    {
                        mTextFieldCenterStyle = new GUIStyle("textfield");
                        mTextFieldCenterStyle.alignment = TextAnchor.MiddleCenter;
                    }

                    return mTextFieldCenterStyle;
                }
            }
            
        }

        /// <summary>
        /// 绘制一个可以下拉的标头
        /// </summary>
        /// <param name="text">显示的文本</param>
        /// <param name="forceOn"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool DrawHeader(string text, bool forceOn, bool state, bool bold = true, int fontSize = 11)
        {
            GUILayout.Space(3f);
            if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            GUILayout.BeginHorizontal();
            GUI.changed = false;

            text = string.Format("<size={0}>" + text + "</size>", fontSize);
            if (bold)
                text = string.Format("<b>{0}</b>", text);
            if (state) text = "\u25BC " + text;
            else text = "\u25BA " + text;
            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;

            GUILayout.Space(2f);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            if (!forceOn && !state) GUILayout.Space(3f);
            return state;
        }

        /// <summary>
        /// 绘制一个可以下拉的标头
        /// </summary>
        /// <param name="text">显示的文本</param>
        /// <param name="forceOn"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool DrawHeader(GUIContent text, bool forceOn, bool state, bool bold = true, int fontSize = 11)
        {
            GUILayout.Space(3f);
            if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            GUILayout.BeginHorizontal();
            GUI.changed = false;

            text.text = string.Format("<size={0}>" + text.text + "</size>", fontSize);
            if (bold)
                text.text = string.Format("<b>{0}</b>", text.text);
            if (state) text.text = "\u25BC " + text.text;
            else text.text = "\u25BA " + text.text;

            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;

            GUILayout.Space(2f);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            if (!forceOn && !state) GUILayout.Space(3f);
            return state;
        }

        private static int m_ContentDepth = 0;
        public static void BeginContents(GUIStyle style)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f * m_ContentDepth++);
            EditorGUILayout.BeginHorizontal(style, GUILayout.MinHeight(10f));
            GUILayout.BeginVertical();
            GUILayout.Space(2f);
        }

        public static void ClearContentDepth()
        {
            m_ContentDepth = 0;
        }

        public static void EndContents()
        {
            m_ContentDepth--;
            try { GUILayout.Space(3f); }
            catch (System.Exception) { }

            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(3f);
            GUILayout.EndHorizontal();

            GUILayout.Space(3f);
        }

        /// <summary>
        /// 绘制分页列表
        /// </summary>
        /// <param name="currentIndex">当前页数</param>
        /// <param name="total">总页数</param>
        /// <param name="callback">点击回调</param>
        /// <returns>当前页数</returns>
        public static int DrawPageList(int currentIndex, int total, Action callback)
        {
            if (currentIndex <= 0 || total <= 0) return currentIndex;

            int pageIndex = currentIndex;

            EditorGUILayout.BeginHorizontal();

            drawPageItem(1, currentIndex, "1|<", 1, ref pageIndex);
            drawPageItem(1, currentIndex, "<上一页", currentIndex - 1, ref pageIndex);

            pageIndex = EditorGUILayout.IntField(pageIndex, Styles.TextFieldCenterStyle, GUILayout.MinWidth(30f), GUILayout.MaxWidth(30f), GUILayout.ExpandWidth(false));
            if (pageIndex <= 0) pageIndex = 1;
            else if (pageIndex > total) pageIndex = total;

            drawPageItem(total, currentIndex, "下一页>", currentIndex + 1, ref pageIndex);
            drawPageItem(total, currentIndex, ">|" + total.ToString(), total, ref pageIndex);

            if (pageIndex != currentIndex && callback != null) callback();

            EditorGUILayout.EndHorizontal();

            return pageIndex;
        }

        private static void drawPageItem(int index, int currentIndex, string content, int resultIndex, ref int result)
        {
            if (currentIndex == index)
            {
                GUILayout.Label(content, Styles.AssetLabelPartialStyle);
            }
            else
            {
                if (GUILayout.Button(content, Styles.AssetLabelStyle))
                {
                    result = resultIndex;
                }
            }
        }
    }
}