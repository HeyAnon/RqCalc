using RqCalc.Model;

namespace RqCalc.Application;

public interface ICharacterValidator
{
    void Validate(ICharacterSource character);
}