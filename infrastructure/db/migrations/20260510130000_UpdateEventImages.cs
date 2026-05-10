using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hcktn.infrastructure.db.migrations
{
    public partial class UpdateEventImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE ""EventImages"" SET ""Url"" = 'https://vikna.tv/wp-content/uploads/2026/04/14/pexels-olha-maltseva-2156976676-34926383.jpg' WHERE ""EventId"" = 1;
UPDATE ""EventImages"" SET ""Url"" = 'https://climbingbusinessjournal.com/wp-content/uploads/2024/03/EP-Walls-Web-Enhanced-78-scaled.jpg' WHERE ""EventId"" = 2;
UPDATE ""EventImages"" SET ""Url"" = 'https://dovzhenkocentre.org/wp-content/uploads/2025/04/golosy.jpg' WHERE ""EventId"" = 3;
UPDATE ""EventImages"" SET ""Url"" = 'https://thenudge.com/wp-content/uploads/2025/10/DSCF5517-scaled-e1760958817595.jpg' WHERE ""EventId"" = 4;
UPDATE ""EventImages"" SET ""Url"" = 'https://24tv.ua/resources/photos/news/202202/1857400.jpg?v=1661256897000&w=1200&h=675' WHERE ""EventId"" = 5;
UPDATE ""EventImages"" SET ""Url"" = 'https://agropolit.com/media/news/original/00/26/26456/ribolovstvo-34211.jpg' WHERE ""EventId"" = 6;
UPDATE ""EventImages"" SET ""Url"" = 'https://voevodyno.com/wp-content/uploads/2024/06/pohod-2-optimized.jpg' WHERE ""EventId"" = 7;
UPDATE ""EventImages"" SET ""Url"" = 'https://archive.kyivpost.com/wp-content/uploads/2018/11/02_PET_0311-800x520.jpg' WHERE ""EventId"" = 8;
UPDATE ""EventImages"" SET ""Url"" = 'https://sud.ua/uk/news/ukraine/306573-kakie-vidy-pensii-predusmotreny-dlya-voennykh-spisok' WHERE ""EventId"" = 9;
UPDATE ""EventImages"" SET ""Url"" = 'https://green4.photo/wp-content/new_uploads//2022/10/apc_0239-2.jpg' WHERE ""EventId"" = 10;
UPDATE ""EventImages"" SET ""Url"" = 'https://images.pexels.com/photos/4760051/pexels-photo-4760051.jpeg?auto=compress&cs=tinysrgb&h=627&fit=crop&w=1200' WHERE ""EventId"" = 11;
UPDATE ""EventImages"" SET ""Url"" = 'https://static.tildacdn.one/tild6338-6535-4235-a233-313130343133/IMG_1670.jpg' WHERE ""EventId"" = 12;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE ""EventImages"" SET ""Url"" = '/images/grant.jpg' WHERE ""EventId"" = 1;
UPDATE ""EventImages"" SET ""Url"" = '/images/climbing.jpg' WHERE ""EventId"" = 2;
UPDATE ""EventImages"" SET ""Url"" = '/images/theatre.jpg' WHERE ""EventId"" = 3;
UPDATE ""EventImages"" SET ""Url"" = '/images/ceramics.jpg' WHERE ""EventId"" = 4;
UPDATE ""EventImages"" SET ""Url"" = '/images/it-course.jpg' WHERE ""EventId"" = 5;
UPDATE ""EventImages"" SET ""Url"" = '/images/fishing.jpg' WHERE ""EventId"" = 6;
UPDATE ""EventImages"" SET ""Url"" = '/images/hiking.jpg' WHERE ""EventId"" = 7;
UPDATE ""EventImages"" SET ""Url"" = '/images/coffee.jpg' WHERE ""EventId"" = 8;
UPDATE ""EventImages"" SET ""Url"" = '/images/finance.jpg' WHERE ""EventId"" = 9;
UPDATE ""EventImages"" SET ""Url"" = '/images/photo.jpg' WHERE ""EventId"" = 10;
UPDATE ""EventImages"" SET ""Url"" = '/images/yoga.jpg' WHERE ""EventId"" = 11;
UPDATE ""EventImages"" SET ""Url"" = '/images/walk.jpg' WHERE ""EventId"" = 12;
");
        }
    }
}
