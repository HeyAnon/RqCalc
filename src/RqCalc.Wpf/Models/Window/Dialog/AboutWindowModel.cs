using System;

using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class AboutWindowModel : ContextModel
    {
        public AboutWindowModel(IApplicationContext context, Version version)
            : base(context)
        {
            this.Version = version;
        }


        public Version Version { get; }

        public int SerializerVersion => this.Context.LastVersion.Id;
    }
}