using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonSong.BaseDao.LiteDb;

namespace LiteDbLog.LiteDBLog
{
    public class DBLogEntity :BaseLiteEntity
    {
     
        public DBLogLevelEnum Level { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Exception Exception { get; set; }

        public string Name { get; set; }
    }

    public enum DBLogLevelEnum
    {
      
        Debugger=1,
        Info = 2,
        Warn = 4,
        Error=5,
    }
}
