using RqCalc.Core;
using RqCalc.Model;

namespace RqCalc.Logic.Calc;

internal interface ICharacterCalc : ICharacterSourceBase
{
    WeaponInfo CurrentWeaponInfo { get; }
}