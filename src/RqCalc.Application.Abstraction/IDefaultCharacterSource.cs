using RqCalc.Model;

namespace RqCalc.Application;

public interface IDefaultCharacterSource
{
    public ICharacterSource GetDefaultCharacter();
}