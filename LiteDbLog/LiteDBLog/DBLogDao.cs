using System;
using System.ComponentModel;
using JsonSong.BaseDao.LiteDb;

namespace LiteDbLog.LiteDBLog
{
    public class DBLogDao : BaseLiteDao<DBLogEntity>
    {
        private const string DBName = "dblog";
        private readonly string _cnName = "";
        public DBLogDao(string cnName):base(DBName,cnName)
        {
            this._cnName = cnName;
        }


        public void Info(string content)
        {
             Log(content);
        }

        public void Log(string content, string title = "", DBLogLevelEnum level = DBLogLevelEnum.Info)
        {
            var en = new DBLogEntity()
            {
                Name = _cnName,
                Content = content,
                Title = title,
                Level = level
            };
            Insert(en);
        }

        public void Error(string content, Exception ex, string title = "", DBLogLevelEnum level = DBLogLevelEnum.Error)
        {
            var en = new DBLogEntity()
            {
                Name = _cnName,
                Content = content,
                Title = title,
                Level = level,
                Exception = ex
            };
            Insert(en);
        }
    }
}
