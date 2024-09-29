using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.FlashcardMigrations
{
    /// <inheritdoc />
    public partial class SeededCorrectAndincorrectWordsWithJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CorrectWords",
                columns: new[] { "WordId", "Frequency", "Word" },
                values: new object[,]
                {
                    { 1, 100, "apple" },
                    { 2, 200, "banana" },
                    { 3, 150, "cherry" },
                    { 4, 120, "date" },
                    { 5, 110, "elderberry" },
                    { 6, 130, "fig" },
                    { 7, 140, "grape" },
                    { 8, 160, "honeydew" },
                    { 9, 170, "kiwi" },
                    { 10, 180, "lemon" },
                    { 11, 190, "mango" },
                    { 12, 210, "nectarine" },
                    { 13, 220, "orange" },
                    { 14, 230, "papaya" },
                    { 15, 240, "quince" },
                    { 16, 250, "raspberry" },
                    { 17, 260, "strawberry" },
                    { 18, 270, "tangerine" },
                    { 19, 280, "ugli" },
                    { 20, 290, "vanilla" },
                    { 21, 300, "watermelon" },
                    { 22, 310, "xigua" },
                    { 23, 320, "yellowfruit" },
                    { 24, 330, "zucchini" },
                    { 25, 340, "apricot" },
                    { 26, 350, "blackberry" },
                    { 27, 360, "cantaloupe" },
                    { 28, 370, "dragonfruit" },
                    { 29, 380, "elderflower" },
                    { 30, 390, "figs" },
                    { 31, 400, "grapefruit" },
                    { 32, 410, "huckleberry" },
                    { 33, 420, "indianfig" },
                    { 34, 430, "jackfruit" },
                    { 35, 440, "kumquat" },
                    { 36, 450, "lime" },
                    { 37, 460, "mulberry" },
                    { 38, 470, "nectar" },
                    { 39, 480, "olive" },
                    { 40, 490, "peach" },
                    { 41, 500, "plum" },
                    { 42, 510, "pomegranate" },
                    { 43, 520, "quandong" },
                    { 44, 530, "rambutan" },
                    { 45, 540, "soursop" },
                    { 46, 550, "tamarind" },
                    { 47, 560, "uglifruit" },
                    { 48, 570, "voavanga" },
                    { 49, 580, "wolfberry" },
                    { 50, 590, "ximenia" },
                    { 51, 600, "yumberry" },
                    { 52, 610, "ziziphus" },
                    { 53, 620, "almond" },
                    { 54, 630, "brazilnut" },
                    { 55, 640, "cashew" },
                    { 56, 650, "datefruit" },
                    { 57, 660, "elderberryfruit" },
                    { 58, 670, "figfruit" },
                    { 59, 680, "grapefruitfruit" },
                    { 60, 690, "hazelnut" },
                    { 61, 700, "indianalmond" },
                    { 62, 710, "jujube" },
                    { 63, 720, "kiwifruit" },
                    { 64, 730, "lemonfruit" },
                    { 65, 740, "macadamia" },
                    { 66, 750, "nectarfruit" },
                    { 67, 760, "orangefruit" },
                    { 68, 770, "peachfruit" },
                    { 69, 780, "quincefruit" },
                    { 70, 790, "raspberryfruit" },
                    { 71, 800, "strawberryfruit" },
                    { 72, 810, "tangerinefruit" },
                    { 73, 820, "uglifruitfruit" },
                    { 74, 830, "vanillafruit" },
                    { 75, 840, "watermelonfruit" },
                    { 76, 850, "xiguafruit" },
                    { 77, 860, "yellowfruitfruit" },
                    { 78, 870, "zucchinifruit" },
                    { 79, 880, "apricotfruit" },
                    { 80, 890, "blackberryfruit" },
                    { 81, 900, "cantaloupefruit" },
                    { 82, 910, "dragonfruitfruit" },
                    { 83, 920, "elderflowerfruit" },
                    { 84, 930, "figsfruit" },
                    { 85, 940, "grapefruitfruitfruit" },
                    { 86, 950, "huckleberryfruit" },
                    { 87, 960, "indianfigfruit" },
                    { 88, 970, "jackfruitfruit" },
                    { 89, 980, "kumquatfruit" },
                    { 90, 990, "limefruit" },
                    { 91, 1000, "mulberryfruit" },
                    { 92, 1010, "nectarfruitfruit" },
                    { 93, 1020, "olivefruit" },
                    { 94, 1030, "peachfruitfruit" },
                    { 95, 1040, "plumfruit" },
                    { 96, 1050, "pomegranatefruit" },
                    { 97, 1060, "quandongfruit" },
                    { 98, 1070, "rambutanfruit" },
                    { 99, 1080, "soursopfruit" },
                    { 100, 1090, "tamarindfruit" }
                });

            migrationBuilder.InsertData(
                table: "IncorrectWords",
                columns: new[] { "WordId", "Frequency", "Word" },
                values: new object[,]
                {
                    { 1, 50, "appl" },
                    { 2, 60, "banan" },
                    { 3, 55, "chery" },
                    { 4, 52, "dat" },
                    { 5, 51, "elderbery" },
                    { 6, 53, "fg" },
                    { 7, 54, "grap" },
                    { 8, 56, "honeyde" },
                    { 9, 57, "kiw" },
                    { 10, 58, "lemonn" },
                    { 11, 59, "mngo" },
                    { 12, 61, "nectarin" },
                    { 13, 62, "orane" },
                    { 14, 63, "papay" },
                    { 15, 64, "quinc" },
                    { 16, 65, "raspbery" },
                    { 17, 66, "strawbery" },
                    { 18, 67, "tangerin" },
                    { 19, 68, "uglii" },
                    { 20, 69, "vanila" },
                    { 21, 70, "watermeln" },
                    { 22, 71, "xigu" },
                    { 23, 72, "yellowfrut" },
                    { 24, 73, "zuchini" },
                    { 25, 74, "apricott" },
                    { 26, 75, "blackbery" },
                    { 27, 76, "cantalop" },
                    { 28, 77, "dragonfrut" },
                    { 29, 78, "elderflwer" },
                    { 30, 79, "figs" },
                    { 31, 80, "grapefrut" },
                    { 32, 81, "hucklberry" },
                    { 33, 82, "indianfg" },
                    { 34, 83, "jackfrut" },
                    { 35, 84, "kumquatt" },
                    { 36, 85, "limee" },
                    { 37, 86, "mulbery" },
                    { 38, 87, "nectr" },
                    { 39, 88, "oliv" },
                    { 40, 89, "pech" },
                    { 41, 90, "plumm" },
                    { 42, 91, "pomegrante" },
                    { 43, 92, "quandng" },
                    { 44, 93, "rambutn" },
                    { 45, 94, "soursop" },
                    { 46, 95, "tamrind" },
                    { 47, 96, "uglifrut" },
                    { 48, 97, "voavang" },
                    { 49, 98, "wolfbery" },
                    { 50, 99, "ximeni" },
                    { 51, 100, "yumbery" },
                    { 52, 101, "ziziphus" },
                    { 53, 102, "almond" },
                    { 54, 103, "brazilnut" },
                    { 55, 104, "cashew" },
                    { 56, 105, "datefruit" },
                    { 57, 106, "elderberryfruit" },
                    { 58, 107, "figfruit" },
                    { 59, 108, "grapefruitfruit" },
                    { 60, 109, "hazelnut" },
                    { 61, 110, "indianalmond" },
                    { 62, 111, "jujube" },
                    { 63, 112, "kiwifruit" },
                    { 64, 113, "lemonfruit" },
                    { 65, 114, "macadamia" },
                    { 66, 115, "nectarfruit" },
                    { 67, 116, "orangefruit" },
                    { 68, 117, "peachfruit" },
                    { 69, 118, "quincefruit" },
                    { 70, 119, "raspberryfruit" },
                    { 71, 120, "strawberryfruit" },
                    { 72, 121, "tangerinefruit" },
                    { 73, 122, "uglifruitfruit" },
                    { 74, 123, "vanillafruit" },
                    { 75, 124, "watermelonfruit" },
                    { 76, 125, "xiguafruit" },
                    { 77, 126, "yellowfruitfruit" },
                    { 78, 127, "zucchinifruit" },
                    { 79, 128, "apricotfruit" },
                    { 80, 129, "blackberryfruit" },
                    { 81, 130, "cantaloupefruit" },
                    { 82, 131, "dragonfruitfruit" },
                    { 83, 132, "elderflowerfruit" },
                    { 84, 133, "figsfruit" },
                    { 85, 134, "grapefruitfruitfruit" },
                    { 86, 135, "huckleberryfruit" },
                    { 87, 136, "indianfigfruit" },
                    { 88, 137, "jackfruitfruit" },
                    { 89, 138, "kumquatfruit" },
                    { 90, 139, "limefruit" },
                    { 91, 140, "mulberryfruit" },
                    { 92, 141, "nectarfruitfruit" },
                    { 93, 142, "olivefruit" },
                    { 94, 143, "peachfruitfruit" },
                    { 95, 144, "plumfruit" },
                    { 96, 145, "pomegranatefruit" },
                    { 97, 146, "quandongfruit" },
                    { 98, 147, "rambutanfruit" },
                    { 99, 148, "soursopfruit" },
                    { 100, 149, "tamarindfruit" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "CorrectWords",
                keyColumn: "WordId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "IncorrectWords",
                keyColumn: "WordId",
                keyValue: 100);
        }
    }
}
