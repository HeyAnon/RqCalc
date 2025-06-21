using RqCalc.Wpf.Models._Base;

namespace RqCalc.Wpf.Models.Window.Dialog
{
    public class AboutWindowModel : ContextModel
    {
        public AboutWindowModel(IServiceProvider context, Version version)
            : base(context)
        {
            this.Version = version;
        }


        public Version Version { get; }

        public int SerializerVersion => this.Context.LastVersion.Id;
    }
}