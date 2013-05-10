using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Data;
using Geoway.ADF.MIS.DB.Public;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    public class DataAccess
    {
        private static DBHelper db;
        public static DBHelper DB
        {
            set
            {
                db = value;
                //DBOper.DbSystem = db;
            }
            get
            {
                return db;
            }
        }

        public static IWorkspace TargetWorkspace;
    }
}
