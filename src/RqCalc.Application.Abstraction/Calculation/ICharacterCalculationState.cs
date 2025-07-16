using RqCalc.Domain;
using RqCalc.Model;

namespace RqCalc.Application.Calculation;

public interface ICharacterCalculationState : ICharacterSourceBase
{
    WeaponInfo? CurrentWeaponInfo { get; }
}