//using Framework.Core;
//using Framework.Core.Serialization;
//using Framework.DataBase;
//using RqCalc.Application.Calculation;
//using RqCalc.Application.ImageSource;
//using RqCalc.Application.Serializer.Character;
//using RqCalc.Application.Serializer.GuildTalent;
//using RqCalc.Application.Serializer.Talent;
//using RqCalc.Application.Settings;
//using RqCalc.Domain;
//using RqCalc.Domain._Base;
//using RqCalc.Domain._Extensions;
//using RqCalc.Domain.BonusType;
//using RqCalc.Domain.Equipment;
//using RqCalc.Domain.Formula;
//using RqCalc.Model;

//namespace RqCalc.Application;

//public partial class ApplicationContext 
//{
//    //private readonly INativeExpressionParser _nativeParser;

//    //internal readonly IReadOnlyList<IStat> BaseStats;

//    //internal readonly IReadOnlyList<IStat> EditStats;

//    //internal readonly IReadOnlyList<IReadOnlyList<IStat>> DependencyStatLayers;

//    //internal readonly IReadOnlyDictionary<IStat, int> StatPriority;

//    //internal readonly IReadOnlyList<IReadOnlyList<IBonusType>> DependencyBonusTypeLayers;

//    //internal readonly IReadOnlyDictionary<IBonusType, int> BonusTypePriority;


//    //internal readonly IReadOnlyList<IBonusType> AttackBonusTypes;


//    //private readonly IEquipmentSlot _primaryWeaponSlot;

//    //internal readonly IReadOnlyList<IEquipmentForgeData> EquipmentForges;

//    //internal readonly IReadOnlyDictionary<Tuple<int, int>, int> EquipmentLevelForges;

//    //internal readonly IReadOnlyDictionary<IClass, IReadOnlyList<int>> ClassLevelHpBonuses;

//    //private readonly ICharacterSource _defaultCharacter;


//    //internal readonly IReadOnlyDictionary<IFormula, Func<ICharacterCalculationState, decimal>> Formulas;


//    public ApplicationContext(

//        ApplicationSettings applicationSettings,
//        IDataSource<IPersistentDomainObjectBase> dataSource,
//        ISerializer<string, byte[]> urlSerializer,
//        ITypeResolver<string> typeResolver,
//        IImageSourceService imageSourceService)
//    {
//        if (urlSerializer == null) throw new ArgumentNullException(nameof(urlSerializer));

//        this.DataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));


//        //this.Settings = Anon.RQ_Calc.Domain.Settings.Create(this.DataSource.GetFullList<ISetting>()).ToApplicationSettings(this.DataSource);



//        var stats = this.DataSource.GetFullList<IStat>();

//        {
//            this.DependencyBonusTypeLayers = this.GetDependencyBonusTypeLayers();
//            this.BonusTypePriority = this.DependencyBonusTypeLayers.SelectMany((layer, layerIndex) => layer.Select(bonus => new { Bonus = bonus, Priority = layerIndex * BonusEvaluateRule.LayerStep + (bonus.IsMultiply.Value ? BonusEvaluateRule.MultiplyOffset : BonusEvaluateRule.SumOffset) }))
//                .ToDictionary(pair => pair.Bonus, pair => pair.Priority);
//        }

//        {
//            this.AttackBonusTypes = this.DataSource.GetFullList<IBonusType>().Where(bonusType => bonusType.Stats.Any() && bonusType.Stats.All(bts => bts.Stat == this.AttackStat)).ToList();
//        }

//        this._primaryWeaponSlot = this.DataSource.GetFullList<IEquipmentSlot>().Single(s => s.IsWeapon == true && s.IsPrimarySlot());

//        this.EquipmentForges = this.DataSource.GetFullList<IEquipmentForge>().ToDictionary(v => v.Level).ToArrayI();
//        this.EquipmentLevelForges = this.DataSource.GetFullList<IEquipmentLevelForge>().ToDictionary(v => Tuple.Create(v.EquipmentLevel, v.Level), v => v.Hp);




//        {
//            var characterSerializer = new CharacterSerializer(this);

//            this.CharacterBinarySerializer = characterSerializer;

//            this.CharacterSerializer = this.CharacterBinarySerializer.Select(urlSerializer);

//            this.AurasSharedBonuses = characterSerializer.LastVersionSerializer.AuraSharedBonuses;
//        }

//        {
//            this.TalentSerializer = new TalentBuildSerializer(this).Select(urlSerializer);
//        }

//        {
//            this.GuildTalentSerializer = new GuildTalentBuildSerializer(this).Select(urlSerializer);
//        }

//        this.TypeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
//        this.ImageSourceService = imageSourceService ?? throw new ArgumentNullException(nameof(imageSourceService));
//    }

        
//    public ITypeResolver<string> TypeResolver { get; }

//    public IImageSourceService ImageSourceService { get; }



//    public IReadOnlyDictionary<IAura, Tuple<IBonusContainer<IBonusBase>, IBonusContainer<IBonusBase>>> AurasSharedBonuses { get; }


//    private IReadOnlyList<IReadOnlyList<IBonusType>> GetDependencyBonusTypeLayers()
//    {
//        var groupRequest = from bonusType in this.DataSource.GetFullList<IBonusType>()

//            group bonusType by this.DependencyStatLayers.FirstOrDefault(statLayer => bonusType.Stats.Any(bts => statLayer.Contains(bts.Stat)));

//        var joinRequest = from statLayer in this.DependencyStatLayers

//            join g in groupRequest on statLayer equals g.Key

//            select g.ToArray();

//        return joinRequest.ToArray();
//    }
//}