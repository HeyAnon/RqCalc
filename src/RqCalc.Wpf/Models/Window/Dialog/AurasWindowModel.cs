using System.Collections.ObjectModel;
using Framework.Core;
using RqCalc.Domain;
using RqCalc.Domain._Extensions;
using RqCalc.Wpf.Models._Base;

namespace RqCalc.Wpf.Models.Window.Dialog
{
    public class AurasWindowModel : ContextModel
    {
        public AurasWindowModel(IServiceProvider context, IClass currentClass, IAura currentAura, int currentLevel, IReadOnlyDictionary<IAura, bool> startupSharedAuras) 
            : base(context)
        {
            if (currentClass == null) throw new ArgumentNullException(nameof(currentClass));

            this.Auras = currentClass.GetAuras(currentLevel, context.LastVersion).ToObservableCollection();

            this.SharedAuras = this.Context.AurasSharedBonuses.OrderBy(aura => aura.Key.Name).ToObservableCollection(pair => new SharedAuraModel(this.Context, pair.Key) { Active = startupSharedAuras.ContainsKey(pair.Key), WithTalents = startupSharedAuras.GetValueOrDefault(pair.Key, true) });



            this.Aura = currentAura;

            if (this.Aura == null)
            {
                this.Aura = this.Auras.FirstOrDefault();
            }
        }


        public ObservableCollection<IAura> Auras
        {
            get { return this.GetValue(v => v.Auras); }
            set { this.SetValue(v => v.Auras, value); }
        }

        public IAura Aura
        {
            get { return this.GetValue(v => v.Aura); }
            set { this.SetValue(v => v.Aura, value); }
        }

        public ObservableCollection<SharedAuraModel> SharedAuras
        {
            get { return this.GetValue(v => v.SharedAuras); }
            private set { this.SetValue(v => v.SharedAuras, value); }
        }
    }
}