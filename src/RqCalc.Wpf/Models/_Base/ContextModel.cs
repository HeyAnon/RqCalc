using System;


using Framework.Reactive;

using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class ContextModel : NotifyModelBase, IClientContext
    {
        public ContextModel(IApplicationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            this.Context = context;
        }

        public IApplicationContext Context { get; }
    }
}