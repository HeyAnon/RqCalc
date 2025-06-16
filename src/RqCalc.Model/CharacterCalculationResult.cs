using RqCalc.Domain;

namespace RqCalc.Model;

public class CharacterCalculationResult(
    string Code,
    IReadOnlyDictionary<IStat, decimal> Stats,
    IReadOnlyDictionary<CharacterEquipmentIdentity, IEquipmentResultInfo> Equipments,
    IReadOnlyDictionary<IStat, decimal> StatDescriptions);