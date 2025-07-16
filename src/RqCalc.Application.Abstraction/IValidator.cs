namespace RqCalc.Application;

public interface IValidator<in T>
{
    void Validate(T value);
}
