using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using JsonSong.Front.Extend;
using LiteDB;
using LiteDbLog.Facade;
using LiteDbLog.LiteDBLog;
using Suijing.Utils.Utility;

namespace JsonSong.Front.Models.System
{

    public  class DBLogModel :BasePagerModel<LiteDbLog.LiteDBLog.DBLogEntity>
    {
        public static IList<string> NameEnum = DBLogInstances.GetAllInstanceName();
        public static IList<SelectListItem> LevelEnum = EnumDescHelper.GetEnumSelectItemList(typeof (DBLogLevelEnum));

        public DBLogLevelEnum Level { get; set; }
        public string Name { get; set; }

        public string Content { get; set; }

    }
}
