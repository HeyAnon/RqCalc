using RqCalc.Tests._Base;

namespace RqCalc.Tests;

[TestClass]
public class MainTests : TestBase
{
    [TestMethod]
    public void CheckUniqueTalentPositions()
    {
        using (var connection = new SQLiteConnection(Properties.Settings.Default.ConnectionString))
        using (var dataSource = new RQDBContext(connection, false, ImplementTypeResolver.Default.WithCache()))
        {
            foreach (var branch in dataSource.TalentBranches)
            {
                foreach (var pair in branch.Talents.GroupBy(tal => tal.HIndex).OrderBy(g => g.Key).ZipStrong(Enumerable.Range(0, 7), (g, index) => new { G = g, Index = index}))
                {
                    if (pair.Index != pair.G.Key)
                    {
                        throw new Exception();
                    }

                    foreach (var pair2 in pair.G.OrderBy(tal => tal.VIndex).Zip(Enumerable.Range(0, 2), (tal, index) => new { T = tal, Index = index}))
                    {
                        if (pair2.T.VIndex != pair2.Index)
                        {
                            throw new Exception();
                        }
                    }
                }
            }
        }
    }

    [TestMethod]
    public void TestDeserializeCodes()
    {
        var codes = new[]
        {
            "EBrBR5zlJAiZ/8tFtE572at28t0NbmR7PLp3Mv32cb3cwzRNF7s7lfb80g1PuuHJdnqqkqw9HAYB5n1HEiEB",
            "FAeCX5zlpAQh85PJgdNoR3vPzs6zjY1qbxvFTl7dPp5pT7tjc6fElle054n2POdOg9U3u8Fpmo75wOdSFgBBBeYgBSWttwAA",
            "DgfBL85yUoKQ+ckncmC0yb23s/tsY6Pa22Cxk2W1j2fY3O/Y3FfY8Mo3PvnG59x5qxqze/t5HhQOfgAA",
            "DgfJL85yUoKQqcnkU6AHuYFgmyfWaWC61chmq8EEI9GA0CDAaTjDNhTUjYK6McJ3Vu1VSEgCgCkOvwAA",
            "FBSO38VZTkoQMj+5pyim9rKX7Ows292Ydrow7GRa7eOq2N6psO13bnjODU+z01P1fWz3sGnCKX+2XAJAoAAA",
            "EBXE7+IsJyUImT9MVgVOWx5JdvaN3exMO72IdvKp9rExtncp7Dodm79j89dsf1bdURtcEAQ9g1sAAA==",
            "FAeCX5zlpAQh85MDJ/JoR3vhzs6zjY1+bxvFTl7dPp5pT7tjc6fElle054n2POdmg1Uyu8Fpmo75wOdSFgDhAgA=",
            "FBqCjzjLSRAyP3gcYJ32spfu5Lwb3MjscePeyfTbx/VyDzdNs9jVM7DnV+14qh1Ps9Nglczaw2IQxLzvSALgPw==",
            "DhjBH86SIGT68eA0Hu1o7+3sbNvYmDa4Mezkte3jmfa0OzZ3Jra88n1Pvu+JNhus0uwGp2kacYFrAAA=",
            "DhjBH86SIGT6cUAlHu1o7+3kbNvYmDa48e7ktezjmfa0OzZ3Jra88o1PvvGJNhus0uwGp2kacYFrAAA=",
            "EBjHH86SIGT+8XhwGu1oL+3kbNvYmDa4Mexkuu3jmva0Ozb3TGz5HfueY98TbTZYpdkNTtM04gLXyo2GBA==",
            "FBSO38VZTkoQMv8YGGCm9rKX7Ows293od7ow7GQa7uOq2N6zsO13bnjODc+702D1fWz3sGnCKX+yXAJAuAAA",
            "FBSO38VZTkoQMv+4Gfil9rKX7Ows293od7oR72Qa7uOq2N6zsO137nfO/U6z02D1fWz3sGnCKX+yXAJAuAAA",
            "ERTE7+IsJyUImV88DqhSO9pIdnaW7W5MO10YNvgIt7VRbO9Z2HaK9nzRnq/cbLDaPrY7qCjglD9ZLgEA",
            "DhHBG4uznJQgZH7yiRzI7fXvbmzd2/gM7nFw3ckyuY9nsLVfsbtzodM17bSnnXaz1V3N4eMun6ap7HOCIKS6+AUA",
            "FBGCNxZnOSlByPzkEzmQ2+vetbF3b2MjuMfBdSfL5D6eit2dFjpd00572mm3W93VZPizyQVBUPY5QRCkungFAIKBBg==",
            "EBHBG4uznJQgZH7yiRzI7XXvbuzd29gI7nFw3ckyuY9nsLnj2MlpodM17bSnnXaz1V3N4eMun6ap7HOCIKS6eAUA",
            "FBGCNxZnOSlByPzkMSpQ3etesrN3b2Ojs8fFdSeL5D6uil2dght+x67/2PWnm/7VZP2zyQVBUPY5QRCkungFhGz/AQ==",
            "FBGCNxZnOSlByPzkMSpQ3etesrN3b2Ojs8fFdSeL5D6uweaOxa5OwQ2/Y9d/7PrTTf9qsm7e5c/zKPucIAhSXbwCQrb/",
            "CRHBm8VZTkoQMj85gBqXdrT3dnLuDW4E97gx7mQS3Mcz2NOu2N1Xd1yiPU+05yk3G6yZbd7lIE2XfQ42zdW51AA=",
            "DhXB7+IsJyUImX9MQIVPO9qLdnKO7W5MO90YdjKq9vFUbO8s7XqNW55xy9NsNljVPRv8axr/hzQAAA==",
            "DhXB7+IsJyUImZ8cUIVPO9qLdjKP7e5MOz0adjKq9jFVbu8s77pFe+5oz11u9lSlPRscpGn/hzQAAA==",
            "FBWC38VZTkoQMr8YHGiddrSX7OQc292YdroR72Ta7eOq2NWpsOt37nfO/c672WCV7NngYdPwf0hDuRCy/Qc=",
            "AgrA16QxPTlAPp1whFdhDb746czwINhhbfnhr3lpR2kaOAAUDKkJTzrhVku8452sXArStDz/0Q==",
            "BRHBm8VZThpzk0/kQK5SeNc2wK4jZ7COg2tOlslKOUVbAqUmYLppTDWNJisA/OBGQRBMefAL",
            "AxXF7+IsJyUImZ8MXgVOWx5Jdnau7W5MO12IdvKp9rFRbPMrm27H5ufY/DS7n1V31AYXBEHP4RY=",
            "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"
        };

        var facade = new JsonFacade(ServiceFacadeEnvironment.Configuration);

        foreach (var code in codes)
        {
            var character = facade.DecryptCharacter(code);
        }
    }

    [TestMethod]
    public void TestUnique63Equipment()
    {
        using (var connection = new SQLiteConnection(Properties.Settings.Default.ConnectionString))
        using (var dataSource = new RQDBContext(connection, false, ImplementTypeResolver.Default.WithCache()))
        {
            var idTypes = new[] {1, 3, 4, 5, 6};

            var request = from equip in dataSource.Equipments.ToList()

                where equip.Level == 63 && equip.Attack == null && idTypes.Contains(equip.Type.Id)

                from @class in equip.GetClassConditions()

                group @class by equip;

            var equip63D = request.ToList();

            Assert.AreEqual(equip63D.Count, idTypes.Length * 4);

            foreach (var @classGroup in equip63D)
            {
                if (@classGroup.Count() != 2)
                {
                    throw new Exception();
                }
            }
        }
    }

    [TestMethod]
    public void TestUnique65Equipment()
    {
        using (var connection = new SQLiteConnection(Properties.Settings.Default.ConnectionString))
        using (var dataSource = new RQDBContext(connection, false, ImplementTypeResolver.Default.WithCache()))
        {
            var idTypes = new[] { 1, 3, 4, 5, 6 };

            var request = from equip in dataSource.Equipments.ToList()

                where equip.Level == 65 && equip.Attack == null && idTypes.Contains(equip.Type.Id)

                from @class in equip.GetClassConditions()

                group @class by equip;

            var equip65D = request.ToList();

            Assert.AreEqual(equip65D.Count, idTypes.Length * 8);

            foreach (var @classGroup in equip65D)
            {
                if (@classGroup.Count() != 1)
                {
                    throw new Exception();
                }
            }
        }
    }


    [TestMethod]
    public void TestImageSize()
    {
        this.Process((dataSource, context) =>
        {
            var z = dataSource.GetFullList<IImage>().Sum(i => i.Data.Length);

            return;
        });
    }

        
    [TestMethod]
    public void TestSerialize()
    {
        var e = this.Process((dataSource, context) =>
        {
            var testData = this.GetTestData(dataSource);

            var serialized = context.Serializer.Serialize(testData);

            var deserialized = context.Serializer.Deserialize(serialized);

            return testData.Equals(deserialized);
        });

        return;
    }

    [TestMethod]
    public void Test2()
    {
        this.Process((dataSource, context) =>
        {
            var testData = this.GetTestData(dataSource);

            var res = context.Calc(testData);

            return;
        });
    }

    private CharacterSource GetTestData(IDataSource<IPersistentDomainObjectBase> dataSource)
    {
        if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));

        var editStats = dataSource.GetObjects<IStat>(s => s.IsEditable);
        var cards = dataSource.GetFullList<ICard>();
        var stamps = dataSource.GetFullList<IStamp>();
        var slots = dataSource.GetFullList<IEquipmentSlot>();
        var equipments = dataSource.GetFullList<IEquipment>();
        var consumables = dataSource.GetFullList<IConsumable>();

        var weaponCard = cards.GetByName("Карта Минотавра");
        var plateCard = cards.GetByName("Карта Костяного кошмара");
        var amuletCard = cards.GetByName("Карта Фараона");

        var bloodCard1 = cards.GetByName("Карта Крови: Голем");
        var bloodCard2 = cards.GetByName("Карта Крови: Панцирь");
        var bloodCard3 = cards.GetByName("Карта Крови: Крепость");
        var bloodCard4 = cards.GetByName("Карта Крови: Демон");

        //var poisonCard = cards.GetByName("Карта Яда");

        var blackAngelStamp = stamps.GetByName("Черного ангела");
        var faceStamp = stamps.GetByName("Виртуозного снайпера");
        var plateStamp = stamps.GetByName("Песчаного кокона");
        var humResistStamp = stamps.GetByName("Хозяина");
        var druidStamp = stamps.GetByName("Друида");
        var totemStamp = stamps.GetByName("Жужи");
        var guildBonuses = dataSource.GetFullList<ILegacy_GuildBonus>();

        var bestStampColor = dataSource.GetFullList<IStampColor>().OrderById().Last();

        var @class = dataSource.GetFullList<IClass>().GetByName("Волшебник");


        var talents = dataSource.GetFullList<ITalentBranch>().Where(branch => @class.IsSubsetOf(branch)).SelectMany(branch => branch.Talents).ToList();

        return new CharacterSource
        {
            Gender = dataSource.GetFullList<IGender>().GetByName("Мужской"),
            Class = @class,
            State = dataSource.GetFullList<IState>().GetByName("Пешком"),
            Level = 60,

            Buffs = { { dataSource.GetFullList<IBuff>().GetByName("Архивариус"), 8 } },

            Aura = dataSource.GetFullList<IAura>().GetByName("Аура жизни"),

            Elixir = dataSource.GetFullList<IElixir>().GetByName("Эликсир Мудрого Дракона"),

            Event = dataSource.GetFullList<IEvent>().GetByName("День св. Валлена"),

            Consumables =
            {
                consumables.GetByName("Травяная жвачка"),
                consumables.GetByName("Травяной чай"),
                consumables.GetByName("Газировка"),
                consumables.GetByName("Королевское благословение"),
            },

            //GuildTalents = 

            //GuildBonuses =
            //{
            //    { guildBonuses.GetByName("Атака"),                3 },
            //    { guildBonuses.GetByName("Оборона"),              3 },
            //    { guildBonuses.GetByName("Мобильность"),          2 },
            //    { guildBonuses.GetByName("Богатство"),            2 },
            //    { guildBonuses.GetByName("Охота за сокровищами"), 2 },
            //},

            Talents =
            {
                talents.GetByName("Стазис"),
                talents.GetByName("Пылающая преграда"),
                talents.GetByName("Кинетическое поле"),
                talents.GetByName("Архивариус"),
                talents.GetByName("Боевой маг"),
                talents.GetByName("Чарокрад"),
                talents.GetByName("Волна силы"),
                talents.GetByName("Опаляющее пламя"),
                talents.GetByName("Несокрушимая преграда"),
                talents.GetByName("Разрушитель чар"),
                talents.GetByName("Хранитель жизни"),
                talents.GetByName("Искажающее поле"),
                talents.GetByName("Совершенный щит"),
                talents.GetByName("Контроль времени"),
                talents.GetByName("Мираж"),
                talents.GetByName("Печать боли"),
                talents.GetByName("Хрономант"),
                talents.GetByName("Зеркало реальности"),
            },

            EnableAura = true,
            EnableBuffs = false,
            EnableConsumables = true,
            EnableGuildTalents = true,
            EnableElixir = true,
            EnableTalents = true,

            EditStats =
            {
                { editStats.GetByName("Интеллект"),    100 },
                { editStats.GetByName("Ловкость"),     39  },
                { editStats.GetByName("Выносливость"), 100 },
                { editStats.GetByName("Удача"),        1   }
            },

            Equipments =
            {
                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Головной убор")},                   new CharacterEquipmentData { Equipment = equipments.GetByName("Шляпа Мастера иллюзий"),     Upgrade = 9,  Cards = { bloodCard1 },                         StampVariant = blackAngelStamp.GetByColor(bestStampColor) } },
                                                                                                                                                                                                                                    
                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Аксессуар для лица")},              new CharacterEquipmentData { Equipment = equipments.GetByName("Маска гоблина-диверсанта"),  Upgrade = 10, Cards = { bloodCard2 },                         StampVariant = faceStamp.GetByColor(bestStampColor) } },
                                                                                                                                                                                                                                    
                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Доспех")},                          new CharacterEquipmentData { Equipment = equipments.GetByName("Мундир истребителя нежити"), Upgrade = 10, Cards = { plateCard  },                         StampVariant = plateStamp.GetByColor(bestStampColor) } },
                                                                                                                                                                                                                                    
                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Перчатки")},                        new CharacterEquipmentData { Equipment = equipments.GetByName("Перчатки короля стрелков"),  Upgrade = 10, Cards = { bloodCard3 },                         StampVariant = blackAngelStamp.GetByColor(bestStampColor) } },
                                                                                                                                                                                                                                    
                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Поножи")},                          new CharacterEquipmentData { Equipment = equipments.GetByName("Брюки истребителя нежити"),  Upgrade = 9,  Cards = { bloodCard3 },                         StampVariant = humResistStamp.GetByColor(bestStampColor) } },
                                                                                                                                                                                                                                    
                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Сапоги")},                          new CharacterEquipmentData { Equipment = equipments.GetByName("Сапоги короля стрелков"),    Upgrade = 9,  Cards = { bloodCard3 },                         StampVariant = humResistStamp.GetByColor(bestStampColor) } },
                                                                                                        
                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Оружие (правая рука)")},            new CharacterEquipmentData { Equipment = equipments.GetByName("Накопитель гнева"),          Upgrade = 10, Cards = { weaponCard, weaponCard, weaponCard }, StampVariant = blackAngelStamp.GetByColor(bestStampColor) } },

                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Щит / Орб / Оружие (левая рука)")}, new CharacterEquipmentData { Equipment = equipments.GetByName("Высокочастотный орб"),       Upgrade = 10, Cards = { bloodCard3 },                         StampVariant = blackAngelStamp.GetByColor(bestStampColor) } },
                    
                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Артефакт")},                        new CharacterEquipmentData { Equipment = equipments.GetByName("Древний идол"),              Upgrade = 10, Cards = { bloodCard4 },                         StampVariant = druidStamp.GetByColor(bestStampColor) } },

                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Тотем")},                           new CharacterEquipmentData { Equipment = equipments.GetByName("Тотем берсерка"),            Upgrade = 9,  Cards = { bloodCard1 }, Active = false,         StampVariant = totemStamp.GetByColor(bestStampColor) } },
                    
                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Кольцо"), Index = 0},               new CharacterEquipmentData { Equipment = equipments.GetByName("Пожирающее душу кольцо"),    Upgrade = 10, Cards = { bloodCard2 },                         StampVariant = druidStamp.GetByColor(bestStampColor) } },
                                                                                                                                                                                                                                          
                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Кольцо"), Index = 1},               new CharacterEquipmentData { Equipment = equipments.GetByName("Пожирающее душу кольцо"),    Upgrade = 10, Cards = { bloodCard2 },                         StampVariant = druidStamp.GetByColor(bestStampColor) } },

                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Амулет")},                          new CharacterEquipmentData { Equipment = equipments.GetByName("Хрустальный амулет"),        Upgrade = 10, Cards = { amuletCard },                         StampVariant = druidStamp.GetByColor(bestStampColor) } },
                    
                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Украшение")},                       new CharacterEquipmentData { Equipment = equipments.GetByName("Осколок Света") } },

                { new CharacterEquipmentIdentity { Slot = slots.GetByName("Верховой зверь")},                  new CharacterEquipmentData { Equipment = equipments.GetByName("Белый крок"),                                                      Active = false } },
            },
        };
    }

    [TestMethod]
    public void TestEvaluateBonuses()
    {
        this.Process((dataSource, context) =>
        {
            var bonusRequest = from cardBonus in dataSource.GetFullList<ICardBonus>()
                                   
                let result = TryResult.Catch(() => cardBonus.ToBonusBase().EvaluateTemplate())

                where result.IsFault()

                select new
                {
                    Bonus = cardBonus,

                    Result = result
                };

            var faults = bonusRequest.ToList();

            return;
        });
    }
}