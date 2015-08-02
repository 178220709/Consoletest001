using JsonSong.BaseDao.LiteDb;

namespace MyProject.TempApp.DataAccess.Entity
{
    public class ParticipantEntity : BaseLiteEntity
    {
        public ParticipantEntity()
        {
        }

        public string Name { get; set; }
    }
}