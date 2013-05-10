using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.Archiver.Utility.DAL;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.ADF.MIS.DB.Public.Interface;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    /// <summary>
    /// 影像快视图数据库操作类

    /// </summary>
    public class DataSnapshotDAL : DALBase<DataSnapshotDAL>
    {
        public const string TABLE_NAME = "TBARC_SNAPSHOT";

        public const string FLD_NAME_F_ID = "F_ID";
        public const string FLD_NAME_F_OBJECTID = "F_OBJECTID";
        public const string FLD_NAME_F_IMAGE = "F_IMAGE";
        public const string FLD_NAME_F_XSIZE = "F_XSIZE";
        public const string FLD_NAME_F_OFFSET = "F_OFFSET";
        public const string FLD_NAME_F_ROTATION = "F_ROTATION";
        public const string FLD_NAME_F_YSIZE = "F_YSIZE";
        public const string FLD_NAME_F_XLEFTTOP = "F_XLEFTTOP";
        public const string FLD_NAME_F_YLEFTTOP = "F_YLEFTTOP";
        public const string FLD_NAME_F_EXTENSION = "F_EXTENSION";
        public const string FLD_NAME_F_HASAUX = "F_HASAUX";
        public const string FLD_NAME_F_AUX = "F_AUX";
        public const string FLD_NAME_F_REFERENCE = "F_REFERENCE";
        public const string FLD_NAME_F_HASTHUMB = "F_HASTHUMB";
        public const string FLD_NAME_F_THUMBIMAGE = "F_THUMBIMAGE";
        public const string FLD_NAME_F_THUMBEXTEN = "F_THUMBEXTEN";

        #region 属性

        private int _objectID;
        /// <summary>
        /// 快视图对应的数据ID，
        /// </summary>
        public int ObjectID
        {
            get { return _objectID; }
            set { _objectID = value; }
        }
        

        private int _id;
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private byte[] _snapShotImageBytes = null;
        /// <summary>
        /// 快视图二进制字节流

        /// </summary>
        public byte[] SnapShotImageBytes
        {
            get { return _snapShotImageBytes; }
            set { _snapShotImageBytes = value; }
        }

        private double _xSize;
        /// <summary>
        /// x方向像素大小
        /// </summary>
        public double XSize
        {
            get { return _xSize; }
            set { _xSize = value; }
        }

        private double _offset;
        /// <summary>
        /// 平移量

        /// </summary>
        public double Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        private double _rotation;
        /// <summary>
        /// 旋转量

        /// </summary>
        public double Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        private double _ySize;
        /// <summary>
        /// y方向像素大小
        /// </summary>
        public double YSize
        {
            get { return _ySize; }
            set { _ySize = value; }
        }

        private double _xLeftTop;
        /// <summary>
        /// 左上角点x坐标
        /// </summary>
        public double XLeftTop
        {
            get { return _xLeftTop; }
            set { _xLeftTop = value; }
        }

        private double _yLeftTop;
        /// <summary>
        /// 左上角点y坐标
        /// </summary>
        public double YLeftTop
        {
            get { return _yLeftTop; }
            set { _yLeftTop = value; }
        }

        private string _snapShotExtension = string.Empty;
        /// <summary>
        /// 文件扩展名

        /// </summary>
        public string SnapShotExtension
        {
            get { return _snapShotExtension; }
            set { _snapShotExtension = value; }
        }

        private bool _hasAux = false;
        /// <summary>
        /// 是否包含AUX文件
        /// </summary>
        public bool HasAux
        {
            get { return _hasAux; }
            set { _hasAux = value; }
        }

        private byte[] _auxBytes = null;
        /// <summary>
        /// AUX文件
        /// </summary>
        public byte[] AuxBytes
        {
            get { return _auxBytes; }
            set { _auxBytes = value; }
        }

        private string _spatialReferenceName = string.Empty;
        /// <summary>
        /// 快视图空间参考名称
        /// </summary>
        public string SpatialReferenceName
        {
            get { return _spatialReferenceName; }
            set { _spatialReferenceName = value; }
        }
        private bool _hasThumb = false;
        /// <summary>
        /// 是否包含AUX文件
        /// </summary>
        public bool HasThumb
        {
            get { return _hasThumb; }
            set { _hasThumb = value; }
        }

        private byte[] _thumbImageBytes = null;
        /// <summary>
        /// 拇指图二进制字节流
        /// </summary>
        public byte[] ThumbImageBytes
        {
            get { return _thumbImageBytes; }
            set { _thumbImageBytes = value; }
        }

        private string _thumbExtension;
        /// <summary>
        /// 拇指图后缀
        /// </summary>
        public string ThumbExtension
        {
            get { return _thumbExtension; }
            set { _thumbExtension = value; }
        }

        #endregion

        public override bool Insert()
        {
            string sqlStatement = string.Empty;
            IList<DBFieldItem> items = new List<DBFieldItem>();
            _id = GetNextID(TABLE_NAME, FLD_NAME_F_ID);
            items.Add(new DBFieldItem(FLD_NAME_F_ID, _id, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_OBJECTID, _objectID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_XSIZE, _xSize, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_OFFSET, _offset, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_ROTATION, _rotation, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_YSIZE, _ySize, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_XLEFTTOP, _xLeftTop, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_YLEFTTOP, _yLeftTop, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_EXTENSION, _snapShotExtension, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_HASAUX, _hasAux ? 1 : 0, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_REFERENCE, _spatialReferenceName, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_HASTHUMB, _hasThumb ? 1 : 0, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_THUMBEXTEN,_thumbExtension,EnumDBFieldType.FTString));
            try
            {
                sqlStatement = SQLStringUtility.GetInsertSQL(TABLE_NAME, items, DBHelper.GlobalDBHelper);
            }
            catch (Exception ex)
            {
                return false;
            }

            return DoSQL(sqlStatement);
        }

        public override bool Delete()
        {
            return DBHelper.GlobalDBHelper.DeleteRow(TABLE_NAME, FLD_NAME_F_ID + "=" + _id) > 0;
        }

        public override bool Update()
        {
            string sqlStatement = string.Empty;

            IList<DBFieldItem> items = new List<DBFieldItem>();

            _id = GetNextID(TABLE_NAME, FLD_NAME_F_ID);
            string filter = FLD_NAME_F_ID + "=" + _id;

            items.Add(new DBFieldItem(FLD_NAME_F_XSIZE, _xSize, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_OBJECTID, _objectID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_OFFSET, _offset, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_ROTATION, _rotation, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_YSIZE, _ySize, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_XLEFTTOP, _xLeftTop, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_YLEFTTOP, _yLeftTop, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_YLEFTTOP, _snapShotExtension, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_HASAUX, _hasAux ? 1 : 0, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_REFERENCE, _spatialReferenceName, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_HASTHUMB, _hasThumb ? 1 : 0, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_THUMBEXTEN, _thumbExtension, EnumDBFieldType.FTString));
            try
            {
                sqlStatement = SQLStringUtility.GetUpdateSQL(TABLE_NAME, items, filter, DBHelper.GlobalDBHelper);
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                throw;
            }

            return DoSQL(sqlStatement);
        }
        // <summary>
        /// 选择
        /// </summary>
        /// <returns></returns>
        public override IList<DataSnapshotDAL> Select()
        {
            throw new Exception("未实现");
        }
        /// <summary>
        /// 查询单条影像记录
        /// </summary>
        /// <param name="id">影像对应的数据ID</param>
        /// <returns></returns>
        public static DataSnapshotDAL SelectByDataId(int id)
        {
            string sql = string.Format("select {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12} from {13} where {14}={15}",
                                       FLD_NAME_F_ID, 
                                       FLD_NAME_F_XSIZE,
                                       FLD_NAME_F_OFFSET,
                                       FLD_NAME_F_ROTATION,
                                       FLD_NAME_F_YSIZE,
                                       FLD_NAME_F_XLEFTTOP,
                                       FLD_NAME_F_YLEFTTOP,
                                       FLD_NAME_F_EXTENSION,
                                       FLD_NAME_F_HASAUX,
                                       FLD_NAME_F_REFERENCE,
                                       FLD_NAME_F_OBJECTID,
                                       FLD_NAME_F_HASTHUMB,
                                       FLD_NAME_F_THUMBEXTEN,
                                       
                                       TABLE_NAME,
                                       FLD_NAME_F_OBJECTID,
                                       id);
            using (IDataReader reader = DBHelper.GlobalDBHelper.DoQuery(sql))
            {
                if (reader.Read())
                {
                    DataSnapshotDAL dal = new DataSnapshotDAL();
                    dal.ObjectID = id;
                    if (reader[FLD_NAME_F_XSIZE] != DBNull.Value)
                    {
                        dal.XSize = GetDoubleValue(reader, FLD_NAME_F_XSIZE);
                    }
                    dal.ID = int.Parse(reader[FLD_NAME_F_ID].ToString());
                    dal.Offset = double.Parse(reader[FLD_NAME_F_OFFSET].ToString());
                    dal.Rotation = double.Parse(reader[FLD_NAME_F_ROTATION].ToString());
                    dal.YSize = double.Parse(reader[FLD_NAME_F_YSIZE].ToString());
                    dal.XLeftTop = double.Parse(reader[FLD_NAME_F_XLEFTTOP].ToString());
                    dal.YLeftTop = double.Parse(reader[FLD_NAME_F_YLEFTTOP].ToString());
                    dal.SnapShotExtension = reader[FLD_NAME_F_EXTENSION].ToString();
                    dal.SpatialReferenceName = reader[FLD_NAME_F_REFERENCE].ToString();
                    dal.ThumbExtension = reader[FLD_NAME_F_THUMBEXTEN].ToString(); 
                    
                    if (reader[FLD_NAME_F_HASAUX] == DBNull.Value)
                    {
                        dal.HasAux = false;
                    }
                    else
                    {

                        dal.HasAux = Convert.ToInt32(reader[FLD_NAME_F_HASAUX]) > 0;
                    }
                    
                     if (reader[FLD_NAME_F_HASTHUMB] == DBNull.Value)
                    {
                        dal.HasThumb = false;
                    }
                    else
                    {

                        dal.HasThumb = Convert.ToInt32(reader[FLD_NAME_F_HASTHUMB]) > 0;
                    }
                    return dal;
                }
                else
                {
                    return null;
                }
            }
        }


        //public static DataSnapshotDAL SelectByID(int id)
        //{
        //    string sql = string.Format("select {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12} from {13} where {14}={15}",
        //                               FLD_NAME_F_ID,
        //                               FLD_NAME_F_XSIZE,
        //                               FLD_NAME_F_OFFSET,
        //                               FLD_NAME_F_ROTATION,
        //                               FLD_NAME_F_YSIZE,
        //                               FLD_NAME_F_XLEFTTOP,
        //                               FLD_NAME_F_YLEFTTOP,
        //                               FLD_NAME_F_EXTENSION,
        //                               FLD_NAME_F_HASAUX,
        //                               FLD_NAME_F_REFERENCE,
        //                               FLD_NAME_F_OBJECTID,
        //                               FLD_NAME_F_HASTHUMB,
        //                               FLD_NAME_F_THUMBEXTEN,

        //                               TABLE_NAME,
        //                               FLD_NAME_F_ID,
        //                               id);
        //    using (IDataReader reader = DBHelper.GlobalDBHelper.DoQuery(sql))
        //    {
        //        if (reader.Read())
        //        {
        //            DataSnapshotDAL dal = new DataSnapshotDAL();
        //            dal.ObjectID = id;
        //            if (reader[FLD_NAME_F_XSIZE] != DBNull.Value)
        //            {
        //                dal.XSize = GetDoubleValue(reader, FLD_NAME_F_XSIZE);
        //            }
        //            dal.ID = int.Parse(reader[FLD_NAME_F_ID].ToString());
        //            dal.Offset = double.Parse(reader[FLD_NAME_F_OFFSET].ToString());
        //            dal.Rotation = double.Parse(reader[FLD_NAME_F_ROTATION].ToString());
        //            dal.YSize = double.Parse(reader[FLD_NAME_F_YSIZE].ToString());
        //            dal.XLeftTop = double.Parse(reader[FLD_NAME_F_XLEFTTOP].ToString());
        //            dal.YLeftTop = double.Parse(reader[FLD_NAME_F_YLEFTTOP].ToString());
        //            dal.SnapShotExtension = reader[FLD_NAME_F_EXTENSION].ToString();
        //            dal.SpatialReferenceName = reader[FLD_NAME_F_REFERENCE].ToString();
        //            dal.ThumbExtension = reader[FLD_NAME_F_THUMBEXTEN].ToString();

        //            if (reader[FLD_NAME_F_HASAUX] == DBNull.Value)
        //            {
        //                dal.HasAux = false;
        //            }
        //            else
        //            {

        //                dal.HasAux = Convert.ToInt32(reader[FLD_NAME_F_HASAUX]) > 0;
        //            }

        //            if (reader[FLD_NAME_F_HASTHUMB] == DBNull.Value)
        //            {
        //                dal.HasThumb = false;
        //            }
        //            else
        //            {

        //                dal.HasThumb = Convert.ToInt32(reader[FLD_NAME_F_HASTHUMB]) > 0;
        //            }
        //            return dal;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}

        /// <summary>
        /// 删除影像记录
        /// </summary>
        /// <param name="dataID">影像对应数据ID</param>
        /// <returns></returns>
        public static bool Delete(int dataID)
        {
            return DBHelper.GlobalDBHelper.DeleteRow(TABLE_NAME, FLD_NAME_F_OBJECTID + "=" + dataID) >= 0;
        }

        /// <summary>
        /// 读取快视图影像文件到本地
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public bool ReadSnapShotRasterFile(string fileName)
        {
            try
            {
                DBHelper.GlobalDBHelper.ReadBlob2File(fileName, FLD_NAME_F_ID + "=" + _id, TABLE_NAME, FLD_NAME_F_IMAGE);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 读取拇指图影像文件到本地
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public bool ReadThumbRasterFile(string fileName)
        {
            try
            {
                DBHelper.GlobalDBHelper.ReadBlob2File(fileName, FLD_NAME_F_ID + "=" + _id, TABLE_NAME, FLD_NAME_F_THUMBIMAGE);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 存储影像文件到数据库
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public bool SaveSnapShotRasterFile(string fileName)
        {
            try
            {
                DBHelper.GlobalDBHelper.SaveFile2Blob(fileName, FLD_NAME_F_ID + "=" + _id, TABLE_NAME, FLD_NAME_F_IMAGE, false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 存储影像文件到数据库
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public bool SaveThumbRasterFile(string fileName)
        {
            try
            {
                DBHelper.GlobalDBHelper.SaveFile2Blob(fileName, FLD_NAME_F_ID + "=" + _id, TABLE_NAME, FLD_NAME_F_THUMBIMAGE, false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 读取AUX文件到本地
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public bool ReadAuxFile(string fileName)
        {
            try
            {
                DBHelper.GlobalDBHelper.ReadBlob2File(fileName, FLD_NAME_F_ID + "=" + _id, TABLE_NAME, FLD_NAME_F_AUX);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 保存AUX文件到数据库
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public bool SaveAuxFile(string fileName)
        {
            try
            {
                DBHelper.GlobalDBHelper.SaveFile2Blob(fileName, FLD_NAME_F_ID + "=" + _id, TABLE_NAME, FLD_NAME_F_AUX, false);
                return true;
            }
            catch
            {
                throw;
            }
        }

        private static double GetDoubleValue(IDataReader reader, string fldName)
        {
            if (reader[FLD_NAME_F_XSIZE] != DBNull.Value)
            {
                return double.Parse(reader[FLD_NAME_F_XSIZE].ToString());
            }
            else
            {
                return .0;
            }
        }

        /// <summary>
        /// 指定的影像数据是否存在
        /// </summary>
        /// <param name="rasterID">影像ID</param>
        /// <returns></returns>
        public static bool IsExist(IDBHelper db, int dataId)
        {
            string sql = string.Format("SELECT COUNT({0}) FROM {1} WHERE {2}={3} AND {4} IS NOT NULL",
                                       FLD_NAME_F_ID,
                                       TABLE_NAME,
                                       FLD_NAME_F_OBJECTID,
                                       dataId,
                                       FLD_NAME_F_IMAGE);
            using (IDataReader reader = db.DoQuery(sql))
            {
                if (reader.Read())
                {
                    return reader.GetInt32(0) > 0;
                }
            }
            return false;
        }
    }
}
