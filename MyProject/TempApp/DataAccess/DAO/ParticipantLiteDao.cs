using System;
using System.Collections.Generic;
using JsonSong.BaseDao.LiteDb;
using MyProject.TempApp.DataAccess.Entity;
using Omu.ValueInjecter;

namespace MyProject.TempApp.DataAccess.DAO
{
    public class ParticipantLiteDao : BaseLiteDao<ParticipantEntity>
    {
        private ParticipantLiteDao(string path, string cnName)
            : base(path, cnName)
        {
            
        }

        private static ParticipantLiteDao _instance;

        public static ParticipantLiteDao Instance
        {
            get
            {
                return _instance ?? (_instance = new ParticipantLiteDao("temp", "participant"));
            }
        }

        public void AddNoRepeat(string name )
        {
            var entity =  GetByName(name);
            if (entity == null)
            {
                var en = new ParticipantEntity()
                {
                    Name = name
                };
                Insert(en);
            }
            else
            {
                return;
            }
        }

        private static ParticipantEntity GetByName(string name)
        {
            return Instance.FindOne(a => a.Name == name);
        }
    }
}