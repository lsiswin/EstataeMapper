using EstateMapperLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace EstateMapperWeb.Context
{
    public class MyContext : DbContext
    {
        public DbSet<House> House { get; set; }
        public DbSet<Layout> Layouts { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<SubRegion> SubRegions { get; set; }

        public MyContext(DbContextOptions options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // House ↔ SubRegion 配置
            modelBuilder.Entity<House>()
                .HasOne<SubRegion>()                // House 属于一个 SubRegion
                .WithMany(s => s.Houses)            // SubRegion 拥有多个 House
                .HasForeignKey(h => h.SubRegionId)  // 外键为 House.SubRegionId
                .OnDelete(DeleteBehavior.Restrict); // 禁止级联删除（或根据需求调整）
            // House ↔ Tags 多对多配置
            modelBuilder.Entity<House>()
                .HasMany(h => h.Tags)
                .WithOne()
                .HasForeignKey(h => h.HouseId)
                .OnDelete(DeleteBehavior.Cascade);

            // House ↔ Layouts 一对多配置
            modelBuilder.Entity<House>()
                .HasMany(h => h.Layouts)
                .WithOne()
                .HasForeignKey(l => l.HouseId)
                .OnDelete(DeleteBehavior.Cascade); // 级联删除


            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("Regions");
                entity.HasKey(r => r.RegionId);
                entity.Property(r => r.RegionId)
                    .HasColumnName("RegionId"); // 明确指定列名

                //entity.HasData(
                //    new Region { RegionId = 420100, Name = "武汉市" },
                //    new Region { RegionId = 420200, Name = "黄石市" },
                //    new Region { RegionId = 420300, Name = "十堰市" },
                //    new Region { RegionId = 420500, Name = "宜昌市" },
                //    new Region { RegionId = 420600, Name = "襄阳市" },
                //    new Region { RegionId = 420700, Name = "鄂州市" },
                //    new Region { RegionId = 420800, Name = "荆门市" },
                //    new Region { RegionId = 420900, Name = "孝感市" },
                //    new Region { RegionId = 421000, Name = "荆州市" },
                //    new Region { RegionId = 421100, Name = "黄冈市" },
                //    new Region { RegionId = 421200, Name = "咸宁市" },
                //    new Region { RegionId = 421300, Name = "随州市" },
                //    new Region { RegionId = 422800, Name = "恩施土家族苗族自治州" },
                //    new Region { RegionId = 429004, Name = "仙桃市" },
                //    new Region { RegionId = 429005, Name = "潜江市" },
                //    new Region { RegionId = 429006, Name = "天门市" },
                //    new Region { RegionId = 429021, Name = "神农架林区" }
                //);
            });

            modelBuilder.Entity<SubRegion>(entity =>
            {
                entity.ToTable("SubRegions");
                entity.HasKey(sr => sr.SubRegionId);
                entity.Property(sr => sr.SubRegionId)
                    .HasColumnName("SubRegionId"); // 明确指定列名

                // 外键关系
                entity.HasOne(sr => sr.Region)
                    .WithMany(r => r.SubRegions)
                    .HasForeignKey(sr => sr.RegionId);
                entity.HasKey(sr => sr.SubRegionId);

                //entity.HasData(
                //    // 武汉市辖区
                //    new { SubRegionId = 420101, Name = "江岸区", RegionId = 420100 },
                //    new { SubRegionId = 420102, Name = "江汉区", RegionId = 420100 },
                //    new { SubRegionId = 420103, Name = "硚口区", RegionId = 420100 },
                //    new { SubRegionId = 420104, Name = "汉阳区", RegionId = 420100 },
                //    new { SubRegionId = 420105, Name = "武昌区", RegionId = 420100 },
                //    new { SubRegionId = 420106, Name = "青山区", RegionId = 420100 },
                //    new { SubRegionId = 420107, Name = "洪山区", RegionId = 420100 },
                //    new { SubRegionId = 420108, Name = "东西湖区", RegionId = 420100 },
                //    new { SubRegionId = 420109, Name = "汉南区", RegionId = 420100 },
                //    new { SubRegionId = 420110, Name = "蔡甸区", RegionId = 420100 },
                //    new { SubRegionId = 420111, Name = "江夏区", RegionId = 420100 },
                //    new { SubRegionId = 420112, Name = "黄陂区", RegionId = 420100 },
                //    new { SubRegionId = 420113, Name = "新洲区", RegionId = 420100 },

                //    // 黄石市辖区（示例）
                //    new { SubRegionId = 420201, Name = "黄石港区", RegionId = 420200 },
                //    new { SubRegionId = 420202, Name = "西塞山区", RegionId = 420200 },
                //    new { SubRegionId = 420203, Name = "下陆区", RegionId = 420200 },
                //    new { SubRegionId = 420204, Name = "铁山区", RegionId = 420200 },
                //    new { SubRegionId = 420205, Name = "阳新县", RegionId = 420200 },
                //    new { SubRegionId = 420206, Name = "大冶市", RegionId = 420200 },

                //    // 其他城市按照相同模式添加...
                //    // 省直辖特殊处理
                //    new { SubRegionId = 429004, Name = "仙桃市", RegionId = 429004 },
                //    new { SubRegionId = 429005, Name = "潜江市", RegionId = 429005 },
                //    new { SubRegionId = 429006, Name = "天门市", RegionId = 429006 },
                //    new { SubRegionId = 429021, Name = "神农架林区", RegionId = 429021 }
                //);
            });

          

        }
    }
}
