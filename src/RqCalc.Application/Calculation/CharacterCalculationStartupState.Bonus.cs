using Framework.Core;

using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Domain.BonusType;
using RqCalc.Domain.Card;
using RqCalc.Domain.Equipment;
using RqCalc.Domain.VirtualBonus;
using RqCalc.Model;

namespace RqCalc.Application.Calculation;

public partial class CharacterCalculationStartupState
{
    private IEnumerable<IBonusBase> GetBonuses(BonusSource source)
    {
        if (source.HasFlag(BonusSource.Class))
        {
            foreach (var bonus in character.Class.Bonuses)
            {
                yield return bonus;
            }
        }

        if (source.HasFlag(BonusSource.Equipment))
        {
            foreach (var equipmentInfo in character.Equipments.Values)
            {
                var equipment = equipmentInfo.Equipment;

                if (equipment.IsAllowed(character.State))
                {
                    foreach (var bonus in GetEquipmentBonuses(equipment))
                    {
                        if (bonus.Activate == null || (bonus.Activate.Value == equipmentInfo.Active))
                        {
                            yield return bonus;
                        }
                    }

                    foreach (var bonus in equipment.Type.Bonuses)
                    {
                        yield return bonus;
                    }

                    if (equipmentInfo.Elixir != null)
                    {
                        foreach (var bonus in equipmentInfo.Elixir.Bonuses)
                        {
                            yield return bonus;
                        }
                    }
                }
            }
        }

        if (source.HasFlag(BonusSource.Stamp))
        {
            foreach (var equipmentInfo in character.Equipments.Values)
            {
                if (equipmentInfo.StampVariant != null && equipmentInfo.Equipment.IsAllowed(character.State))
                {
                    foreach (var bonus in this.GetStampBonuses(equipmentInfo))
                    {
                        yield return bonus;
                    }
                }
            }
        }

        if (source.HasFlag(BonusSource.Elixir) && character.EnableElixir && character.Elixir != null)
        {
            foreach (var bonus in character.Elixir.Bonuses)
            {
                yield return bonus;
            }
        }

        if (source.HasFlag(BonusSource.Consumable) && character.EnableConsumables)
        {
            foreach (var consumable in character.Consumables)
            {
                foreach (var bonus in consumable.Bonuses)
                {
                    yield return bonus;
                }
            }
        }

        if (source.HasFlag(BonusSource.Guild) && character.EnableGuildTalents)
        {
            foreach (var guildTalentPair in character.GuildTalents)
            {
                var guildTalent = guildTalentPair.Key;

                var points = guildTalentPair.Value;
                    
                foreach (var guildTalentBonus in guildTalent.Bonuses)
                {
                    var variables = guildTalentBonus.Variables.ToDictionary(v => v.Index, v => guildTalent.Variables.Single(vv => vv.Index == v.TalentVariableIndex && vv.Points == points).Value).ToArrayI();

                    yield return new VirtualBonusBase
                    {
                        Type = guildTalentBonus.Type,
                        Variables = variables.ToList()
                    };
                }
            }
        }

        if (source.HasFlag(BonusSource.Aura) && character.EnableAura)
        {
            var mainAuraTalentBonuses = from talent in character.Talents

                from bonus in talent.Bonuses

                where bonus.AuraCondition == character.Aura

                select bonus;

            var mainBonuses = character.Aura == null ? [] : character.EnableTalents ? character.Aura.GetBonuses(lastVersion, false, mainAuraTalentBonuses) : character.Aura.GetBonuses(lastVersion, false, false);



            var allAuraBonuses = from auraPair in new[] { new { Aura = character.Aura, Bonuses = mainBonuses.ToArray() } }

                    .Concat(character.SharedAuras.Select(pair => new { Aura = pair.Key, Bonuses = pair.Key.GetBonuses(lastVersion, true, pair.Value).ToArray()}))

                from bonus in auraPair.Bonuses
                                     
                group bonus.Variables by new { auraPair.Aura, bonus.Type } into bonusGroup

                select new VirtualBonusBase
                {
                    Type = bonusGroup.Key.Type,

                    Variables = bonusGroup.Aggregate((l1, l2) => l1.ZipStrong(l2, Math.Max).ToList()).ToList()
                };

            foreach (var bonus in allAuraBonuses)
            {
                yield return bonus;
            }
        }

        if (source.HasFlag(BonusSource.Buff) && character.EnableBuffs)
        {
            foreach (var buffPair in character.Buffs)
            {
                var buff = buffPair.Key;

                if (buff.Class != null)
                {
                    if (buff.TalentCondition == null || (character.EnableTalents && character.Talents.Contains(buffPair.Key.TalentCondition)))
                    {
                        var buffBonusesRequest = from bonus in buff.Bonuses

                            select new { Bonus = (IBonusBase)bonus, Priority = 0 };

                        var talentBuffBonusesRequest = from talent in character.Talents

                            from bonus in talent.Bonuses

                            where bonus.BuffCondition == buff

                            select new { Bonus = (IBonusBase)bonus, Priority = 1 };


                        var totalBonusRequest = from pair in character.EnableTalents ? buffBonusesRequest.Concat(talentBuffBonusesRequest) : buffBonusesRequest

                            group pair by pair.Bonus.Type into bonusTypeGroup

                            let activeBonusPair = bonusTypeGroup.OrderByDescending(pair => pair.Priority).First()

                            select activeBonusPair.Bonus;


                        foreach (var buffBonus in totalBonusRequest)
                        {
                            yield return buffBonus.Multiply(buffPair.Value);
                        }
                    }
                }
                else
                {
                    foreach (var buffBonus in buff.Bonuses)
                    {
                        yield return buffBonus.Multiply(buffPair.Value);
                    }
                }
            }
        }

        if (source.HasFlag(BonusSource.Talent) && character.EnableTalents)
        {
            foreach (var talent in character.Talents)
            {
                foreach (var bonus in talent.Bonuses)
                {
                    if (bonus.AuraCondition == null && bonus.BuffCondition == null)
                    {
                        yield return bonus;
                    }
                }
            }
        }

        if (source.HasFlag(BonusSource.Card))
        {
            var request = from equipmentInfo in character.Equipments.Values

                let equipment = equipmentInfo.Equipment

                where equipment.IsAllowed(character.State)

                from card in equipmentInfo.Cards

                where card != null

                from cardBonus in card.Bonuses

                let expandedCardBonus = new VirtualBonusBase
                {
                    Type = cardBonus.Type,

                    Variables = this.GetCardBonusVariables(equipmentInfo, cardBonus).ToArrayI().ToList()
                }

                where expandedCardBonus.Variables.Any()
                              
                group expandedCardBonus by new { Card = card, BonusType = expandedCardBonus.Type } into bonusGroup

                from bonus in bonusTypeService.AttackBonusTypes.Contains(bonusGroup.Key.BonusType) ? GroupByType(bonusGroup, bonusGroup.Key.BonusType) : bonusGroup

                select bonus;

            foreach (var bonus in request)
            {
                yield return bonus;
            }
        }

        if (source.HasFlag(BonusSource.Collection) && character.EnableCollecting)
        {
            foreach (var collectedItem in character.CollectedItems)
            {
                foreach (var bonus in collectedItem.Bonuses)
                {
                    yield return bonus;
                }
            }
        }
    }

    private static IEnumerable<IBonusBase> GroupByType(IEnumerable<IBonusBase> bonuses, IBonusType bonusType)
    {
        if (bonuses == null) throw new ArgumentNullException(nameof(bonuses));

        var calcVars = bonusType.Stats.ToHashSet(st => st.VarIndex);

        var bonusesCache = bonuses.ToArray();

        var variablesRequest = from bonus in bonusesCache

            from variablePair in bonus.Variables.Select((v, i) => new { Value = v, Index = i })

            group variablePair.Value by variablePair.Index into varGroup

            select new
            {
                varGroup.Key,

                Values = calcVars.Contains(varGroup.Key) ? [varGroup.Sum()] : varGroup.Distinct().ToArray()
            };
            
        var variables = variablesRequest.ToList();

        var result = variables.Any(v => v.Values.Length > 1) ? bonusesCache :
        [
            new VirtualBonusBase
            {
                Type = bonusType,

                Variables = variables.ToDictionary(v => v.Key, v => v.Values.Single()).ToArrayI().ToList()
            }
        ];

        return result;
    }

    private Dictionary<int, decimal> GetCardBonusVariables(ICharacterEquipmentData characterEquipment, ICardBonus cardBonus)
    {
        if (characterEquipment == null) throw new ArgumentNullException(nameof(characterEquipment));
        if (cardBonus == null) throw new ArgumentNullException(nameof(cardBonus));

        if (cardBonus.RequiredCard != null && this.GetCardCount(cardBonus.RequiredCard) == 0)
        {
            return new Dictionary<int, decimal>();
        }

        var defaultBonus = this.GetDefaultCardBonusVariables(characterEquipment, cardBonus);

        return this.ProcessCardBonusVariablesFuncList(cardBonus).Aggregate(defaultBonus, (state, f) => f(state));
    }

    private IEnumerable<Func<Dictionary<int, decimal>,  Dictionary<int, decimal>>> ProcessCardBonusVariablesFuncList(ICardBonus cardBonus) =>
    [
        state =>
        {
            if (cardBonus.MultiplyEffectCard != null)
            {
                var multiplyEffectCardCount = this.GetCardCount(cardBonus.MultiplyEffectCard);

                return state.ChangeValue(v => v * multiplyEffectCardCount);
            }
            else
            {
                return state;
            }
        },

        state =>
        {
            if (cardBonus.NegateCard != null)
            {
                var negateCardCount = this.GetCardCount(cardBonus.NegateCard);

                return state.ChangeValue(v => v * (1 - negateCardCount));
            }
            else
            {
                return state;
            }
        },

        state =>
        {
            if (cardBonus.RequiredSet != null)
            {
                var cardSetCount = this.GetCardCount(card => card.Set == cardBonus.RequiredSet);

                if (cardSetCount < cardBonus.RequiredSetSize)
                {
                    return new Dictionary<int, decimal>();
                }
            }

            return state;
        }
    ];

    private Dictionary<int, decimal> GetDefaultCardBonusVariables(ICharacterEquipmentData characterEquipment, ICardBonus cardBonus)
    {
        if (characterEquipment == null) throw new ArgumentNullException(nameof(characterEquipment));
        if (cardBonus == null) throw new ArgumentNullException(nameof(cardBonus));

        var equipment = characterEquipment.Equipment;

        if (cardBonus.UpgradeEquipmentInfo != null)
        {
            if (cardBonus.UpgradeEquipmentInfo.Condition >= characterEquipment.Upgrade)
            {
                return new Dictionary<int, decimal>();
            }

            var mainVariable = cardBonus.Variables.Single();

            var stepBonus = cardBonus.UpgradeEquipmentInfo.Step == 0 ? 1 : (characterEquipment.Upgrade - cardBonus.UpgradeEquipmentInfo.Condition) / cardBonus.UpgradeEquipmentInfo.Step;

            var calcBonus = mainVariable.Value * stepBonus;

            return new[] { mainVariable.Value, cardBonus.UpgradeEquipmentInfo.Step, cardBonus.UpgradeEquipmentInfo.Condition, calcBonus }.Select((value, index) => (index + mainVariable.Index, (decimal)value)).ToDictionary();
        }
        else
        {
            var res = cardBonus.Variables.ToDictionary(v => v.Index, variable =>
            {
                var allowed = !variable.Conditions.Any() || variable.Conditions.Any(condition => (condition.EquipmentType == null || condition.EquipmentType == equipment.Type)
                                                                                                 && (condition.IsSingleHandWeapon == null || condition.IsSingleHandWeapon == equipment.Type.WeaponInfo!.IsSingleHand));

                return allowed ? variable.Value : 0M;
            });

            return res;
        }
    }

    private int GetCardCount(ICard card)
    {
        if (card == null) throw new ArgumentNullException(nameof(card));

        return this.GetCardCount(c => c == card);
    }

    private int GetCardCount(Func<ICard, bool> filter) => character.Equipments.SelectMany(pair => pair.Value.Cards).Where(c => c != null).Count(filter!);

    private IEnumerable<IBonusBase> GetStampBonuses(ICharacterEquipmentData equipmentInfo)
    {
        if (equipmentInfo == null) throw new ArgumentNullException(nameof(equipmentInfo));

        if (equipmentInfo.StampVariant != null)
        {
            var equipment = equipmentInfo.Equipment;

            var internalLevel = equipment.Info!.InternalLevel;

            foreach (var bonus in equipmentInfo.StampVariant.Bonuses.OrderBy(bonus => bonus.OrderIndex))
            {
                var value = MathHelper.GetStampValue(bonus.QualityValue, internalLevel, bonus.Type.StampQuality!.MinCoef, bonus.Type.StampQuality.MaxCoef, 10, settings.QualityMaxLevel);

                yield return new VirtualBonusBase
                {
                    Type = bonus.Type,

                    Variables = new[] { value }.ToList()
                };
            }
        }
    }

    private static IEnumerable<IEquipmentBonus> GetEquipmentBonuses(IEquipment equipment, bool? dynamic = null)
    {
        if (equipment == null) throw new ArgumentNullException(nameof(equipment));

        return from bonus in equipment.Bonuses

            where dynamic == null || (dynamic == bonus.Type.Variables.Any(v => v.MulFormula != null))

            select bonus;
    }        
}