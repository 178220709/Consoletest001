using System.Data.Entity;
using System.Linq;
using JsonSong.SpiderApp.Data;

namespace JsonSong.SpiderApp.Base
{
    public partial class MyDbContext : DbContext
    {
        private static MyDbContext _instance = new MyDbContext();

        public static IQueryable<T> GetQuery<T>() where T: BaseEntity
        {
            return _instance.Set<T>();
        }


        public MyDbContext()
            : base("conn")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("qds114441695_db");
        }

        /// <summary>
        /// 组织架构
        /// </summary>
        public DbSet<SpiderEntity> Spiders { get; set; }

    }
}
