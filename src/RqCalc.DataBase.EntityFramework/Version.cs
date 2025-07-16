using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("Version")]
public class Version : PersistentDomainObjectBase, IVersion
{
    public int MaxLevel { get; set; }

    public bool SharedAuras { get; set; }

    public bool SharedBuffs { get; set; }

    public bool CardBuffs { get; set; }

    public bool StampBuffs { get; set; }

    public bool LostControl { get; set; }

    public bool Collections { get; set; }

    public int MaxTalentLevel { get; set; }

    public bool SerializeCardBySlotType { get; set; }

    public bool GuildTalents { get; set; }
}