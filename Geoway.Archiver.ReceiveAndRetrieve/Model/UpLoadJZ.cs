using System;
using System.Collections.Generic;
using System.Text;
using Geoway.ADF.MIS.DB.Public.Interface;
using System.Data;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.ReceiveAndRetrieve.Factory;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.Utility.Class;
using Geoway.Archiver.Modeling.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.ADF.MIS.Core.Public.util;
using Geoway.ADF.MIS.Utility.DevExpressEx;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    /// <summary>
    /// 进度暂时
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="max"></param>
    /// <param name="message"></param>
    public delegate void SetProgressEventHandler(int pos, int max, string message);

    /// <summary>
    /// 添加日志
    /// </summary>
    /// <param name="msg"></param>
    public delegate void AddLogEventHanlder(string msg);
    
    public class UpLoadJZ
    {
        private IDBHelper _dbHelper;
        private XLSReadHelper _xlsReadHelper;//元数据文件（EXCEL文件）操作类
        private string _currentSheetName;//读取的元数据文件（EXCEL文件）的当前sheet页

        public event SetProgressEventHandler SetProgressEvent;

        public event AddLogEventHanlder AddLogEvent;
        

        public UpLoadJZ(IDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        
        #region 属性

        private int _catalogID;
        /// <summary>
        /// 选中节点ID
        /// </summary>
        public int CatalogID
        {
            set{_catalogID = value;}
        }
        
        private string _virtualWarehouseAddress;
        /// <summary>
        /// 库房位置
        /// </summary>
        public string VirtualWarehouseAddress
        {
            set { _virtualWarehouseAddress = value; }
        }

        private string _metaFilePath;
        /// <summary>
        /// 元数据路径
        /// </summary>
        public string MetaFilePath
        {
            set
            {
                _metaFilePath = value;
                _xlsReadHelper = new XLSReadHelper(_metaFilePath);
            }
        }

        private string _selectedSheetName;
        /// <summary>
        /// 所选Sheet名称
        /// </summary>
        public string SelectedSheetName
        {
            set { _selectedSheetName = value; }
        }

        private string _barCode;
        /// <summary>
        /// 条形码
        /// </summary>
        public string BarCode
        {
            set { _barCode = value; }
        }
        #endregion


        public bool DoUpLoadJZ()
        {
            int pos = 0;
            int max = 0;
            //日志
            AddLog(string.Format("开始入库..."));
            bool success = true;
            IList<string> lstSheetes=new List<string>();
            try
            {
                if (_selectedSheetName.Length == 0)
                {
                    lstSheetes = _xlsReadHelper.GetXLSSheets(_metaFilePath);
                }
                else
                {
                    lstSheetes.Add(_selectedSheetName);
                }
                max = lstSheetes.Count;
                //遍历每个sheet页
                foreach (string sheet in lstSheetes)
                {
                    pos++;
                    //显示进度
                    InvokeProgress(pos, max, string.Format("开始写入第{0}页(共{1}页)数据:{2}...", pos-1, max, _selectedSheetName));
                    _currentSheetName = sheet;
                    success &= WriteSingleSheet();
                    InvokeProgress(pos, max, string.Format("写入第{0}页(共{1}页)数据:{2}结束", pos, max, _selectedSheetName));
                }
                //日志
                AddLog(string.Format("入库完成..."));
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                success = false;
                //日志
                AddLog(string.Format("入库失败! 错误{0}",ex));
            }
            return success;
        }

        /// <summary>
        /// 写excel的单张sheet页
        /// </summary>
        /// <returns></returns>
        public bool WriteSingleSheet()
        {
            int position = 0;
            int max = 0;
            bool success = true;
            StringBuilder error = new StringBuilder();
            //日志
            AddLog(string.Format("开始写入【{0}】页面中的记录...",_currentSheetName));
            
            try
            {
                DataTable dataTable = null;
                Dictionary<string, object> dicResult = new Dictionary<string, object>();
                //根据sheet页名称读取数据到内存表
                dataTable = _xlsReadHelper.GetSheetDataTable(_currentSheetName);
                max = dataTable.Rows.Count;
                //遍历每一行，即每一条数据
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    position++;
                    
                    int colCount = dataTable.Columns.Count;//元数据属性数量
                    for (int i = 0; i < colCount; i++)
                    {
                        if (!dicResult.ContainsKey(dataTable.Columns[i].ColumnName))
                        {
                            dicResult.Add(dataTable.Columns[i].ColumnName, dataRow[i].ToString());
                        }
                    }
                    if (dicResult.Count == 0)
                    {
                        continue;
                    }
                    
                    UpLoadJZData upLoadJzData = new UpLoadJZData(_dbHelper);
                    upLoadJzData.BarCode = _barCode;
                    upLoadJzData.CatalogID = _catalogID;
                    upLoadJzData.DicSoure = dicResult;
                    upLoadJzData.VirtualWarehouseAddress = _virtualWarehouseAddress;
                    success = upLoadJzData.WriteMetaData();
                    
                    //显示进度
                    InvokeProgress(position, max, string.Format("{0}:第{1}条(共{2}条)数据写入...", _currentSheetName, position, max));
                    //记录错误信息
                    if (!success)
                    {
                        error.Append(string.Format("{0}:第{1}条数据入库失败\r\n", _currentSheetName, position));
                    }
                }

                //日志
                if (error.Length == 0)
                {
                    AddLog(string.Format("【{0}】页面中的记录入库成功！", _currentSheetName));
                }
                else
                {
                    AddLog(string.Format("【{0}】页面中的记录入库完成，但存在错误：{1}", _currentSheetName, error));
                }
            }
            catch(Exception ex)
            {
                success = false;
                //日志
                AddLog(string.Format("【{0}】页面中的记录入库失败！错误：{1}", _currentSheetName, ex));
         
            }
            return success;
        }
        
        
        private void InvokeProgress(int pos, int max, string message)
        {
            if(SetProgressEvent!=null)
            {
                SetProgressEvent(pos, max, message);
            }
        }
        
        private void AddLog(string msg)
        {
            if(AddLogEvent!=null)
            {
                AddLogEvent(msg);
            }
        }
    }
}
