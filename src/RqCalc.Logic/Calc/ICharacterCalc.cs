using RqCalc.Core;
using RqCalc.Domain.Model;

namespace RqCalc.Logic.Calc;

internal interface ICharacterCalc : ICharacterSourceBase
{
    WeaponInfo CurrentWeaponInfo { get; }
}