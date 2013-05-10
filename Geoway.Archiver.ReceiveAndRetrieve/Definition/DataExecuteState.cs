using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    [Serializable]
    public class DataExecuteState
    {
        EnumDataExecuteState serverState; //上传服务器执行状态
        public EnumDataExecuteState ServerState
        {
            get { return serverState; }
            set { serverState = value; }
        }

        EnumDataExecuteState metaState; //上传元数据执行状态
        public EnumDataExecuteState MetaState
        {
            get { return metaState; }
            set { metaState = value; }
        }

        EnumDataExecuteState snapShotState; //上传快视图执行状态
        public EnumDataExecuteState SnapShotState
        {
            get { return snapShotState; }
            set { snapShotState = value; }
        }

        EnumDataExecuteState thumbImageState; //拇指图执行状态
        public EnumDataExecuteState ThumbImageState
        {
            get { return thumbImageState; }
            set { thumbImageState = value; }
        }

        EnumDataExecuteState extentState; //空间范围执行状态
        public EnumDataExecuteState ExtentState
        {
            get { return extentState; }
            set { extentState = value; }
        }

        public override string ToString()
        {
            string result = string.Empty;

            if (this.serverState == EnumDataExecuteState.NoDone && this.metaState == EnumDataExecuteState.NoDone && this.snapShotState == EnumDataExecuteState.NoDone)
            {
                result = "未执行";
            }

            else if (this.serverState == EnumDataExecuteState.Failed || this.metaState == EnumDataExecuteState.Failed || this.snapShotState == EnumDataExecuteState.Failed)
            {
                result = "执行失败";
            }
            else
            {

                result = "执行成功";
            }
            return result;
        }

        public virtual bool IsSuccessed()
        {
            if (this.serverState == EnumDataExecuteState.NoDone &&
                    this.metaState == EnumDataExecuteState.NoDone &&
                    this.snapShotState == EnumDataExecuteState.NoDone &&
                    this.thumbImageState == EnumDataExecuteState.NoDone &&
                    this.extentState == EnumDataExecuteState.NoDone)
            {
                return false;
            }
            return  this.serverState != EnumDataExecuteState.Failed &&
                    this.metaState != EnumDataExecuteState.Failed &&
                    this.snapShotState != EnumDataExecuteState.Failed &&
                    this.thumbImageState != EnumDataExecuteState.Failed &&
                    this.extentState != EnumDataExecuteState.Failed;
        }

        private string GetStateString(EnumDataExecuteState state)
        {
            switch (state)
            {
                case EnumDataExecuteState.Failed:
                    return "执行失败";
                case EnumDataExecuteState.Successed:
                    return "执行成功";
                case EnumDataExecuteState.NoDone:
                    return "未执行";
                default:
                    return "未执行";
            }
        }

        /// <summary>
        /// 数据实体上传状态
        /// </summary>
        public string ServerStateString
        {
            get { return GetStateString(serverState); }
        }

        /// <summary>
        /// 元数据上传状态
        /// </summary>
        public string MetadataStateString
        {
            get { return GetStateString(metaState); }
        }

        /// <summary>
        /// 快视图上传状态
        /// </summary>
        public string SnapShotStateString
        {
            get { return GetStateString(snapShotState); }
        }

        /// <summary>
        /// 拇指图执行状态
        /// </summary>
        public string ThumbImageStateString
        {
            get { return GetStateString(thumbImageState); }
        }

        /// <summary>
        /// 空间范围执行状态
        /// </summary>
        public string ExtentStateString
        {
            get { return GetStateString(extentState); }
        }
    }
}
