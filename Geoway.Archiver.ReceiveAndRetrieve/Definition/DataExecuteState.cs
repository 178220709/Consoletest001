using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    [Serializable]
    public class DataExecuteState
    {
        EnumDataExecuteState serverState; //�ϴ�������ִ��״̬
        public EnumDataExecuteState ServerState
        {
            get { return serverState; }
            set { serverState = value; }
        }

        EnumDataExecuteState metaState; //�ϴ�Ԫ����ִ��״̬
        public EnumDataExecuteState MetaState
        {
            get { return metaState; }
            set { metaState = value; }
        }

        EnumDataExecuteState snapShotState; //�ϴ�����ͼִ��״̬
        public EnumDataExecuteState SnapShotState
        {
            get { return snapShotState; }
            set { snapShotState = value; }
        }

        EnumDataExecuteState thumbImageState; //Ĵָͼִ��״̬
        public EnumDataExecuteState ThumbImageState
        {
            get { return thumbImageState; }
            set { thumbImageState = value; }
        }

        EnumDataExecuteState extentState; //�ռ䷶Χִ��״̬
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
                result = "δִ��";
            }

            else if (this.serverState == EnumDataExecuteState.Failed || this.metaState == EnumDataExecuteState.Failed || this.snapShotState == EnumDataExecuteState.Failed)
            {
                result = "ִ��ʧ��";
            }
            else
            {

                result = "ִ�гɹ�";
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
                    return "ִ��ʧ��";
                case EnumDataExecuteState.Successed:
                    return "ִ�гɹ�";
                case EnumDataExecuteState.NoDone:
                    return "δִ��";
                default:
                    return "δִ��";
            }
        }

        /// <summary>
        /// ����ʵ���ϴ�״̬
        /// </summary>
        public string ServerStateString
        {
            get { return GetStateString(serverState); }
        }

        /// <summary>
        /// Ԫ�����ϴ�״̬
        /// </summary>
        public string MetadataStateString
        {
            get { return GetStateString(metaState); }
        }

        /// <summary>
        /// ����ͼ�ϴ�״̬
        /// </summary>
        public string SnapShotStateString
        {
            get { return GetStateString(snapShotState); }
        }

        /// <summary>
        /// Ĵָͼִ��״̬
        /// </summary>
        public string ThumbImageStateString
        {
            get { return GetStateString(thumbImageState); }
        }

        /// <summary>
        /// �ռ䷶Χִ��״̬
        /// </summary>
        public string ExtentStateString
        {
            get { return GetStateString(extentState); }
        }
    }
}
