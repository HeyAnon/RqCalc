using System.Collections.ObjectModel;
using Framework.Core;
using RqCalc.Domain;
using RqCalc.Wpf.Models._Base;
using RqCalc.Wpf.Models.Window.Dialog._Base;

namespace RqCalc.Wpf.Models.Window.Dialog
{
    public class ElixirWindowModel : ContextModel, ILegacyModel, IClearModel
    {
        private readonly IClass currentClass;


        public ElixirWindowModel(IServiceProvider context, IClass currentClass, IElixir startupElixir)
            : base(context)
        {
            if (currentClass == null) throw new ArgumentNullException(nameof(currentClass));

            this.currentClass = currentClass;

            this.Elixir = startupElixir;

            this.ShowLegacy = this.Elixir.Maybe(e => e.IsLegacy);

            this.Refresh();

            this.SubscribeExplicit(rule => rule.Subscribe(v => v.ShowLegacy, this.Refresh));
        }


        public ObservableCollection<IElixir> Elixirs
        {
            get { return this.GetValue(v => v.Elixirs); }
            private set { this.SetValue(v => v.Elixirs, value); }
        }

        public IElixir Elixir
        {
            get { return this.GetValue(v => v.Elixir); }
            set { this.SetValue(v => v.Elixir, value); }
        }


        public bool ShowLegacy
        {
            get { return this.GetValue(v => v.ShowLegacy); }
            set { this.SetValue(v => v.ShowLegacy, value); }
        }


        public bool HasLegacy { get; } = true;

        public bool CloseDialog { get; } = true;


        public void Clear()
        {
            this.Elixir = null;
        }


        private void Refresh()
        {
            var request = from elixir in this.Context.DataSource.GetFullList<IElixir>()

                          where !elixir.IsLegacy || this.ShowLegacy

                          orderby elixir.GetOrderIndex(this.currentClass) descending
                          
                          select elixir;


            this.Elixirs = request.ToObservableCollection();

            if (this.Elixir == null)
            {
                this.Elixir = this.Elixirs.FirstOrDefault();
            }
        }
    }
}