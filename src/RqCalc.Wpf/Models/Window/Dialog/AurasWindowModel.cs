using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Framework.Core;

using Framework.Reactive;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class AurasWindowModel : ContextModel
    {
        public AurasWindowModel(IApplicationContext context, IClass currentClass, IAura currentAura, int currentLevel, IReadOnlyDictionary<IAura, bool> startupSharedAuras) 
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