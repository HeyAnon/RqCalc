using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public interface IClientContext
    {
        IApplicationContext Context { get; }
    }
}