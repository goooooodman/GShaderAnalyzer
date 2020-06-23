/** 
 *FileName:       AnalyzeStageBase.cs
 *Author:         yinlongfei@hotmail.com
 *Date:           2020-06-21
 *Description:    各解析阶段基类
 *History:        
*/
namespace GShaderAnalyzer
{
    public abstract class AnalyzeStageBase
    {
        /// <summary>
        /// 初始化发生错误
        /// </summary>
        protected const int InitError = -1;
        /// <summary>
        /// 初始化便完成了阶段
        /// </summary>
        protected const int InitComplete = int.MinValue;
        /// <summary>
        /// 无法确定阶段需要执行次数
        /// </summary>
        protected const int InitUnknown = int.MaxValue;
        /// <summary>
        /// 初始化完成了整个解析
        /// </summary>
        protected const int InitFinish = 0;

        /// <summary>
        /// 当前需要分析的索引
        /// </summary>
        public int CurrentIndex
        {
            private set;
            get;
        }
        /// <summary>
        /// 总数
        /// </summary>
        public int Amount
        {
            private set;
            get;
        }

        /// <summary>
        /// 参数
        /// </summary>
        protected ShaderAnalyzeParams m_Params;
        /// <summary>
        /// 解析数据包
        /// </summary>
        protected AnalyzeWrap m_AnalyzeWrap;

        protected virtual int CountPerFrame
        {
            get { return 5; }
        }

        public EAnalyzeStageStatus Init(AnalyzeWrap wrap, ShaderAnalyzeParams param)
        {
            m_AnalyzeWrap = wrap;
            m_Params = param;

            int amount = init(wrap, param);
            EAnalyzeStageStatus status = EAnalyzeStageStatus.Success;

            switch (amount)
            {
                case InitComplete:
                    status = EAnalyzeStageStatus.Complete;
                    break;
                case InitUnknown:
                    status = EAnalyzeStageStatus.Success;
                    break;
                case InitError:
                    status = EAnalyzeStageStatus.Error;
                    break;
                case InitFinish:
                    status = EAnalyzeStageStatus.Finish;
                    break;
                default:
                    status = EAnalyzeStageStatus.Success;
                    Amount = amount;
                    break;
            }

            return status;
        }

        /// <summary>
        /// 子类实现的初始化方法
        /// </summary>
        /// <param name="wrap"></param>
        /// <param name="param"></param>
        /// <returns>总计要执行的次数，负数代表发生错误，0代表不需要Execute，int.MinValue代表解析完成，int.MaxValue代表执行次数未知由派生类自行决定</returns>
        protected virtual int init(AnalyzeWrap wrap, ShaderAnalyzeParams param)
        {
            return int.MaxValue;
        }

        public EAnalyzeStageStatus Execute()
        {
            int index = 0;

            while (index < CountPerFrame && CurrentIndex < Amount)
            {
                EAnalyzeStageStatus status = execute(CurrentIndex);
                if (status == EAnalyzeStageStatus.Error)
                    return EAnalyzeStageStatus.Error;
                else if (status == EAnalyzeStageStatus.Finish)
                    return EAnalyzeStageStatus.Finish;
                else if (status == EAnalyzeStageStatus.Complete)
                    return EAnalyzeStageStatus.Complete;
                CurrentIndex++;
                index++;
            }

            return CurrentIndex == Amount ? EAnalyzeStageStatus.Finish : EAnalyzeStageStatus.Success;
        }

        protected virtual EAnalyzeStageStatus execute(int index) { return EAnalyzeStageStatus.Finish; }

        public void Clear()
        {
            clear();

            m_Params = null;
            m_AnalyzeWrap = null;
            Amount = 0;
            CurrentIndex = 0;
        }

        protected virtual void clear() { }
    }
}