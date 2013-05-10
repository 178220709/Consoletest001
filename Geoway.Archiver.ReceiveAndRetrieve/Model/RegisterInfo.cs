using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Interface;


namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    public abstract class RegisterInfo : IRegisterDBOper<RegisterInfo>
    {
        int id;

        public int ID //数据标志
        {
            get { return id; }
            set { id = value; }
        }

        int metaID;  //元数据ID

        public int MetaID
        {
            get { return metaID; }
            set { metaID = value; }
        }

        string metaTableName;//元数据表ID

        public string MetaTableName
        {
            get { return metaTableName; }
            set { metaTableName = value; }
        }

        IGeometry geometry;//数据真实范围

        public IGeometry Geometry
        {
            get { return geometry; }
            set { geometry = value; }
        }

        string geography; //地理标志符

        public string Geography
        {
            get { return geography; }
            set { geography = value; }
        }

        int year; //数据年代

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        long datasize;//数据大小，以kb为单位

        public long DataSize
        {
            get { return datasize; }
            set { datasize = value; }
        }

        EnumObjectState flag;//数据状态(是否删除)

        public EnumObjectState Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        double xMin;

        public double MinX
        {
            get { return xMin; }
            set { xMin = value; }
        }
        double xMax;

        public double MaxX
        {
            get { return xMax; }
            set { xMax = value; }
        }
        double yMin;

        public double MinY
        {
            get { return yMin; }
            set { yMin = value; }
        }
        double yMax;

        public double MaxY
        {
            get { return yMax; }
            set { yMax = value; }
        }


        #region IDBOper<RegisterInfo> 成员
        public abstract bool Add();

        public abstract bool Delete();

        public abstract bool Update();

        #endregion
    }
}
