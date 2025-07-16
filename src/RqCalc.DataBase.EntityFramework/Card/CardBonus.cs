using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;
using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;
using RqCalc.Domain.BonusType;
using RqCalc.Domain.Card;

namespace RqCalc.DataBase.EntityFramework.Card;

[Table("CardBonus")]
public partial class CardBonus : PersistentDomainObjectBase
{
    public virtual HashSet<CardBonusVariable> Variables { get; set; } = null!;

    public virtual Card Card { get; set; } = null!;

    public virtual BonusType.BonusType Type { get; set; } = null!;

    public virtual Card? RequiredCard { get; set; }

    public virtual Card? NegateCard { get; set; }

    public virtual Card? MultiplyEffectCard { get; set; }
    
    public virtual CardSet? RequiredSet { get; set; }



    public virtual int RequiredSetSize { get; set; }

    public int OrderIndex { get; set; }

    public int? UpgConditionVariable { get; set; }

    public int? UpgLevelStepVariable { get; set; }

    
    [Column("RequiredSet_Id")]
    public int? RequiredSetId { get; set; }


    [Column("Type_Id")]
    public int? TypeId { get; set; }
        

    public override string ToString() => $"BonusType: {this.Type.Template} | Values: {this.Variables.Join(", ", v => $"{v.Value} [{v.Index}]")}";
}

public partial class CardBonus : ICardBonus
{   
    private readonly Lazy<CardUpgradeEquipmentInfo?> lazyInfo;


    public CardBonus() => this.lazyInfo = LazyHelper.Create(this.GetInfo);

    public CardUpgradeEquipmentInfo? UpgradeEquipmentInfo => this.lazyInfo.Value;


    IReadOnlyCollection<ICardBonusVariable> ICardBonus.Variables => this.Variables;

    ICard ICardBonus.Card => this.Card;

    ICard? ICardBonus.RequiredCard => this.RequiredCard;

    ICard? ICardBonus.NegateCard => this.NegateCard;

    ICard? ICardBonus.MultiplyEffectCard => this.MultiplyEffectCard;

    ICardSet? ICardBonus.RequiredSet => this.RequiredSet;

    IBonusType Framework.Persistent.ITypeObject<IBonusType>.Type => this.Type;

    private CardUpgradeEquipmentInfo? GetInfo()
    {
        if (this.UpgConditionVariable != null && this.UpgLevelStepVariable != null)
        {
            return new CardUpgradeEquipmentInfo(this.UpgConditionVariable.Value, this.UpgLevelStepVariable.Value);
        }
        else if (this.UpgConditionVariable == null && this.UpgLevelStepVariable == null)
        {
            return null;
        }
        else
        {
            throw new Exception("Invalid UpgradeEquipmentInfo");
        }
    }
}