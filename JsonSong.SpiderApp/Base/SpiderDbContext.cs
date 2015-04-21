using System.Data.Entity;
using JsonSong.SpiderApp.Data;

namespace JsonSong.SpiderApp.Base
{
    public partial class SpiderDbContext : DbContext
    {
        public SpiderDbContext()
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
