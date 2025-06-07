using System.Linq.Expressions;

using Framework.Core;
using Framework.Core.Serialization;
using Framework.DataBase;
using Framework.ExpressionParsers;

using RqCalc.Application._Extensions;
using RqCalc.Application.Calc;
using RqCalc.Application.ImageSource;
using RqCalc.Application.Serializer.Character;
using RqCalc.Application.Serializer.GuildTalent;
using RqCalc.Application.Serializer.Talent;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.BonusType;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.Formula;
using RqCalc.Domain.GuildTalent;
using RqCalc.Model;
using RqCalc.Model.Impl;

namespace RqCalc.Application;

public partial class ApplicationContext : IApplicationContext
{
    private readonly INativeExpressionParser _nativeParser;

    internal readonly IReadOnlyList<IStat> BaseStats;

    internal readonly IReadOnlyList<IStat> EditStats;

    internal readonly IReadOnlyList<IReadOnlyList<IStat>> DependencyStatLayers;

    internal readonly IReadOnlyDictionary<IStat, int> StatPriority;

    internal readonly IReadOnlyList<IReadOnlyList<IBonusType>> DependencyBonusTypeLayers;

    internal readonly IReadOnlyDictionary<IBonusType, int> BonusTypePriority;


    internal readonly IReadOnlyList<IStat> SourcesStats;

    internal readonly IReadOnlyList<IBonusType> AttackBonusTypes;


    private readonly IEquipmentSlot _primaryWeaponSlot;

    internal readonly IReadOnlyList<IEquipmentForgeData> EquipmentForges;

    internal readonly IReadOnlyDictionary<Tuple<int, int>, int> EquipmentLevelForges;

    internal readonly IReadOnlyDictionary<IClass, IReadOnlyList<int>> ClassLevelHpBonuses;

    private readonly ICharacterSource _defaultCharacter;


    internal readonly IReadOnlyDictionary<IFormula, Func<ICalcState, decimal>> Formulas;


    public ApplicationContext(

        ApplicationSettings applicationSettings,
        IDataSource<IPersistentDomainObjectBase> dataSource,
        INativeExpressionParser nativeParser,
        ISerializer<string, byte[]> urlSerializer,
        ITypeResolver<string> typeResolver,
        IImageSourceService imageSourceService)
    {
        if (urlSerializer == null) throw new ArgumentNullException(nameof(urlSerializer));

        this.DataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

        this._nativeParser = nativeParser ?? throw new ArgumentNullException(nameof(nativeParser));


        //this.Settings = Anon.RQ_Calc.Domain.Settings.Create(this.DataSource.GetFullList<ISetting>()).ToApplicationSettings(this.DataSource);


        var classStats = from @class in this.DataSource.GetFullList<IClass>()

            from stat in @class.GetStats()

            select stat;


        var stats = this.DataSource.GetFullList<IStat>();

        {
            this.DependencyStatLayers = this.GetDependencyStatLayers(stats);
            this.StatPriority = this.DependencyStatLayers.SelectMany((layer, layerIndex) => layer.Select(stat => new { Stat = stat, Priority = layerIndex * BonusEvaluateRule.LayerStep }))
                .ToDictionary(pair => pair.Stat, pair => pair.Priority);

            this.DependencyBonusTypeLayers = this.GetDependencyBonusTypeLayers();
            this.BonusTypePriority = this.DependencyBonusTypeLayers.SelectMany((layer, layerIndex) => layer.Select(bonus => new { Bonus = bonus, Priority = layerIndex * BonusEvaluateRule.LayerStep + (bonus.IsMultiply.Value ? BonusEvaluateRule.MultiplyOffset : BonusEvaluateRule.SumOffset) }))
                .ToDictionary(pair => pair.Bonus, pair => pair.Priority);
        }

        {
            this.AttackBonusTypes = this.DataSource.GetFullList<IBonusType>().Where(bonusType => bonusType.Stats.Any() && bonusType.Stats.All(bts => bts.Stat == this.AttackStat)).ToList();
        }

        this.EditStats = stats.Where(s => s.IsEditable).ToReadOnlyCollection();

        this.BaseStats = stats.Except(classStats.Distinct()).ToReadOnlyCollection();

        this.NotPrimaryEditStats = this.BaseStats.Where(s => s.IsEditable).ToReadOnlyCollection();

        {
            this.Formulas = this.GetParsedFormulas();
        }

        {
            this.SourcesStats = stats.Where(s => s.Sources.Any()).ToArray();
        }

        this._primaryWeaponSlot = this.DataSource.GetFullList<IEquipmentSlot>().Single(s => s.IsPrimarySlot());

        this.EquipmentForges = this.DataSource.GetFullList<IEquipmentForge>().ToDictionary(v => v.Level).ToArrayI();
        this.EquipmentLevelForges = this.DataSource.GetFullList<IEquipmentLevelForge>().ToDictionary(v => Tuple.Create(v.EquipmentLevel, v.Level), v => v.Hp);

        this.ClassLevelHpBonuses = this.DataSource.GetFullList<IClass>().Where(@class => @class.Parent == null).ToDictionary(

            @class => @class,

            @class => new Dictionary<int, int> { { 0, 0 } }.Concat(@class.LevelHpBonuses.ToDictionary(bonus => bonus.Level, bonus => bonus.Value)).ToArrayI().ToReadOnlyListI());


        {
            var characterSerializer = new CharacterSerializer(this);

            this.CharacterBinarySerializer = characterSerializer;

            this.CharacterSerializer = this.CharacterBinarySerializer.Select(urlSerializer);

            this.AurasSharedBonuses = characterSerializer.LastVersionSerializer.AuraSharedBonuses;
        }

        {
            this.TalentSerializer = new TalentBuildSerializer(this).Select(urlSerializer);
        }

        {
            this.GuildTalentSerializer = new GuildTalentBuildSerializer(this).Select(urlSerializer);
        }


        this._defaultCharacter = this.CreateDefaultCharacter();

        this.TypeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
        this.ImageSourceService = imageSourceService ?? throw new ArgumentNullException(nameof(imageSourceService));
    }

        
    public ITypeResolver<string> TypeResolver { get; }

    public IImageSourceService ImageSourceService { get; }


    public IDataSource<IPersistentDomainObjectBase> DataSource { get; }


    public IReadOnlyList<IStat> NotPrimaryEditStats { get; }


    public ISerializer<byte[], ICharacterSource> CharacterBinarySerializer { get; }

    public ISerializer<string, ICharacterSource> CharacterSerializer { get; }

    public ISerializer<string, ITalentBuildSource> TalentSerializer { get; }

    public ISerializer<string, IGuildTalentBuildSource> GuildTalentSerializer { get; }



    public IReadOnlyDictionary<IAura, Tuple<IBonusContainer<IBonusBase>, IBonusContainer<IBonusBase>>> AurasSharedBonuses { get; }


    public IEnumerable<IStat> GetEditStats(IClass @class)
    {
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        return new[] { @class.PrimaryStat }.Concat(this.NotPrimaryEditStats);
    }

    public ICharacterResult Calc(ICharacterSource character)
    {
        if (character == null) throw new ArgumentNullException(nameof(character));

        this.Validate(character);

        var calc = new CharacterCalc(this, character);

        var stats = calc.GetStats().ChangeValue(d => d.Normalize());

        return new CharacterResult
        {
            Stats = stats,

            Code = this.CharacterSerializer.Serialize(character),

            //TalentCode = this.TalentSerializer.Serialize(character),

            //GuildTalentCode = this.GuildTalentSerializer.Serialize(character),

            Equipments = calc.GetEquipmentResults(),

            StatDescriptions = calc.GetDescriptionValues(stats)
        };
    }

    public ICharacterSource GetDefaultCharacter()
    {
        return this._defaultCharacter;
    }

    public IEquipmentClass GetEquipmentClass(IEquipment equipment)
    {
        if (equipment == null) throw new ArgumentNullException(nameof(equipment));

        var internalLevel = equipment.Info.Maybe(v => v.InternalLevel, 1);

        return this.DataSource.GetFullList<IEquipmentClass>().OrderByDescending(e => e.Level).First(c => internalLevel >= c.Level);
    }

    public int GetFreeGuildTalents(IGuildTalentBuildSource characterInput)
    {
        if (characterInput == null) throw new ArgumentNullException(nameof(characterInput));

        var pointLimit = this.DataSource.GetFullList<IGuildTalentBranch>().Sum(b => b.MaxPoints);

        var usedPoints = characterInput.GuildTalents.Sum(tal => tal.Value);

        return pointLimit - usedPoints;
    }


    private IReadOnlyList<IReadOnlyList<IBonusType>> GetDependencyBonusTypeLayers()
    {
        var groupRequest = from bonusType in this.DataSource.GetFullList<IBonusType>()

            group bonusType by this.DependencyStatLayers.FirstOrDefault(statLayer => bonusType.Stats.Any(bts => statLayer.Contains(bts.Stat)));

        var joinRequest = from statLayer in this.DependencyStatLayers

            join g in groupRequest on statLayer equals g.Key

            select g.ToArray();

        return joinRequest.ToArray();
    }

    private IReadOnlyList<IReadOnlyList<IStat>> GetDependencyStatLayers(IReadOnlyCollection<IStat> stats)
    {
        if (stats == null) throw new ArgumentNullException(nameof(stats));


        var request = from stat in stats

            let dependencyStats = stat.Bonuses.SelectMany(statBonus => statBonus.Type.Stats.ToList(bonusTypeStat => bonusTypeStat.Stat)).ToArray()

            where dependencyStats.AnyA()

            select new
            {
                Stat = stat,

                DependencyStats = dependencyStats
            };


        var depStats = request.ToList();


        var layers = new { Prev = Enumerable.Empty<IReadOnlyList<IStat>>(), Current = stats.ToList() }.Iterate(pair => pair.Current.Any(), state =>
        {
            var currentStats = state.Current;

            var currentDepStat = depStats.Where(pair => currentStats.Contains(pair.Stat)).ToList();

            return currentStats.Partial(stat => stat.Sources.All(source => source.Variables.All(v => v.TypeStat == null || state.Prev.Any(layer => layer.Contains(v.TypeStat))))

                                                && !currentDepStat.Any(statPair => statPair.DependencyStats.Contains(stat)), (prev, current) => new { Prev = state.Prev.Concat([prev
            ]), Current = current });
        });


        var result = layers.Prev.ToArray();

        return result;
    }

    private Dictionary<IFormula, Func<ICalcState, decimal>> GetParsedFormulas()
    {
        return this.DataSource.GetFullList<IFormula>().Where(formula => formula.Enabled).ToDictionary(formula => formula, formula =>
        {
            var expr = this._nativeParser.Parse(new NativeExpressionParsingData(new MethodTypeInfo(formula.Variables.Select(ParseVariableType), typeof(decimal)), formula.Value));

            var stateParam = Expression.Parameter(typeof(ICalcState));

            var lambda = Expression.Lambda<Func<ICalcState, decimal>>(

                Expression.Invoke(expr,

                    formula.Variables.Select(var =>
                    {
                        var varExpr = this.GetCompileSourceFormulaArgExpression(var);

                        return (varExpr as LambdaExpression).Maybe(varLambda => varLambda.GetBodyWithOverrideParameters(stateParam)) ?? varExpr;
                    })), stateParam);

            return lambda.Compile();
        });
    }

    private static Type ParseVariableType(IFormulaVariable variable)
    {
        if (variable == null) throw new ArgumentNullException(nameof(variable));

        switch (variable.Type)
        {
            case FormulaVariableType.Decimal:

            case FormulaVariableType.Stat:
            case FormulaVariableType.StatDescription:
            case FormulaVariableType.HpPerVitality:

            case FormulaVariableType.LevelDef:
            case FormulaVariableType.LevelAttack:
                return typeof(decimal);

            case FormulaVariableType.Int32:
            case FormulaVariableType.Level:
            case FormulaVariableType.MaxLevel:
            case FormulaVariableType.LevelMultiplicity:
                return typeof(int);

            case FormulaVariableType.CurrentWeaponInfo:
                return typeof(WeaponInfo);

            default:
                throw new ArgumentOutOfRangeException("variable.Type");

        }
    }

    private Expression GetCompileSourceFormulaArgExpression(IFormulaVariable variable)
    {
        if (variable == null) throw new ArgumentNullException(nameof(variable));

        switch (variable.Type)
        {
            case FormulaVariableType.Decimal:
                return ExpressionHelper.Create((ICalcState state) => state.CustomVariables[variable.Index]);

            case FormulaVariableType.Int32:
                return ExpressionHelper.Create((ICalcState state) => (int)state.CustomVariables[variable.Index]);

            case FormulaVariableType.Level:
                return ExpressionHelper.Create((ICalcState state) => state.Level);

            case FormulaVariableType.MaxLevel:
                return Expression.Constant(this.LastVersion.MaxLevel);

            case FormulaVariableType.Stat:
            {
                var stat = variable.TypeStat.FromMaybe(() => "Null stat");

                return ExpressionHelper.Create((ICalcState state) => state.Stats[stat]);
            }

            case FormulaVariableType.StatDescription:
            {
                var stat = variable.TypeStat.FromMaybe(() => "Null stat");

                var descFormula = stat.DescriptionFormula.FromMaybe(() => "Null Desc Formula");

                var descDel = LazyHelper.Create(() => this.Formulas[descFormula]);

                return ExpressionHelper.Create((ICalcState state) => descDel.Value(state.ChangeVariable(state.Stats[stat])));
            }

            case FormulaVariableType.CurrentWeaponInfo:
                return ExpressionHelper.Create((ICalcState state) => state.CurrentWeaponInfo);

            case FormulaVariableType.LevelDef:
                return ExpressionHelper.Create((ICalcState state) => MathHelper.ArmorByLevel(state.Level, this.Settings.QualityMaxLevel));

            case FormulaVariableType.LevelAttack:
                return ExpressionHelper.Create((ICalcState state) => MathHelper.AttackByLevel(state.Level));

            case FormulaVariableType.LevelMultiplicity:
                return Expression.Constant(this.Settings.LevelMultiplicity);

            case FormulaVariableType.HpPerVitality:
                return ExpressionHelper.Create((ICalcState state) => state.Class.HpPerVitality);

            default:
                throw new ArgumentOutOfRangeException("variable.Type");
        }
    }

    private ICharacterSource CreateDefaultCharacter()
    {
        var gender = this.DataSource.GetFullList<IGender>().OrderById().First();
        var @class = this.DataSource.GetFullList<IClass>().OrderById().First();
        var state = this.DataSource.GetFullList<IState>().OrderById().First();

        return new CharacterSource
        {
            Level = 1,
            Gender = gender,
            Class = @class,
            State = state,
            EditStats = this.GetEditStats(@class).ToDictionary(stat => stat, _ => 1)
        };
    }


    //public static ApplicationContext Create(IDataSource<IPersistentDomainObjectBase> dataSource)
    //{
    //    if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));

    //    return Create(dataSource, CSharpNativeExpressionParser.Compile.WithCache(), Framework.Core.Serialization.Serializer.Base64);
    //}

    //public static ApplicationContext Create(IDataSource<IPersistentDomainObjectBase> dataSource, INativeExpressionParser expressionParser, ISerializer<string, byte[]> serializer)
    //{
    //    if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));
    //    if (expressionParser == null) throw new ArgumentNullException(nameof(expressionParser));
    //    if (serializer == null) throw new ArgumentNullException(nameof(serializer));

    //    return new ApplicationContext(dataSource, expressionParser, serializer, DomainTypeResolver.Default, new ImageSourceService(dataSource, ImageSourceInvokeCache.Default));
    //}
}