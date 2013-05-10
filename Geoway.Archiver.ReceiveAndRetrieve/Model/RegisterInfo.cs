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

        public int ID //���ݱ�־
        {
            get { return id; }
            set { id = value; }
        }

        int metaID;  //Ԫ����ID

        public int MetaID
        {
            get { return metaID; }
            set { metaID = value; }
        }

        string metaTableName;//Ԫ���ݱ�ID

        public string MetaTableName
        {
            get { return metaTableName; }
            set { metaTableName = value; }
        }

        IGeometry geometry;//������ʵ��Χ

        public IGeometry Geometry
        {
            get { return geometry; }
            set { geometry = value; }
        }

        string geography; //�����־��

        public string Geography
        {
            get { return geography; }
            set { geography = value; }
        }

        int year; //�������

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        long datasize;//���ݴ�С����kbΪ��λ

        public long DataSize
        {
            get { return datasize; }
            set { datasize = value; }
        }

        EnumObjectState flag;//����״̬(�Ƿ�ɾ��)

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


        #region IDBOper<RegisterInfo> ��Ա
        public abstract bool Add();

        public abstract bool Delete();

        public abstract bool Update();

        #endregion
    }
}
