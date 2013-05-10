using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.Utility.Definition;
using Geoway.Archiver.Utility.Class;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using Geoway.ADF.GIS.Utility;
using Geoway.ADF.MIS.Utility.Log;


namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    public delegate void VectorDataTransferEventHandler(VectorTransferProgressEventArgs e);
    
    internal class ImportSpatialData
    {
        #region 事件定义
        public event VectorDataTransferEventHandler TransferBegin;//开始上传
        public event VectorDataTransferEventHandler TransferProgress;//正在上传
        public event VectorDataTransferEventHandler TransferEnd;//结束上传
        #endregion

        private VectorTransferProgressEventArgs _e;

        #region 属性
        private string _fileName;
        /// <summary>
        /// 本地空间数据路径
        /// </summary>
        public string FileName
        {
            set
            {
               
                _fileName = value;
            }
        }

        private EnumWorkspaceType _enumWorkspaceType;
        /// <summary>
        /// 目标工作空间类型
        /// </summary>
        public EnumWorkspaceType EnumWorkspaceType
        {
            set { _enumWorkspaceType = value; }
        }

        private IFeatureWorkspace _targetWorkspace;

        public IFeatureWorkspace TargetWorkspace
        {
            set { _targetWorkspace = value; }
        }

        private string _targetDatasetName;
        /// <summary>
        /// 目标数据集名称
        /// </summary>
        public string TargetDatasetName
        {
            set { _targetDatasetName = value; }
        }

        private string _targetFcName;
        /// <summary>
        /// 
        /// </summary>
        public string TargetFcName
        {
            set
            {
                
                _targetFcName = value;
            }
        }

        #endregion

        public ImportSpatialData()
        {
            _e = new VectorTransferProgressEventArgs();
        }
        /// <summary>
        /// 执行上传
        /// </summary>
        /// <returns></returns>
        internal bool DoImport()
        {
            try
            {
                //源要素类名称
                string fcSourceName = Path.GetFileNameWithoutExtension(_fileName);
                string wsfileName = Path.GetDirectoryName(_fileName);
                //根据空间数据源类型获取工作空间
                IFeatureWorkspace wsSource =
                    GwWorkspaceFactory.GetWorkspace(_enumWorkspaceType, wsfileName) as IFeatureWorkspace;
                IFeatureClass fcSource = VectorDataOperater.GetFeatureClass(wsSource, fcSourceName);
                IFeatureClass fcTarget = VectorDataOperater.CreateFeatureClass(ref fcSource,
                                                                               ref _targetWorkspace,
                                                                               _targetFcName,
                                                                               "Defaults");
                if (fcTarget != null)
                {
                    VectorTransferResult transferResult;
                    DataTransfer dataTransfer = new DataTransfer();
                    dataTransfer.OnProgressCountChange += dataTransfer_OnProgressCountChange;
                    dataTransfer.OnProgressPositionChange += dataTransfer_OnProgressPositionChange;
                    dataTransfer.OnCopyError += dataTransfer_OnCopyError;
                    dataTransfer.TransferEnd += dataTransfer_TransferEnd;
                    return dataTransfer.CopyFeatures(fcSource, fcTarget, null, false, out transferResult);

                }
                return false;
            }
            catch(Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }
        }

        private void dataTransfer_TransferEnd(VectorTransferResult result)
        {
            _e.FeatureCount = result.TotalFeatureCount;
            _e.CompleteFeatureCount = result.CompleteFeatureCount;
            _e.EnumTransResultType = result.TransResultType;
            InvokeTransferEnd(_e);
        }

        private void dataTransfer_OnCopyError(string strFCName, Exception ex)
        {
            _e.ErrorInfo = ex.ToString();
            InvokeTransferProgress(_e);
            _e.ErrorInfo = string.Empty;
        }

        private void dataTransfer_OnProgressPositionChange(int iValue)
        {
            _e.CompleteFeatureCount = iValue;
            InvokeTransferProgress(_e);
        }

        private void dataTransfer_OnProgressCountChange(int iValue)
        {
            _e.FeatureCount = iValue;
            InvokeTransferBegin(_e);
        }


        private void InvokeTransferBegin(VectorTransferProgressEventArgs e)
        {
            if(null!=TransferBegin)
            {
                TransferBegin.Invoke(e);
            }
        }
        
        private void InvokeTransferProgress(VectorTransferProgressEventArgs e)
        {
            if(null!= TransferProgress)
            {
                TransferProgress.Invoke(e);
            }
        
        }
        
        private void InvokeTransferEnd(VectorTransferProgressEventArgs e)
        {
            if(null!=TransferEnd)
            {
                TransferEnd.Invoke(e);
            }
        }
    }

    public class VectorTransferProgressEventArgs : EventArgs
    {
        public int FeatureCount;
        public int CompleteFeatureCount;
        public string ErrorInfo;
        public enumTransResultType EnumTransResultType;
    }
}
