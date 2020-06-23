/** 
 *FileName:       AnalyzedBase.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    �������ݻ��࣬����������ɫ��Դ���룬����Դ����ֻ���ڽ�������
 *History:        
*/
using System;
using System.Collections.Generic;

namespace GShaderAnalyzer
{
    /// <summary>
    /// �������ݻ���
    /// </summary>
    public abstract class AnalyzedBase
    {
        [NonSerialized]
        internal List<string> SourceCodeLines = new List<string>();
    }
}