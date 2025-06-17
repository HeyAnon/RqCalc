namespace RqCalc.Tests.__Support;

public class TestBase(IServiceProvider serviceProvider)
{
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;
}