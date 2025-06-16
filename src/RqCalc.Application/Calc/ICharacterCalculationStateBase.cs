using RqCalc.Domain;
using RqCalc.Model;

namespace RqCalc.Application.Calc;

public interface ICharacterCalculationStateBase : ICharacterSourceBase
{
    WeaponInfo? CurrentWeaponInfo { get; }
}