using System;
using System.Collections.Generic;
using System.Text;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using System.Windows.Forms;
using System.Data;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    public class DatumInputLogHelper
    {
        public DatumInputLogHelper()
        {

        }

        #region 业务处理

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="db"></param>
        /// <param name="path"></param>
        /// <param name="logName"></param>
        /// <returns></returns>
        public static bool Insert(IDBHelper db, string path, string logName)
        {
            DatumInputLogDal.SingleInstance.LogName = logName;
            DatumInputLogDal.SingleInstance.FullPath = path;
            return DatumInputLogDal.SingleInstance.Insert(db);
        }

        /// <summary>
        /// 读取轻量数据
        /// </summary>
        /// <param name="db"></param>
        /// <param name="logName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool Select(IDBHelper db, ref DataTable dt)
        {
             return DatumInputLogDal.SingleInstance.Select(db, ref dt);
        }

        /// <summary>
        /// 读取文件数据
        /// </summary>
        /// <param name="db"></param>
        /// <param name="logName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool SelectFile(IDBHelper db,string path, string logName, ref DataTable dt)
        {
            DatumInputLogDal.SingleInstance.LogName = logName;
            DatumInputLogDal.SingleInstance.FullPath = path;
            return DatumInputLogDal.SingleInstance.SelectFile(db, ref dt);
        }
        //public static bool Select(IDBHelper db)
        //{
        //    return InfoListManagerDAL.SingleInstance.Select(model, ref dt);
        //}

        #endregion 业务处理
    }
}
