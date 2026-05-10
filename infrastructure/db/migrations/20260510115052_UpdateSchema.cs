using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace hcktn.infrastructure.db.migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clear old fixture data so schema changes can proceed cleanly
            migrationBuilder.Sql(@"
TRUNCATE TABLE ""EventImages"", ""EventTags"", ""OrganisationCredentials"", ""Events"", ""Organisations"", ""Tags"", ""Cities"" RESTART IDENTITY CASCADE;
");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Tags",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactAddress",
                table: "Organisations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVeteran",
                table: "Organisations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePhoto",
                table: "Organisations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BarrierFreeUrl",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Capacity",
                table: "Events",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "GoogleMeetUrl",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "InclusivityIds",
                table: "Events",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Events",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationLink",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Events",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingType",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "PriceId",
                table: "Events",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Recurrence",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "TransferAvailable",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TransferDetails",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PriceType = table.Column<string>(type: "text", nullable: false),
                    PriceValue = table.Column<long>(type: "bigint", nullable: true),
                    PriceNotes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registrations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Suggestions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suggestions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_PriceId",
                table: "Events",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_EventId",
                table: "Registrations",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Prices_PriceId",
                table: "Events",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // ─── Seed: Cities ──────────────────────────────────────────────────
            migrationBuilder.Sql(@"
INSERT INTO ""Cities"" (""Name"") VALUES
    ('Київ'), ('Львів'), ('Одеса'), ('Дніпро'), ('Вінниця'), ('Івано-Франківськ');
");

            // ─── Seed: Tags ────────────────────────────────────────────────────
            migrationBuilder.Sql(@"
INSERT INTO ""Tags"" (""Name"", ""Color"") VALUES
    ('Безкоштовно', '#E6F3CC'),
    ('Інклюзивність', '#DEF1FF'),
    ('Спорт', '#FFE4CC'),
    ('Театр', '#E6E3FF'),
    ('Малі групи', '#FAEDCE'),
    ('Можна з близькими', '#FFD6E0'),
    ('Грант', '#D4F5D4'),
    ('Онлайн', '#DEF1FF'),
    ('Реабілітація', '#E0F0E0'),
    ('Навчання', '#FFF3D6'),
    ('Книги', '#E6E3FF'),
    ('Музика', '#FFD6E0'),
    ('Рукоділля', '#FAEDCE'),
    ('Ветеранський хаб', '#D4F5D4'),
    ('Фестиваль', '#FFE4CC');
");

            // ─── Seed: Organisations ───────────────────────────────────────────
            migrationBuilder.Sql(@"
INSERT INTO ""Organisations"" (""Name"", ""Phone"", ""ContactInfo"", ""Status"", ""IsVeteran"") VALUES
    ('Ветеранський хаб Київ', '+380501234567', 'м. Київ, вул. Хрещатик 1', 1, true),
    ('Фундація ""Повернись живим""', '+380671234567', 'м. Київ', 1, false),
    ('Артпростір ""Культура""', '', 'м. Львів, вул. Коперника 11', 1, false),
    ('Центр ветеранських ініціатив', '', 'м. Одеса, вул. Дерибасівська 5', 1, true),
    ('Побратими', '', 'м. Дніпро', 1, true),
    ('Veteran Hub Київ', '', 'м. Київ, вул. Верхній Вал, 2а', 1, true),
    ('Veteran Hub Вінниця', '', 'м. Вінниця', 1, true),
    ('Ukraїner', '', 'м. Київ', 1, false),
    ('UNIT.City', '', 'м. Київ, вул. Дорогожицька, 3', 1, false),
    ('Faine Events', '', 'м. Львів', 1, false);
");

            // ─── Seed: OrganisationCredentials ─────────────────────────────────
            migrationBuilder.Sql(@"
INSERT INTO ""OrganisationCredentials"" (""OrganisationId"", ""Login"", ""PasswordHash"") VALUES
    (1, 'org1', '$2a$11$EsMP5bbDHD/Uqg35J/1Am.zgOS0ssyIExkmkISwBnS/Ls6oSv0qv2'),
    (2, 'org2', '$2a$11$xpiGYzFLWo/95PIwwY2vq.WPmnTBLJyWBCVcNV1jimvB.UXq/5ETW'),
    (3, 'org3', '$2a$11$5OnDMaRx98GkQ6PDgkTROeDXXfTIpUUiEJLqYeIiS4jLbsHpgJgxG'),
    (4, 'org4', '$2a$11$UbGVfUrkP0BFA/eK4Rma6O7MwprWEyOd2oBkg/FreV5byYylHb5MC'),
    (5, 'org5', '$2a$11$/gMu6EVGocdrMY0jmMSwc.c8LRUuT/dW/CWyj6l5TpJj3VwNiG1w2'),
    (6, 'org6', '$2a$11$jtK672wOvB6vGn3bgOeEx.tV58CJlIX0bTCdBNqCoHkZB2ThqU.MW'),
    (7, 'org7', '$2a$11$v.6W7qmVpKXLpvLu/7QR6OwXjv77QfsCcXEaQJY9vZ9LvAQG6joB.'),
    (8, 'org8', '$2a$11$WQicSFiVUx8uRiLzYZ4mdu4oroSGC5jlFRyZZAQuoosNpFRoA.pIm'),
    (9, 'org9', '$2a$11$tKCJOyxbod23f.OszL5YlOomP2ZWH6N55J2UjlRblS.D6scc3tw5C'),
    (10, 'org10', '$2a$11$HG86Ftg8lNcEhgLLvl7fv.l2ubacz9Hy6.IWhf977Cb12Y82Jk6G2');
");

            // ─── Seed: Prices (one per event, in event order) ──────────────────
            migrationBuilder.Sql(@"
INSERT INTO ""Prices"" (""PriceType"", ""PriceValue"", ""PriceNotes"") VALUES
    ('free', NULL, 'Безкоштовна участь'),
    ('free', NULL, 'Безкоштовно для ветеранів'),
    ('free', NULL, 'Безкоштовно для ветеранів, 200 грн для цивільних'),
    ('free', NULL, NULL),
    ('free', NULL, NULL),
    ('free', NULL, 'Трансфер та спорядження включені'),
    ('partially_free', 500, '500 грн внесок на транспорт, решта безкоштовно'),
    ('free', NULL, 'Кава включена'),
    ('free', NULL, NULL),
    ('free', NULL, NULL),
    ('free', NULL, NULL),
    ('free', NULL, NULL),
    ('free', NULL, NULL),
    ('free', NULL, NULL),
    ('free', NULL, NULL),
    ('free', NULL, NULL),
    ('free', NULL, NULL),
    ('free', NULL, NULL),
    ('free', NULL, NULL),
    ('free', NULL, 'Вхід вільний'),
    ('partially_free', 800, 'Безкоштовно для ветеранів за промокодом'),
    ('discounted', 300, '300 грн для ветеранів (звичайна 600 грн)'),
    ('free', NULL, 'Безкоштовно для ветеранів'),
    ('free', NULL, NULL),
    ('free', NULL, 'Кава та снеки включені');
");

            // ─── Seed: Events ──────────────────────────────────────────────────
            migrationBuilder.Sql(@"
INSERT INTO ""Events"" (""Title"", ""Description"", ""PriceId"", ""IdOrganisation"", ""StartDate"", ""EndDate"", ""CreatedAt"",
    ""CityId"", ""Latitude"", ""Longitude"", ""Address"", ""LocationLink"",
    ""MeetingType"", ""GoogleMeetUrl"", ""Recurrence"", ""Capacity"",
    ""TransferAvailable"", ""TransferDetails"", ""InclusivityIds"", ""BarrierFreeUrl"") VALUES
(
    'Грант для ветеранів та членів їхніх сімей',
    'Програма грантової підтримки для ветеранів та членів їхніх сімей. Фінансування до 50 000 грн на реалізацію власних проєктів у сфері соціального підприємництва, творчості або спорту. Подача заявок до 15 червня.',
    1, 2, '2026-05-25 17:30:00+00', '2026-05-25 20:30:00+00', NOW(),
    1, 50.4501, 30.5234, 'м. Київ, вул. Хрещатик 22', NULL,
    'offline', NULL, 'none', 50,
    false, NULL, ARRAY['wheelchair', 'hearing'], 'https://lun.ua/misto/barrier-free/144#16/50.446515/30.495929'
),
(
    'Тренування з адаптивного скелелазіння',
    'Щотижневі тренування з адаптивного скелелазіння для ветеранів з інвалідністю та без. Досвідчені інструктори, все спорядження надається. Підходить для початківців.',
    2, 1, '2026-05-17 10:00:00+00', '2026-05-17 12:00:00+00', NOW(),
    1, 50.4547, 30.5238, 'м. Київ, Скеледром ""Вертикаль"", вул. Січових Стрільців 50', NULL,
    'offline', NULL, 'weekly', 15,
    false, NULL, ARRAY['wheelchair'], NULL
),
(
    'Театральна вистава ""Голоси"" — прем''єра',
    'Документальна вистава створена за реальними історіями ветеранів. Після вистави — відкрита дискусія з акторами та авторами. Камерний формат, спокійна атмосфера.',
    3, 3, '2026-05-20 19:00:00+00', '2026-05-20 21:30:00+00', NOW(),
    2, 49.8397, 24.0297, 'м. Львів, Артпростір ""Культура"", вул. Коперника 11', NULL,
    'offline', NULL, 'none', 80,
    false, NULL, ARRAY['wheelchair', 'hearing'], 'https://lun.ua/misto/barrier-free/20657#16/49.839726/24.030563'
),
(
    'Курс кераміки для ветеранів',
    'Арт-терапевтичний курс кераміки. 8 занять, малі групи до 8 осіб. Усі матеріали включені. Доведено позитивний вплив на зниження тривожності та ПТСР.',
    4, 4, '2026-05-19 14:00:00+00', '2026-05-19 16:00:00+00', NOW(),
    3, 46.4825, 30.7233, 'м. Одеса, Студія ""Глина"", вул. Катерининська 35', NULL,
    'offline', NULL, 'weekly', 8,
    false, NULL, ARRAY[]::text[], NULL
),
(
    'IT-курс: Основи програмування для ветеранів',
    'Безкоштовний 3-місячний курс з основ веб-розробки. HTML, CSS, JavaScript. Онлайн-формат, можна приєднатись з будь-якого міста. Видається сертифікат.',
    5, 5, '2026-06-01 18:00:00+00', '2026-06-01 20:00:00+00', NOW(),
    4, 48.4647, 35.0462, 'Онлайн (Zoom)', NULL,
    'online', 'https://zoom.us/j/example', 'biweekly', NULL,
    false, NULL, ARRAY[]::text[], NULL
),
(
    'Рибалка на Дніпрі — ветеранський вікенд',
    'Дводенна рибалка для ветеранів та їхніх сімей. Все спорядження надається. Трансфер з Києва включений. Обмежена кількість місць.',
    6, 1, '2026-05-31 07:00:00+00', '2026-06-01 18:00:00+00', NOW(),
    1, 50.3541, 30.8889, 'Київська обл., с. Вишгород, берег Дніпра', NULL,
    'offline', NULL, 'none', 20,
    true, 'Автобус з м. Київ, пл. Перемоги о 06:30', ARRAY[]::text[], NULL
),
(
    'Похід у Карпати: маршрут для початківців',
    'Одноденний похід для ветеранів — маршрут середньої складності. Досвідчений гід-ветеран. Група до 12 осіб. Спокійний темп, є можливість скоротити маршрут.',
    7, 5, '2026-06-07 06:00:00+00', '2026-06-07 20:00:00+00', NOW(),
    6, 48.6208, 24.0945, 'Івано-Франківська обл., Говерла, початок маршруту', NULL,
    'offline', NULL, 'none', 12,
    true, 'Мікроавтобус з Івано-Франківська о 05:00', ARRAY[]::text[], NULL
),
(
    'Кава з ветераном: неформальна зустріч',
    'Щомісячна зустріч у форматі ""кава з ветераном"". Можна прийти просто поговорити, познайомитись. Без формальностей, без програми. Кава за рахунок організаторів.',
    8, 4, '2026-05-24 11:00:00+00', '2026-05-24 14:00:00+00', NOW(),
    3, 46.4843, 30.7407, 'м. Одеса, кав''ярня ""Мазагран"", вул. Дерибасівська 18', NULL,
    'offline', NULL, 'monthly', NULL,
    false, NULL, ARRAY['wheelchair'], 'https://lun.ua/misto/barrier-free/25443#16/46.484300/30.740700'
),
(
    'Онлайн-лекція: Фінансова грамотність для ветеранів',
    'Практичний вебінар: як управляти фінансами після служби, які є державні програми підтримки, як отримати пільговий кредит.',
    9, 2, '2026-05-22 19:00:00+00', '2026-05-22 20:30:00+00', NOW(),
    1, 50.4501, 30.5234, 'Онлайн (Google Meet)', NULL,
    'online', 'https://meet.google.com/example', 'none', NULL,
    false, NULL, ARRAY[]::text[], NULL
),
(
    'Воркшоп з фотографії: Мистецтво бачити',
    'Одноденний воркшоп для ветеранів, які хочуть спробувати себе у фотографії. Камери надаються. Практична зйомка в місті + огляд робіт.',
    10, 3, '2026-05-26 10:00:00+00', '2026-05-26 16:00:00+00', NOW(),
    2, 49.8397, 24.0297, 'м. Львів, Центр сучасного мистецтва, вул. Стефаника 3', NULL,
    'offline', NULL, 'none', 10,
    false, NULL, ARRAY['wheelchair'], NULL
),
(
    'Йога для ветеранів: м''яка практика',
    'Адаптивна йога для ветеранів. Підходить для людей з обмеженою рухливістю. Досвідчений інструктор з травма-інформованим підходом.',
    11, 1, '2026-05-18 09:00:00+00', '2026-05-18 10:30:00+00', NOW(),
    1, 50.4501, 30.5234, 'м. Київ, Ветеранський хаб, вул. Саксаганського 42', NULL,
    'offline', NULL, 'weekly', 15,
    false, NULL, ARRAY['wheelchair'], NULL
),
(
    'Прогулянка Києвом: маршрут ""Невідомий Поділ""',
    'Пішохідна екскурсія маловідомими місцями Подолу. Гід — історик-ветеран. Спокійний темп, зупинки на каву. Доступний маршрут для візочників.',
    12, 1, '2026-05-24 15:00:00+00', '2026-05-24 18:00:00+00', NOW(),
    1, 50.4622, 30.5170, 'м. Київ, Контрактова площа, біля фонтану', NULL,
    'offline', NULL, 'none', 20,
    false, NULL, ARRAY['wheelchair'], NULL
),
(
    'Створення ляльок-мотанок',
    'Творче заняття з виготовлення традиційних українських ляльок-мотанок. Терапевтичний ефект ручної роботи, знайомство з українськими традиціями.',
    13, 7, '2026-05-23 14:00:00+00', '2026-05-23 17:00:00+00', NOW(),
    5, 49.2331, 28.4682, 'м. Вінниця, Veteran Hub', NULL,
    'offline', NULL, 'weekly', 12,
    false, NULL, ARRAY['wheelchair'], NULL
),
(
    'Заняття зі співу для воїнів та їхніх близьких',
    'Групові заняття зі співу в безпечному просторі. Не потрібен досвід — головне бажання. Спів як інструмент емоційного відновлення та соціалізації.',
    14, 6, '2026-05-21 18:00:00+00', '2026-05-21 20:00:00+00', NOW(),
    1, 50.4613, 30.5170, 'м. Київ, Veteran Hub, вул. Верхній Вал 2а', NULL,
    'offline', NULL, 'weekly', 20,
    false, NULL, ARRAY['wheelchair', 'hearing'], 'https://lun.ua/misto/barrier-free/144#16/50.461300/30.517000'
),
(
    'Майстер-клас акварельного живопису',
    'Навчання техніці акварелі для початківців. Арт-терапевтичний підхід: малювання як спосіб вираження емоцій. Усі матеріали надаються.',
    15, 7, '2026-05-22 15:00:00+00', '2026-05-22 17:30:00+00', NOW(),
    5, 49.2331, 28.4682, 'м. Вінниця, Veteran Hub', NULL,
    'offline', NULL, 'none', 10,
    false, NULL, ARRAY[]::text[], NULL
),
(
    'Книжковий клуб: обговорення І. Багряного',
    'Щомісячний книжковий клуб для ветеранів. Цього разу обговорюємо твори Івана Багряного. Камерний формат, чай та кава включені.',
    16, 6, '2026-05-28 19:00:00+00', '2026-05-28 21:00:00+00', NOW(),
    1, 50.4613, 30.5170, 'м. Київ, Veteran Hub, вул. Верхній Вал 2а', NULL,
    'offline', NULL, 'monthly', 15,
    false, NULL, ARRAY['wheelchair'], 'https://lun.ua/misto/barrier-free/144#16/50.461300/30.517000'
),
(
    'В''язання гачком — терапевтична група',
    'Заняття з в''язання гачком у форматі терапевтичної групи. Підходить для початківців. Спокійна атмосфера, розмови за рукоділлям.',
    17, 6, '2026-05-20 14:00:00+00', '2026-05-20 16:00:00+00', NOW(),
    1, 50.4613, 30.5170, 'м. Київ, Veteran Hub, вул. Верхній Вал 2а', NULL,
    'offline', NULL, 'weekly', 8,
    false, NULL, ARRAY['wheelchair'], NULL
),
(
    'Презентація «Кому вони потрібні?» — книга про полонених',
    'Обговорення нової книги есеїв Петра Яценка про російських військових в українському полоні. Дискусія з автором та ветеранами.',
    18, 8, '2026-05-14 18:00:00+00', '2026-05-14 20:00:00+00', NOW(),
    1, 50.4613, 30.5170, 'м. Київ, Veteran Hub, вул. Верхній Вал 2а', NULL,
    'offline', NULL, 'none', NULL,
    false, NULL, ARRAY['wheelchair'], NULL
),
(
    'Виставка «Перехідний стан» — фотоекспозиція',
    'Фотовиставка про трансформацію цивільного у військового і назад. Роботи фотожурналістів, які документували повсякденне життя військових. Вхід вільний.',
    19, 8, '2026-06-01 10:00:00+00', '2026-06-14 20:00:00+00', NOW(),
    1, 50.4501, 30.5234, 'м. Київ, книгарня «Сенс», вул. Хрещатик 34', NULL,
    'offline', NULL, 'none', NULL,
    false, NULL, ARRAY['wheelchair'], NULL
),
(
    'DEF.Talks — технології та оборона',
    'Мітап на перетині технологій та оборони. Спікери з Defense Tech стартапів, ветерани-підприємці. Нетворкінг, Q&A.',
    20, 9, '2026-06-05 18:30:00+00', '2026-06-05 21:00:00+00', NOW(),
    1, 50.4569, 30.4832, 'м. Київ, UNIT.City, UNIT.Core B8, вул. Дорогожицька 3', NULL,
    'offline', NULL, 'none', 100,
    false, NULL, ARRAY['wheelchair'], NULL
),
(
    'FAINE MISTO 2026 — музичний фестиваль',
    'Щорічний музичний фестиваль у Львові. 3 дні музики, їжі та спілкування. Спеціальні квитки для ветеранів — безкоштовно.',
    21, 10, '2026-08-01 12:00:00+00', '2026-08-03 23:00:00+00', NOW(),
    2, 49.8397, 24.0297, 'м. Львів, Парк культури', NULL,
    'offline', NULL, 'none', 5000,
    false, NULL, ARRAY['wheelchair'], NULL
),
(
    'Концерт VIVIENNE MORT — акустика',
    'Камерний акустичний концерт гурту Vivienne Mort. Інтимна атмосфера, сидячі місця. Спеціальна ціна для ветеранів.',
    22, 10, '2026-06-15 19:00:00+00', '2026-06-15 22:00:00+00', NOW(),
    2, 49.8397, 24.0297, 'м. Львів, Клуб ""Підземка""', NULL,
    'offline', NULL, 'none', 150,
    false, NULL, ARRAY[]::text[], NULL
),
(
    'Адаптивне плавання для ветеранів',
    'Тренування з адаптивного плавання під керівництвом параолімпійського тренера. Підходить для людей з ампутаціями та обмеженою рухливістю.',
    23, 1, '2026-05-19 09:00:00+00', '2026-05-19 11:00:00+00', NOW(),
    1, 50.4501, 30.5234, 'м. Київ, Спорткомплекс ""Олімпійський"", пров. Спортивний 1', NULL,
    'offline', NULL, 'biweekly', 10,
    false, NULL, ARRAY['wheelchair'], NULL
),
(
    'Майстерня з деревообробки',
    'Практичні заняття з деревообробки у столярній майстерні. Виготовлення виробів своїми руками. Трудотерапія з елементами навчання ремеслу.',
    24, 4, '2026-05-25 10:00:00+00', '2026-05-25 14:00:00+00', NOW(),
    4, 48.4647, 35.0462, 'м. Дніпро, Коворкінг ""Ремесло"", вул. Січеславська 45', NULL,
    'offline', NULL, 'weekly', 6,
    false, NULL, ARRAY[]::text[], NULL
),
(
    'Вечір настільних ігор для ветеранів',
    'Неформальна зустріч за настільними іграми. Стратегії, кооперативні ігри, класика. Без досвіду — навчимо. Перекус та напої включені.',
    25, 5, '2026-05-23 17:00:00+00', '2026-05-23 21:00:00+00', NOW(),
    4, 48.4647, 35.0462, 'м. Дніпро, Антикафе ""Час"", вул. Яворницького 12', NULL,
    'offline', NULL, 'biweekly', NULL,
    false, NULL, ARRAY[]::text[], NULL
);
");

            // ─── Seed: EventTags ───────────────────────────────────────────────
            migrationBuilder.Sql(@"
INSERT INTO ""EventTags"" (""EventId"", ""TagId"") VALUES
    (1,1),(1,2),(1,7),
    (2,1),(2,3),(2,2),
    (3,1),(3,4),(3,5),(3,6),
    (4,1),(4,5),(4,9),
    (5,1),(5,8),(5,10),
    (6,1),(6,6),
    (7,6),
    (8,1),(8,5),(8,6),
    (9,1),(9,8),(9,10),
    (10,1),(10,5),
    (11,1),(11,3),(11,5),(11,9),(11,2),
    (12,1),(12,2),(12,6),
    (13,1),(13,13),(13,14),
    (14,1),(14,12),(14,6),(14,14),
    (15,1),(15,5),(15,14),
    (16,1),(16,11),(16,5),(16,14),
    (17,1),(17,13),(17,5),(17,14),
    (18,1),(18,11),(18,14),
    (19,1),(19,2),
    (20,1),(20,10),
    (21,12),(21,15),(21,6),
    (22,4),(22,12),
    (23,1),(23,3),(23,2),(23,9),
    (24,1),(24,5),(24,9),
    (25,1),(25,5),(25,6);
");

            // ─── Seed: EventImages ─────────────────────────────────────────────
            migrationBuilder.Sql(@"
INSERT INTO ""EventImages"" (""EventId"", ""Url"") VALUES
    (1, '/images/grant.jpg'),
    (2, '/images/climbing.jpg'),
    (3, '/images/theatre.jpg'),
    (4, '/images/ceramics.jpg'),
    (5, '/images/it-course.jpg'),
    (6, '/images/fishing.jpg'),
    (7, '/images/hiking.jpg'),
    (8, '/images/coffee.jpg'),
    (9, '/images/finance.jpg'),
    (10, '/images/photo.jpg'),
    (11, '/images/yoga.jpg'),
    (12, '/images/walk.jpg'),
    (13, 'https://veteranhub.com.ua/wp-content/uploads/2026/05/event_2018.jpg'),
    (14, 'https://veteranhub.com.ua/wp-content/uploads/2026/04/event_1945-3.jpg'),
    (15, 'https://veteranhub.com.ua/wp-content/uploads/2026/05/event_1717.jpg'),
    (16, 'https://veteranhub.com.ua/wp-content/uploads/2026/05/event_2012.jpg'),
    (17, 'https://veteranhub.com.ua/wp-content/uploads/2025/11/event_1882-1.jpg'),
    (18, 'https://veteranhub.com.ua/wp-content/uploads/2026/05/event_2014.jpg'),
    (19, 'https://veteranhub.com.ua/wp-content/uploads/2026/05/event_1943.jpg'),
    (20, 'https://unit.city/wp-content/uploads/xsajt.png.pagespeed.ic.UIvFFJMwZV.png'),
    (21, 'https://faine.events/wp-content/uploads/2025/12/img_1569.jpeg'),
    (22, 'https://faine.events/wp-content/uploads/2026/03/img_9900-scaled.jpg'),
    (23, 'https://veteranhub.com.ua/wp-content/uploads/2026/04/event_1503-2.jpg'),
    (24, 'https://veteranhub.com.ua/wp-content/uploads/2026/05/event_2016.jpg'),
    (25, 'https://veteranhub.com.ua/wp-content/uploads/2026/05/event_1814-2.jpg');
");

            // ─── Seed: Registrations (matches registeredCount from fixtures) ───
            migrationBuilder.Sql(@"
-- Event 1: 23 registrations
INSERT INTO ""Registrations"" (""EventId"") SELECT 1 FROM generate_series(1,23);
-- Event 2: 11
INSERT INTO ""Registrations"" (""EventId"") SELECT 2 FROM generate_series(1,11);
-- Event 3: 45
INSERT INTO ""Registrations"" (""EventId"") SELECT 3 FROM generate_series(1,45);
-- Event 4: 6
INSERT INTO ""Registrations"" (""EventId"") SELECT 4 FROM generate_series(1,6);
-- Event 5: 34
INSERT INTO ""Registrations"" (""EventId"") SELECT 5 FROM generate_series(1,34);
-- Event 6: 18
INSERT INTO ""Registrations"" (""EventId"") SELECT 6 FROM generate_series(1,18);
-- Event 7: 9
INSERT INTO ""Registrations"" (""EventId"") SELECT 7 FROM generate_series(1,9);
-- Event 8: 7
INSERT INTO ""Registrations"" (""EventId"") SELECT 8 FROM generate_series(1,7);
-- Event 9: 52
INSERT INTO ""Registrations"" (""EventId"") SELECT 9 FROM generate_series(1,52);
-- Event 10: 4
INSERT INTO ""Registrations"" (""EventId"") SELECT 10 FROM generate_series(1,4);
-- Event 11: 12
INSERT INTO ""Registrations"" (""EventId"") SELECT 11 FROM generate_series(1,12);
-- Event 12: 14
INSERT INTO ""Registrations"" (""EventId"") SELECT 12 FROM generate_series(1,14);
-- Event 13: 5
INSERT INTO ""Registrations"" (""EventId"") SELECT 13 FROM generate_series(1,5);
-- Event 14: 13
INSERT INTO ""Registrations"" (""EventId"") SELECT 14 FROM generate_series(1,13);
-- Event 15: 7
INSERT INTO ""Registrations"" (""EventId"") SELECT 15 FROM generate_series(1,7);
-- Event 16: 8
INSERT INTO ""Registrations"" (""EventId"") SELECT 16 FROM generate_series(1,8);
-- Event 17: 6
INSERT INTO ""Registrations"" (""EventId"") SELECT 17 FROM generate_series(1,6);
-- Event 18: 32
INSERT INTO ""Registrations"" (""EventId"") SELECT 18 FROM generate_series(1,32);
-- Event 19: 18
INSERT INTO ""Registrations"" (""EventId"") SELECT 19 FROM generate_series(1,18);
-- Event 20: 67
INSERT INTO ""Registrations"" (""EventId"") SELECT 20 FROM generate_series(1,67);
-- Event 21: 2340
INSERT INTO ""Registrations"" (""EventId"") SELECT 21 FROM generate_series(1,2340);
-- Event 22: 89
INSERT INTO ""Registrations"" (""EventId"") SELECT 22 FROM generate_series(1,89);
-- Event 23: 8
INSERT INTO ""Registrations"" (""EventId"") SELECT 23 FROM generate_series(1,8);
-- Event 24: 5
INSERT INTO ""Registrations"" (""EventId"") SELECT 24 FROM generate_series(1,5);
-- Event 25: 11
INSERT INTO ""Registrations"" (""EventId"") SELECT 25 FROM generate_series(1,11);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Prices_PriceId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "Suggestions");

            migrationBuilder.DropIndex(
                name: "IX_Events_PriceId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ContactAddress",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "IsVeteran",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "ProfilePhoto",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "BarrierFreeUrl",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "GoogleMeetUrl",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "InclusivityIds",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "LocationLink",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MeetingType",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Recurrence",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TransferAvailable",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TransferDetails",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
