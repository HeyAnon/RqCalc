using RqCalc.Domain;

namespace RqCalc.Model.Impl;

public class CharacterResult : ICharacterResult
{
    public string Code { get; set; }

    //public string TalentCode { get; set; }

    //public string GuildTalentCode { get; set; }


    public Dictionary<IStat, decimal> Stats { get; set; }

    public Dictionary<CharacterEquipmentIdentity, IEquipmentResultInfo> Equipments { get; set; }

    public Dictionary<IStat, decimal> StatDescriptions { get; set; }



    IReadOnlyDictionary<IStat, decimal> ICharacterResult.Stats => this.Stats;

    IReadOnlyDictionary<CharacterEquipmentIdentity, IEquipmentResultInfo> ICharacterResult.Equipments => this.Equipments;

    IReadOnlyDictionary<IStat, decimal> ICharacterResult.StatDescriptions => this.StatDescriptions;
}