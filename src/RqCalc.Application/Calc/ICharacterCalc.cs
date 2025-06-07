using RqCalc.Domain;
using RqCalc.Model;

namespace RqCalc.Application.Calc;

internal interface ICharacterCalc : ICharacterSourceBase
{
    WeaponInfo? CurrentWeaponInfo { get; }
}