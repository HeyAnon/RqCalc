using RqCalc.Domain;
using RqCalc.Model;

namespace RqCalc.Application.Calc;

public interface ICharacterCalc : ICharacterSourceBase
{
    WeaponInfo? CurrentWeaponInfo { get; }
}