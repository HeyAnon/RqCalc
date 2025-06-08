using System.Text.RegularExpressions;
using RqCalc.Tests._Base;

namespace RqCalc.Tests;

[TestClass]
public class UploadTests : TestBase
{
    [TestMethod]
    public void Test1()
    {
        var data = File.ReadAllLines(@"D:\Install.My\rq\Печати, внутрений уровень предметов + формула. - Предметы.csv");

        var items = data.Skip(1)
            .Select(line => line.Split(','))
            .Select(v => new { Name = v[0], Level = int.Parse(v[2]) })
            .OrderBy(pair => pair.Name)
            .ToList();

        var rrr = items.Select(v => v.Name).GetDuplicates().Pipe(System.Linq.Enumerable.ToHashSet);

        var dict = items.Where(item => !rrr.Contains(item.Name))
            .ToDictionary(item => item.Name.Trim(), item => item.Level);


        using (var connection = new SQLiteConnection(Properties.Settings.Default.ConnectionString))
        using (var context = new RQDBContext(connection, false, ImplementTypeResolver.Default.WithCache()))
        {
            var joinRes = from equipment in context.Equipments.ToList()

                join item in dict on equipment.Name.Replace('ё', 'е').ToLower() equals item.Key.ToLower()

                select new
                {
                    Eq = equipment,

                    Item = item
                };


            var res = joinRes.ToList();

            var notFoundItem = dict.Except(res.Select(pair => pair.Item)).Where(p => p.Value != 1).ToList();


            var notEqLevel = res.Where(item => item.Eq.InternalLevel != null && item.Eq.InternalLevel != item.Item.Value).ToList();

            return;
        }
    }

    [TestMethod]
    public void Test2()
    {
        var regex = new Regex("([А-Я|а-я|' '|-]+),((\"[a-z|A-Z|','|'_']+\")|[a-z|A-Z]+),([a-z|A-Z]+),([А-Я|а-я|' '|'('|')'|a-z|A-Z|'%']+),([0-9]+|(\"[0-9]+,[0-9]+\")),([0-9]+|(\"[0-9]+,[0-9]+\")),([0-9]+|(\"[0-9]+,[0-9]+\")),([0-9]+|(\"[0-9]+,[0-9]+\")),([a-z|A-Z]+)", RegexOptions.Compiled);


        var data = File.ReadAllLines(@"D:\Install.My\rq\Печати, внутрений уровень предметов + формула. - Печати.csv");



        var items = data.Skip(1)
            .Where(line => regex.IsMatch(line))
            .ToList(line => regex.Match(line));


        var items2 = items.ToList(g => new { StampName = g.Groups[1].Value, Color = EnumHelper.Parse<ColorE>(g.Groups[4].Value), StatName = g.Groups[5].Value, MinCoefficient = decimal.Parse(g.Groups[6].Value.Trim('\"')), MaxCoefficient = decimal.Parse(g.Groups[8].Value.Trim('\"')), Points = int.Parse(g.Groups[10].Value), Type = EnumHelper.Parse<AddType>(g.Groups[14].Value) });

        var bonusTypePoints = items2.Select(pair => Tuple.Create(pair.StatName, pair.MinCoefficient, pair.MaxCoefficient)).Distinct().ToDictionary(t => t.Item1, t => new { MinCoeff = t.Item2, MaxCoeff = t.Item3 });

        var gg1 = items2.GroupBy(item => item.StampName).ToList();


        var stampNames = gg1.Select(g => g.Key).ToList();


        var zzzzz = items2.Select(item => item.StatName).OrderBy(v => v).Distinct().ToList();
            

        using (var connection = new SQLiteConnection(@"Data Source=..\..\..\..\_db\rqdata.sqlite;foreign keys=true;Version=3;"))
        using (var context = new RQDBContext(connection, false, ImplementTypeResolver.Default))
        {

            var zz = items2.Select(item => item.StatName).Distinct().GetMergeResult(context.Stats, v => v, v => v.Name, StringComparer.CurrentCultureIgnoreCase);

            var rrrr = context.Stamps.SelectMany(s => s.Variants).SelectMany(s => s.Bonuses).Select(b => b.Type).Distinct().ToList();


            var merge1 = context.Stamps.GetMergeResult(gg1, v => v.Name, v => v.Key, StringComparer.CurrentCultureIgnoreCase);

            foreach (var bonusType in rrrr)
            {
                var points = bonusTypePoints[bonusType.GetMergeName()];

                bonusType.StampQualityMinCoef = points.MinCoeff;
                bonusType.StampQualityMaxCoef = points.MaxCoeff;

                context.BonusTypes.AddOrUpdate(bonusType);
            }

            foreach (var pair in merge1.CombineItems)
            {
                var colorGroup = pair.Item2.GroupBy(item => item.Color).ToList();

                var merge2 = colorGroup.GetMergeResult(pair.Item1.Variants, v => (int)v.Key, v => v.ColorId);

                foreach (var pairVar in merge2.CombineItems)
                {
                    var merge3 = pairVar.Item1.GetMergeResult(pairVar.Item2.Bonuses, v => v.StatName, v => v.Type.GetMergeName());

                    if (merge3.AddingItems.Any())
                    {

                    }

                    foreach (var bonusPair in merge3.CombineItems)
                    {
                        bonusPair.Item2.QualityValue = bonusPair.Item1.Points;

                        context.StampVariantBonuses.AddOrUpdate(bonusPair.Item2);
                    }

                    if (!merge3.IsEmpty)
                    {
                        //    if (!new[] { "Консервы", "Железной веры"}.Contains(pair.Item1.Name))
                        //    {

                        //    }
                    }

                    continue;
                }

                continue;
            }



            context.SaveChanges();

            return;
        }

        //var rrr = items.Select(v => v.Name).GetDuplicates().Pipe(System.Linq.Enumerable.ToHashSet);

        //var dict = items.Where(item => !rrr.Contains(item.Name))
        //                .ToDictionary(item => item.Name.Trim(), item => item.Level);
    }

    [TestMethod]
    public void UploadPets()
    {
        using (var connection = new SQLiteConnection("Data Source=..\\..\\..\\..\\_db\\rqdata.sqlite;foreign keys=true;Version=3"))
        using (var dataSource = new RQDBContext(connection, false, ImplementTypeResolver.Default.WithCache()))
        {
            foreach (var fileName in Directory.GetFiles(@"D:\Pets", "*.png"))
            {
                var petName = Path.GetFileName(fileName).SkipLast("_(питомец).png", true).Replace('_', ' ');

                var petImage = new Image { Data = File.ReadAllBytes(fileName) };

                var pet = new Pet { Image = petImage, Name = petName };

                dataSource.Images.AddOrUpdate(petImage);

                dataSource.Pets.AddOrUpdate(pet);
            }

            dataSource.SaveChanges();

            return;
        }
    }

}


public enum ColorE
{
    Green = 1,
    Blue,
    Violet,
    Orange
}

public enum AddType
{
    Point,
    Combo,
    Percent
}

internal static class StatExtensions
{
    public static string GetMergeName(this BonusType bonusType)
    {
        var sDict = new Dictionary<string, string>
        {
            { "{0} к устойчивости атакам Земли", "Устойчивость к урону стихий (Земля)" },
            { "{0} к Выносливости", "Выносливость" },
            { "{0} к устойчивости атакам Огня", "Устойчивость к урону стихий (Огонь)" },
            { "{0} к устойчивости атакам Воздуха", "Устойчивость к урону стихий (Воздух)" },
            { "{0} к устойчивости атакам Воды", "Устойчивость к урону стихий (Вода)" },
            { "{0} к устойчивости атакам Яда", "Устойчивость к урону стихий (Яд)" },
            { "{0} к устойчивости атакам Хаоса", "Устойчивость к урону стихий (Хаос)" },
            { "{0}% к урону Огнём", "Увеличение элементального урона % (Огонь)" },
            { "{0}% к Максимальному здоровью", "Здоровье %" },
            { "{0} к Максимальному здоровью", "Здоровье" },
            { "{0} к устойчивости атакам Насекомых", "Сопротивление урону от расы % (Насекомое)" },
            { "{0} к Защите", "Защита" },
            { "{0} к Увороту", "Уворот" },
            { "{0} к устойчивости атакам Животных", "Сопротивление урону от расы % (Животное)" },
            { "{0} Устойчивости к PvP урону.", "Устойчивость к PvP урону" },
            { "{0} к устойчивости атакам Нежити", "Сопротивление урону от расы % (Нежить)" },
            { "{0}% к Получаемому золоту", "Увеличение прихода денег (COMBO)" },
            { "{0}% к Защите", "Защита %" },
            { "{0} к Удаче", "Удача" },
            { "{0} к восстановлению Здоровья", "Восстановление здоровья в секунду" },
            { "{0}% к Силе критического удара", "Сила крита (COMBO)" },
            { "{0} к Попаданию", "Попадание" },
            { "{0}% к Скорости бега", "Скорость бега %" },
            { "{0}% Шанс получить +10% к защите при получении урона", "Шанс наложения при получении урона" },
            { "{0} к Опыту за убийство монстров", "Опыт за убийство" },
            { "{0}% опыта при убийстве существ", "Опыт за убийство %" },
            { "{0} к Атаке", "Атака" },
            { "{0}% к Шансу критического удара", "Вероятность крита" },
            { "{0} к Ловкости", "Ловкость" },
            { "{0}% к урону по Чудовищам", "Увеличение урона по расе % (Чудовище)" },
            { "{0}% к урону Землёй", "Увеличение элементального урона % (Земля)" },
            { "{0}% к урону Водой", "Увеличение элементального урона % (Вода)" },
            { "{0}% к урону Воздухом", "Увеличение элементального урона % (Воздух)" },
            { "{0}% Шанс вызвать ожог у противника при ударе", "Шанс наложения при атаке" },
            { "{0}% к Скорости атаки", "Скорость атаки (COMBO)" },
            { "{0}% Шанс получить +10% к увороту при получении урона", "Шанс наложения при получении урона" },
            { "{0}% Шанс ослабить атаку противника при ударе", "Шанс наложения при атаке" },
            { "Особенность: При нанесение урона умениями или обычными атаками можно вызывать у противника \"Яд\" с шансом {0}%", "Шанс наложения при атаке" },
            { "{0}% Шанс при нанесение урона умениями или обычными атаками вызывать у противника \"Кровотечение\"", "Шанс наложения при атаке" },
            { "{0} к Интеллекту", "Интеллект" },
            { "{0} к Силе", "Сила" },
            { "{0} к Точности", "Точность" },
            { "{0} Маны при ударе", "Восстановление маны при ударе" },
            { "{0} к Мане", "Мана" },
            { "{0} Здоровья при ударе", "Восстановление здоровья при ударе" },
            { "{0} к Дальности дистанционной атаки", "Дальность стрельбы" },
            { "{0}% к Мане", "Мана %" },
            { "{0} Здоровья при убийстве", "Восстановление здоровья при убийстве" },
            { "{0}% к Атаке", "Атака %" },
            { "{0} Маны при убийстве", "Восстановление маны при убийстве" },
            { "{0}% к восстановлению Маны", "Восстановление маны %" },
            { "{0} Энергии при ударе", "Восстановление энергии при ударе" },
            { "{0} к Энергии", "Энергия" },
            { "{0}% к Энергии", "Энергия %" },
            { "{0} к восстановлению Энергии", "Восстановление энергии" },
            { "{0} Энергии при убийстве", "Восстановление энергии при убийстве" },
            { "{0} к устойчивости атакам Гуманоидов", "Сопротивление урону от расы % (Гуманоид)" }
        };

        return sDict[bonusType.Template];
    }

    //public static string GetStatName(this string sName, AddType addType)
    //{
    //    var sDict = new Dictionary<string, string>
    //    {
    //        { "Сила крита", "Сила крит. удара"},
    //        { "Скорость бега", "Скорость передвижения"},
    //        { "Увеличение прихода денег", "Получаемое золото"},
    //        { "Вероятность крита", "Шанс крит. удара"},
    //        { "Дальность стрельбы", "Дистанция атаки"},
    //        { "Восстановление здоровья при убийстве", "Восстановление ХП при убийстве"},
    //        { "Увеличение элементального урона % (Огонь)", "Урон огнём"},
    //        { "Увеличение элементального урона % (Вода)", "Урон водой"},
    //        { "Увеличение элементального урона % (Воздух)", "Урон воздухом"},
    //        { "Увеличение элементального урона % (Земля)", "Урон землёй"},
    //        { "Увеличение урона по расе (Чудовище)", "Урон по Чудовищам"},
    //        { "Сопротивление урону от расы (Насекомое)", "Устойчивость к атакам Насекомых"},
    //        { "Устойчивость к урону стихий (Огонь)", "Устойчивость к атакам огня"},
    //        { "Устойчивость к урону стихий (Вода)", "Устойчивость к атакам воды"},
    //        { "Устойчивость к урону стихий (Земля)", "Устойчивость к атакам земли"},
    //        { "Устойчивость к урону стихий (Воздух)", "Устойчивость к атакам воздуха"},
    //        { "Устойчивость к урону стихий (Яд)", "Устойчивость к атакам яда"},
    //        { "Устойчивость к урону стихий (Хаос)", "Устойчивость к атакам хаоса"},
    //        { "Восстановление здоровья при ударе", "Восстановление ХП при ударе"},
    //        { "Сопротивление урону от расы (Животное)", "Устойчивость к атакам Животных"},
    //        { "Сопротивление урону от расы (Гуманоид)", "Устойчивость к атакам Гуманоидов"},
    //        { "Устойчивость к PvP урону", "Устойчивость к урону (PvP)"},
    //        { "Сопротивление урону от расы (Нежить)", "Устойчивость к атакам Нежити"},
    //        { "Восстановление здоровья в секунду", "Восстановление ХП в бою" },


    //        { "Шанс наложения при получении урона", "{0}% Шанс получить +10% к защите при получении урона"},
    //        { "Опыт за убийство", "{0} к Опыту за убийство монстров" },

    //        //{ "Опыт за убийство %", ""}
    //    };

    //    if (addType == AddType.PERCENT)
    //    {
    //        sDict = sDict.ChangeKey(key => key + " %");
    //    }

    //    return sDict.GetValueOrDefault(sName, sName);
    //}

    //public static string NormStatName(this string sName)
    //{
    //    return sName.Replace(" %", "").Replace(" (COMBO)", "");
    //}
}