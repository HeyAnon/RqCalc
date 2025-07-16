using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;

using RqCalc.Domain._Base;
using RqCalc.Domain.BonusType;
using RqCalc.Domain.Card;
using RqCalc.Domain.Stamp;
using RqCalc.Domain.VirtualBonus;

namespace RqCalc.Domain._Extensions;

public static class BonusExtensions
{
    public static string PartialEvaluateTemplate(this IBonusBase bonus)
    {
        if (bonus == null) throw new ArgumentNullException(nameof(bonus));

        return string.Format(bonus.Type.Template, bonus.Variables.Select((value, index) =>
        {
            var hasSign = bonus.Type.Variables.Any(v => v.Index == index && v.HasSign);

            var isDynamic = bonus.Type.Variables.Any(v => v.Index == index && v.IsDynamic());
                
            if (isDynamic)
            {
                return "dynamic";
            }
            else if (index == 0)
            {
                return value > 0 && hasSign ? "+{0}" : "{0}";
            }
            else
            {
                return value > 0 && hasSign ? ("+" + value) : (object)value;    
            }
        }).ToArray());
    }

    public static string EvaluateTemplate(this IBonusBase bonus)
    {
        if (bonus == null) throw new ArgumentNullException(nameof(bonus));

        return string.Format(bonus.Type.Template, bonus.Variables.Select((value, index) =>
        {
            var hasSign = bonus.Type.Variables.Any(v => v.HasSign && v.Index == index);

            return value > 0 && hasSign ? ("+" + Math.Ceiling(value)) : (object)value;
        }).ToArray());
    }

    //public static string EvaluateTemplateSign(this IBonusType bonusType)
    //{
    //    if (bonusType == null) throw new ArgumentNullException("bonusType");

    //    return bonusType.Variables.Where(var => var.HasSign).Aggregate(bonusType.Template, (state, var) => state.Replace("{" + var.Index + "}", "{" + var.Index + "}"));
    //        string.Format(bonusType.Template, bonus.Variables.ToArray((value, index) =>
    //    {
    //        var hasSign = bonus.Type.Variables.Any(v => v.HasSign && v.Index == index);

    //        return value > 0 && hasSign ? ("+" + Math.Ceiling(value)) : (object)value;
    //    }));
    //}

    public static IBonusContainer<IBonusBase> Multiply(this IBonusContainer<IBonusBase> bonusContainer, int count)
    {
        if (bonusContainer == null) throw new ArgumentNullException(nameof(bonusContainer));

        return new VirtualBonusBaseContainer(bonusContainer.Bonuses.Select(bonus => bonus.Multiply(count)));
    }

    public static IBonusBase Multiply(this IBonusBase bonusBase, int count)
    {
        if (bonusBase == null) throw new ArgumentNullException(nameof(bonusBase));

        return new VirtualBonusBase
        {
            Type = bonusBase.Type,
            Variables = bonusBase.Variables.ToList(v => v * count)
        };
    }

    public static IBonusBase ToBonusBase(this ICardBonus cardBonus)
    {
        if (cardBonus == null) throw new ArgumentNullException(nameof(cardBonus));
            
        return new VirtualBonusBase
        {
            Type = cardBonus.Type,
            Variables = cardBonus.GetBonusBaseVariables()
        };
    }

    private static List<decimal> GetBonusBaseVariables(this ICardBonus cardBonus)
    {
        if (cardBonus == null) throw new ArgumentNullException(nameof(cardBonus));
            
        if (cardBonus.UpgradeEquipmentInfo != null)
        {
            var mainVariable = cardBonus.Variables.Single();

            return
            [
                mainVariable.Value,
                cardBonus.UpgradeEquipmentInfo.Step,
                cardBonus.UpgradeEquipmentInfo.Condition
            ];
        }
        else
        {
            return cardBonus.Variables.ToList(v => (decimal)v.Value);
        }
    }

        
    public static IEnumerable<IStat> GetFilterStats(this IBonusType bonusType)
    {
        if (bonusType == null) throw new ArgumentNullException(nameof(bonusType));
            
        var request = from bonusStat in bonusType.Stats
                          
            let stat = bonusStat.Stat.GetRoot()
                          
            where stat.IsMelee != null || stat.Type == StatType.Primary || stat.Type == StatType.Energy
                          
            select stat;

        return request.Distinct();
    }



    public static int GetOrderIndex(this IStamp stamp)
    {
        if (stamp == null) throw new ArgumentNullException(nameof(stamp));
            
        return stamp.Variants.OrderBy(v => v.Id).Last().GetOrderIndex();
    }

    public static int GetOrderIndex(this IBonusContainer<ITypeObject<IBonusType>> bonusContainer)
    {
        if (bonusContainer == null) throw new ArgumentNullException(nameof(bonusContainer));

        var request = from bonus in bonusContainer.Bonuses

            from bonusStat in bonus.Type.Stats

            select bonusStat.Stat.GetRoot().GetOrderIndex();

        return request.Distinct().Sum();
    }

    public static int GetOrderIndex(this IBonusContainer<ITypeObject<IBonusType>> bonusContainer, IClass @class)
    {
        if (bonusContainer == null) throw new ArgumentNullException(nameof(bonusContainer));
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        var request = from bonus in bonusContainer.Bonuses
                          
            from bonusStat in bonus.Type.Stats

            select bonusStat.Stat.GetRoot().GetOrderIndex(@class);

        return request.Distinct().Sum();
    }

    private static int GetOrderIndex(this IStat stat, IClass @class)
    {
        if (stat == null) throw new ArgumentNullException(nameof(stat));
        if (@class == null) throw new ArgumentNullException(nameof(@class));

        if (stat.Type == StatType.Primary)
        {
            return stat == @class.PrimaryStat ? stat.OrderIndex : -stat.OrderIndex;
        }
        else if (stat.Type == StatType.Energy)
        {
            return stat == @class.EnergyStat ? stat.OrderIndex : -stat.OrderIndex;
        }
        else if (stat.IsMelee != null)
        {
            return stat.IsMelee.Value == @class.IsMelee ? stat.OrderIndex : -stat.OrderIndex;
        }
        else
        {
            return stat.OrderIndex;
        };
    }


    private static int GetOrderIndex(this IStat stat)
    {
        if (stat == null) throw new ArgumentNullException(nameof(stat));

        if (stat.Type == StatType.Primary)
        {
            return 0;
        }
        else if (stat.Type == StatType.Energy)
        {
            return 0;
        }
        else if (stat.IsMelee != null)
        {
            return 0;
        }
        else
        {
            return stat.OrderIndex;
        };
    }
}