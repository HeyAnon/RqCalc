namespace RqCalc.Wpf.Models._Base
{
    public class ContextModel(IServiceProvider context) : NotifyModelBase, IClientContext
    {
        public IServiceProvider Context { get; } = context;
    }
}