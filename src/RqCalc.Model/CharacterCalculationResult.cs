using RqCalc.Domain;

namespace RqCalc.Model;

public record CharacterCalculationResult(
    string Code,
    IReadOnlyDictionary<IStat, decimal> Stats,
    IReadOnlyDictionary<CharacterEquipmentIdentity, IEquipmentResultInfo> Equipments,
    IReadOnlyDictionary<IStat, decimal> StatDescriptions);