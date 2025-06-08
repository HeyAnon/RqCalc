using Framework.Core;
using Framework.HierarchicalExpand;

using RqCalc.Application._Extensions;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.BonusType;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.Formula;
using RqCalc.Domain.VirtualBonus;
using RqCalc.Model;
using RqCalc.Model._Extensions;

namespace RqCalc.Application.Calc;


internal partial class CharacterCalc : ICharacterCalc
{
    private readonly ApplicationContext context;
        
    private readonly HashSet<IStat> currentClassStats;
        

    private readonly ICharacterSource character;


    public CharacterCalc(ApplicationContext context, ICharacterSource character)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.character = character ?? throw new ArgumentNullException(nameof(character));

        this.currentClassStats = this.character.Class.GetStats().Concat(this.context.BaseStats).Pipe(System.Linq.Enumerable.ToHashSet);

        {
            var primaryWeaponRequest = from pair in this.character.Equipments

                orderby pair.Key.Slot.IsExtraSlot()
                                           
                where pair.Value.Equipment.IsAllowed(this.character.State)
                                           
                let weaponInfo = pair.Value.Equipment.Info as WeaponInfo
                                           
                where weaponInfo != null
                                           
                select weaponInfo;


            this.CurrentWeaponInfo = primaryWeaponRequest.Average();
        }
    }


    public WeaponInfo? CurrentWeaponInfo { get; }

    public Dictionary<IStat, decimal> GetStats()
    {
        return this.GetBonusEvaluators().GroupBy(evaluator => evaluator.Priority)
            .OrderBy(g => g.Key)
            .Aggregate(new Dictionary<IStat, decimal>(), (startStatsState, startEvalGroup) =>

                startEvalGroup.SelectMany(evaluator =>

                        evaluator switch
                        {
                            BonusEvaluator.ConstBonusEvaluator constEvaluator => [constEvaluator],
                            BonusEvaluator.DynamicBonusEvaluator dynamicEvaluator => dynamicEvaluator.Func(startStatsState),
                            _ => throw new ArgumentOutOfRangeException(nameof(evaluator))
                        })
                    .GroupBy(evaluator => evaluator.IsMultiply)
                    .OrderBy(g => g.Key)
                    .Aggregate(startStatsState, (statsState, evalGroup) =>
                    {
                        if (evalGroup.Key)
                        {
                            return new[] { statsState }.Concat(evalGroup.Select(evaluator => evaluator.Stats.ChangeValue(v => v.ToPercentValue()))).MulValues().TryRound();
                        }
                        else
                        {
                            return new[] { statsState }.Concat(evalGroup.Select(evaluator => evaluator.Stats)).SumValues();
                        }
                    })
            );
    }

    private IEnumerable<BonusEvaluator> GetBonusEvaluators()
    {
        {
            if (this.CurrentWeaponInfo == null)
            {
                yield return new BonusEvaluator.ConstBonusEvaluator(BonusEvaluateRule.SumOffset, false, this.context.AttackStat, 2);
            }

            {
                var classLevelHpBonus = this.context.ClassLevelHpBonuses[this.character.Class.GetRoot()][this.character.Level];

                yield return new BonusEvaluator.ConstBonusEvaluator(BonusEvaluateRule.SumOffset, false, this.context.HpStat, classLevelHpBonus);
            }

            yield return new BonusEvaluator.ConstBonusEvaluator(BonusEvaluateRule.SumOffset, false, this.character.EditStats.ToDictionary().ChangeValue(v => (decimal)v));

            yield return new BonusEvaluator.ConstBonusEvaluator(BonusEvaluateRule.SumOffset, false, this.currentClassStats.ToDictionary(stat => stat, stat => stat.DefaultValue));

            yield return new BonusEvaluator.ConstBonusEvaluator(BonusEvaluateRule.SumOffset, false, this.GetEquipmentStats());
        }

        foreach (var bonus in this.GetBonuses(BonusSource.All).GroupBy(bonus => bonus.Type).SelectMany(g => g.Key.IsSingle ? (IEnumerable<IBonusBase>) [g.FirstOrDefault()] : g))
        {
            if (bonus.Type.IsMultiply != null)
            {
                yield return this.GetBonusEvaluator(bonus);
            }
        }

        foreach (var bonusEvaluator in this.context.DependencyStatLayers.Select(this.GetDependencyEvaluator))
        {
            yield return bonusEvaluator;
        }

        foreach (var sourceStat in this.context.SourcesStats)
        {
            foreach (var sourceEvaluator in this.GetSourceEvaluators(sourceStat))
            {
                yield return sourceEvaluator;
            }
        }
    }

    private IEnumerable<BonusEvaluator> GetSourceEvaluators(IStat sourceStat)
    {
        if (sourceStat == null) throw new ArgumentNullException(nameof(sourceStat));

        return from sourceFormula in sourceStat.Sources

            where sourceFormula.Enabled

            select this.GetSourceEvaluator(sourceStat, sourceFormula);
    }

    private BonusEvaluator GetSourceEvaluator(IStat stat, IFormula sourceFormula)
    {
        if (stat == null) throw new ArgumentNullException(nameof(stat));
        if (sourceFormula == null) throw new ArgumentNullException(nameof(sourceFormula));

        var formulaFunc = this.context.Formulas[sourceFormula];

        return new BonusEvaluator.DynamicBonusEvaluator(this.context.StatPriority[stat] + BonusEvaluateRule.SumOffset, stats => new BonusEvaluator.ConstBonusEvaluator(0, false, stat, formulaFunc(new CalcState (this) { Stats = stats })));
    }

    private BonusEvaluator GetDependencyEvaluator(IReadOnlyList<IStat> layer, int index)
    {
        return new BonusEvaluator.DynamicBonusEvaluator(index * BonusEvaluateRule.LayerStep + BonusEvaluateRule.DependencyOffset, stateStats =>

            stateStats.Where(pair => layer.Contains(pair.Key))
                .SelectMany(pair => this.GetStatBonuses(pair.Key, (int)pair.Value))
                .Select(bonus => this.GetConstBonusEvaluator(bonus)));
    }

       
    private BonusEvaluator GetBonusEvaluator(IBonusBase bonus)
    {
        if (bonus == null) throw new ArgumentNullException(nameof(bonus));
            
        var multiplicityVars = bonus.Type.Variables.Where(variable => variable.MultiplicityStat != null).ToList();
            
        if (multiplicityVars.Any())
        {
            var priority = multiplicityVars.Select(var => this.context.StatPriority[var.MultiplicityStat]).Distinct().Single();

            return new BonusEvaluator.DynamicBonusEvaluator(priority + BonusEvaluateRule.DynamicOffset, stats =>
            {
                var dynamicVariables = from variable in multiplicityVars

                    let stat = variable.MultiplicityStat

                    orderby variable.Index

                    let multiplicityCount = (int)stats.GetValueOrDefault(stat) / (int)bonus.Variables[1]

                    let actualMultiplicityCount = variable.MultiplicityValue == null
                        ? multiplicityCount
                        : 1 + Math.Min(1, multiplicityCount)

                    select new VirtualBonusVariable(variable.Index, (int)bonus.Variables[0] * actualMultiplicityCount);


                var var1 = bonus.Variables.Select((value, index) => (index, value)).ToDictionary();

                var var2 = dynamicVariables.ToDictionary(pair => pair.Index, pair => (decimal) pair.Value);

                var internalBonus = new VirtualBonusBase
                {
                    Type = bonus.Type,

                    Variables = var1.Concat(var2).ToArrayI().ToList()
                };

                return this.GetConstBonusEvaluator(internalBonus);
            });
        }
        else
        {
            return this.GetConstBonusEvaluator(bonus);
        }
    }

    private BonusEvaluator.ConstBonusEvaluator GetConstBonusEvaluator(IBonusBase bonus, int priorityOffset = 0)
    {
        if (bonus == null) throw new ArgumentNullException(nameof(bonus));

        if (bonus.Type.Variables.Any(var => var.IsDynamic()))
        {
            var internalBonus = this.EvaluateDynamicBonus(bonus);

            return this.GetInternalConstBonusEvaluator(internalBonus, priorityOffset);
        }
        else
        {
            return this.GetInternalConstBonusEvaluator(bonus, priorityOffset);
        }
    }

    private IBonusBase EvaluateDynamicBonus(IBonusBase bonus)
    {
        if (bonus == null) throw new ArgumentNullException(nameof(bonus));

        var variablesRequest = from varPair in bonus.Variables.Select((value, i) => new { Value = value, Index = i })

            let mulFunc = bonus.Type.Variables.SingleOrDefault(btv => btv.Index == varPair.Index && btv.IsDynamic())
                .Maybe(btv => btv.MulFormula)
                .Maybe(mulFormula => this.context.Formulas[mulFormula])
                                                                            
            select mulFunc == null ? varPair.Value : mulFunc(new CalcState(this) { CustomVariables = new Dictionary<int, decimal> { { 0, varPair.Value } } });

        return new VirtualBonusBase
        {
            Type = bonus.Type,

            Variables = variablesRequest.ToList()
        };
    }

    private BonusEvaluator.ConstBonusEvaluator GetInternalConstBonusEvaluator(IBonusBase bonus, int priorityOffset)
    {
        if (bonus == null) throw new ArgumentNullException(nameof(bonus));

        var request = from typeStat in bonus.Type.Stats

            where this.currentClassStats.Contains(typeStat.Stat) 
                  //&& bonus.Type.Variables.All(v => v.MultiplicityStat == null || this._currentClassStats.Contains(v.MultiplicityStat)) 
                  && this.IsEvaluated(typeStat)

            let variableValue = bonus.Variables[typeStat.VarIndex]

            where variableValue != 0

            select new
            {
                typeStat.Stat,

                Value = variableValue
            };
            
        var stats = request.ToDictionary(pair => pair.Stat, pair => pair.Value);

        return new BonusEvaluator.ConstBonusEvaluator(this.context.BonusTypePriority[bonus.Type] + priorityOffset, bonus.Type.IsMultiply.Value, stats);
    }


    private bool HasCalcStats(IEquipment equipment)
    {
        if (equipment == null) throw new ArgumentNullException(nameof(equipment));

        return equipment.Info != null && equipment.IsAllowed(this.character.State);
    }

    private Dictionary<IStat, decimal> GetEquipmentStats()
    {
        var baseRequest = from equipmentPair in this.character.Equipments

            let equipment = equipmentPair.Value.Equipment

            where this.HasCalcStats(equipment)
                              
            let attackModifier = this.IsDualWeapon(equipmentPair) ? 0.5M : 1

            let equipmentInfo = equipmentPair.Value
                              
            from statPair in equipmentInfo.Equipment.Info switch
            {
                EquipmentInfo info => [new { Stat = this.context.DefenseStat, Value = (decimal)info.Defense }],

                WeaponInfo info => [new { Stat = this.context.AttackStat, Value = info.Attack * attackModifier }],

                DefenseWeaponInfo info => new[] {
                    new { Stat = this.context.AttackStat,  Value = info.Attack * attackModifier  },
                    new { Stat = this.context.DefenseStat, Value = (decimal)info.Defense }
                },

                _ => throw new ArgumentOutOfRangeException(nameof(equipmentInfo.Equipment.Info))
            }
                              
            where statPair != null && statPair.Value != 0
                              
            select statPair;

        var upgradeRequest = from equipmentPair in this.character.Equipments

            where this.HasCalcStats(equipmentPair.Value.Equipment)
                                 
            let equipmentInfo = equipmentPair.Value
                                 
            join upgradePair in this.GetEquipmentUpgradeInfo() on equipmentPair.Key equals upgradePair.Key
                                 
            let attackModifier = this.IsDualWeapon(equipmentPair) ? 0.5M : 1

            let upgradeInfo = upgradePair.Value
                                 
            from statPair in upgradeInfo switch
            {
                EquipmentUpgradeInfo info =>
                [
                    new { Stat = this.context.DefenseStat, Value = (decimal)info.Defense },
                    new { Stat = this.context.HpStat,      Value = (decimal)info.Hp      }
                ],

                DefenseWeaponUpgradeInfo info => new[]
                {
                    new { Stat = this.context.AttackStat, Value = info.Attack * attackModifier   },
                    new { Stat = this.context.DefenseStat, Value = (decimal)info.Defense },
                },

                WeaponUpgradeInfo info =>
                [
                    new { Stat = this.context.AttackStat, Value = info.Attack * attackModifier   }
                ],

                _ => throw new ArgumentOutOfRangeException(nameof(upgradeInfo))
            }
                                 
            where statPair.Value != 0
                                 
            select statPair;


        var allStatRequest = from equipmentPair in this.character.Equipments

            where this.HasCalcStats(equipmentPair.Value.Equipment)
                                 
            let equipmentInfo = equipmentPair.Value
                                 
            join upgradePair in this.GetEquipmentUpgradeInfo() on equipmentPair.Key equals upgradePair.Key
                                 
            let upgradeInfo = upgradePair.Value
                                 
            where upgradeInfo.AllStatBonus != 0
                                 
            from statPair in this.context.GetEditStats(this.character.Class).Select(stat => new { Stat = stat, Value = (decimal)upgradeInfo.AllStatBonus })
                                 
            select statPair;



        var request = from statPair in baseRequest.Concat(upgradeRequest).Concat(allStatRequest)

            group statPair.Value by statPair.Stat;



        return request.ToDictionary(pair => pair.Key, values => values.Sum());
    }


    public Dictionary<CharacterEquipmentIdentity, EquipmentUpgradeBaseInfo> GetEquipmentUpgradeInfo()
    {
        var request =

            from pair in this.character.Equipments

            let equipmentInfo = pair.Value

            let equipment = equipmentInfo.Equipment

            where equipment.Info != null && equipmentInfo.Upgrade != 0

            let forgeInfo = this.context.EquipmentForges[equipmentInfo.Upgrade]

            let getDefenseFunc = FuncHelper.Create((decimal defense) =>
                (int)Math.Round(Math.Ceiling(forgeInfo.Defense * defense)))

            let getAttackFunc = FuncHelper.Create((decimal attack) =>
                (int)Math.Round(Math.Ceiling(forgeInfo.Attack * attack)))

            let upgradeInfo = equipment.Info switch
            {
                EquipmentInfo info => (EquipmentUpgradeBaseInfo)new EquipmentUpgradeInfo(forgeInfo.AllStatBonus, getDefenseFunc(info.Defense),
                    this.context.EquipmentLevelForges.GetValueOrDefault(Tuple.Create(equipment.Info.InternalLevel, equipmentInfo.Upgrade))),
                WeaponInfo info => new WeaponUpgradeInfo(forgeInfo.AllStatBonus, getAttackFunc(info.Attack)),
                DefenseWeaponInfo info => new DefenseWeaponUpgradeInfo(forgeInfo.AllStatBonus, getAttackFunc(info.Attack), getDefenseFunc(info.Defense)),
                _ => throw new ArgumentOutOfRangeException($"{nameof(equipment.Info)}")
            }

            where !upgradeInfo.IsEmpty

            select new
            {
                pair.Key,

                Value = upgradeInfo
            };

        return request.ToDictionary(pair => pair.Key, pair => pair.Value);
    }
        
    public Dictionary<CharacterEquipmentIdentity, IBonusContainer<IBonusBase>> GetEquipmentStampInfo()
    {
        int x = 123;

        x = 356 switch
        {
            123 => 234,
        };

        var request = from pair in this.character.Equipments

            let equipmentInfo = pair.Value

            where equipmentInfo.StampVariant != null

            select new
            {
                pair.Key,

                StampVariant = (IBonusContainer<IBonusBase>)new VirtualBonusBaseContainer(this.GetStampBonuses(equipmentInfo))
            };

        return request.ToDictionary(pair => pair.Key, pair => pair.StampVariant);
    }

    public Dictionary<CharacterEquipmentIdentity, IBonusContainer<IBonusBase>> GetEquipmentDynamicBonuses()
    {
        var request = from pair in this.character.Equipments

            let equipmentInfo = pair.Value

            where equipmentInfo.Equipment.Bonuses.Any(bonus => bonus.Type.Variables.Any(var => var.IsDynamic()))

            select new
            {
                pair.Key,

                DynamicBonuses = (IBonusContainer<IBonusBase>)
                    new VirtualBonusBaseContainer(this.GetEquipmentBonuses(equipmentInfo.Equipment, true).Select(this.EvaluateDynamicBonus))
            };

        return request.ToDictionary(pair => pair.Key, pair => pair.DynamicBonuses);
    }

    public Dictionary<CharacterEquipmentIdentity, IEquipmentResultInfo> GetEquipmentResults()
    {
        var upgrades = this.GetEquipmentUpgradeInfo();
        var stamps = this.GetEquipmentStampInfo();
        var dynamicBonuses = this.GetEquipmentDynamicBonuses();


        var request = from key in upgrades.Keys.Concat(stamps.Keys).Concat(dynamicBonuses.Keys).Distinct()

            select new
            {
                Key = key,

                Value = (IEquipmentResultInfo)new EquipmentResultInfo
                {
                    Upgrade = upgrades.GetValueOrDefault(key),

                    StampVariant = stamps.GetValueOrDefault(key),

                    DynamicBonuses = dynamicBonuses.GetValueOrDefault(key)
                }
            };


        return request.ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    private bool IsDualWeapon(KeyValuePair<CharacterEquipmentIdentity, ICharacterEquipmentData> equipmentPair)
    {
        return equipmentPair.Value.Equipment.Type.Slot.IsWeapon == true
               && this.GetReverseEquipmentData(equipmentPair.Key).Maybe(reverseEquipment => reverseEquipment.Equipment.Type.Slot.IsWeapon == true);
    }

        

    private ICharacterEquipmentData? GetEquipmentData(CharacterEquipmentIdentity identity)
    {
        return this.character.Equipments.GetValueOrDefault(identity);
    }

    private ICharacterEquipmentData? GetReverseEquipmentData(CharacterEquipmentIdentity identity)
    {
        return identity.GetReverse().Maybe(this.GetEquipmentData);
    }

    internal bool IsEvaluated(IBonusTypeStat typeStat)
    {
        if (typeStat == null) throw new ArgumentNullException(nameof(typeStat));

        return !typeStat.Conditions.Any()
               || typeStat.Conditions.Any(condition => (condition.Event == null || condition.Event == this.character.Event)
                                                       && (condition.State == null || condition.State == this.character.State)
                                                       && (condition.Class == null || this.character.Class.IsSubsetOf(condition.Class))
                                                       && (condition.EquipmentType == null || this.IsEvaluatedEquipmentType(condition.EquipmentType, condition.PairEquipment.Value))
                                                       && (condition.IsMaxLevel == null || condition.IsMaxLevel.Value == (this.character.Level == this.context.LastVersion.MaxLevel))
                                                       && (condition.LostControl == null || condition.LostControl.Value == this.character.LostControl));
    }

    private bool IsEvaluatedEquipmentType(IEquipmentType equipmentType, bool isPair)
    {
        if (equipmentType == null) throw new ArgumentNullException(nameof(equipmentType));

        var minCount = isPair ? 2 : 1;

        return this.character.Equipments.Values.Count(eqData => eqData.Equipment.Type == equipmentType) >= minCount;
    }

    private IEnumerable<IBonusBase> GetStatBonuses(IStat stat, int statValue)
    {
        if (stat == null) throw new ArgumentNullException(nameof(stat));

        var request = from bonus in stat.Bonuses

            where bonus.Formula.Enabled

            let value = this.GetStatBonusValue(bonus, statValue)

            where value != 0

            select new VirtualBonusBase
            {
                Type = bonus.Type,
                Variables = new[] { value }.ToList()
            };

        var res = request.ToList();

        return res;
    }

    private decimal GetStatBonusValue(IStatBonus statBonus, int statValue)
    {
        if (statBonus == null) throw new ArgumentNullException(nameof(statBonus));

        var del = this.context.Formulas[statBonus.Formula];

        var res = del(new CalcState(this) { CustomVariables = new Dictionary<int, decimal> { { 0, statValue }}});

        return res;
    }



    public Dictionary<IStat, decimal> GetDescriptionValues(IReadOnlyDictionary<IStat, decimal> stats)
    {
        if (stats == null) throw new ArgumentNullException(nameof(stats));


        var request = from pair in stats

            let formula = pair.Key.DescriptionFormula

            where formula.IsEnabled()

            let del = this.context.Formulas[formula]

            select new
            {
                pair.Key,

                Value = del(new CalcState(this) { Stats = stats, CustomVariables = new Dictionary<int, decimal> { { 0, pair.Value } } })
            };


        return request.ToDictionary(pair => pair.Key, pair => pair.Value);
    }


    IClass IClassBuildSource.Class => this.character.Class;

    int ILevelObject.Level => this.character.Level;

    IGender ICharacterSourceBase.Gender => this.character.Gender;
}