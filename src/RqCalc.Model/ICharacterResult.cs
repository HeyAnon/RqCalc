using RqCalc.Domain;

namespace RqCalc.Model;

public interface ICharacterResult
{
    string Code { get; }

    //string TalentCode { get; }

    //string GuildTalentCode { get; }


    IReadOnlyDictionary<IStat, decimal> Stats { get; }

    IReadOnlyDictionary<CharacterEquipmentIdentity, IEquipmentResultInfo> Equipments { get; }


    IReadOnlyDictionary<IStat, decimal> StatDescriptions { get; }
}