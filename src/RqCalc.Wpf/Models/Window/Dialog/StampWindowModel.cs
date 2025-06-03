using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Framework.Core;

using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class StampWindowModel : ContextModel, IClearModel, ILegacyModel
    {
        private readonly IEquipment _equipment;

        private readonly IClass _currentClass;

        private readonly IStampColor _bestColor;

        private readonly IReadOnlyDictionary<IStampVariant, bool> _baseVariants;


        public StampWindowModel(IApplicationContext context, IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats, IEquipment equipment, IClass currentClass, IStampVariant startupStampVariant)
            : base(context)
        {
            this._equipment = equipment ?? throw new ArgumentNullException(nameof(equipment));
            this._currentClass = currentClass ?? throw new ArgumentNullException(nameof(currentClass));
            
            this.StampColors = this.Context.DataSource.GetFullList<IStampColor>().OrderById().ToObservableCollection();

            this._bestColor = this.StampColors.Last();

            this.StampVariant = startupStampVariant;
            
            {
                var baseVariantsRequest = from stamp in this.Context.DataSource.GetFullList<IStamp>()

                                          let allowed = this.Context.IsAllowedStamp(stamp, equipment, currentClass)

                                          where allowed != false
                                         
                                          let variant = stamp.GetByColor(this._bestColor)
                                         
                                          select variant.ToKeyValuePair(allowed == true);

                this._baseVariants = baseVariantsRequest.ToDictionary();
            }
            


            if (this.StampColor == null)
            {
                this.StampColor = this._bestColor;
            }


            this.ShowLegacy = this.Stamp.Maybe(stamp => stamp.IsLegacy);
            this.ShowShared = this.Stamp.Maybe(stamp => this._baseVariants.Any(pair => pair.Key.Stamp == stamp && !pair.Value));

            this.Refresh();

            this.SubscribeExplicit(rule => rule.Subscribe(v => v.ShowLegacy, this.Refresh));
            this.SubscribeExplicit(rule => rule.Subscribe(v => v.ShowShared, this.Refresh));
        }


        public bool HasLegacy => this._baseVariants.Keys.Any(v => v.Stamp.IsLegacy);

        public bool HasShared => this._baseVariants.Values.Any(v => !v);

        public bool ShowShared
        {
            get { return this.GetValue(v => v.ShowShared); }
            set { this.SetValue(v => v.ShowShared, value); }
        }

        public ObservableCollection<IStampVariant> DesignStampVariants
        {
            get { return this.GetValue(v => v.DesignStampVariants); }
            private set { this.SetValue(v => v.DesignStampVariants, value); }
        }

        public ObservableCollection<IStampColor> StampColors
        {
            get { return this.GetValue(v => v.StampColors); }
            private set { this.SetValue(v => v.StampColors, value); }
        }


        public IStampVariant DesignStampVariant
        {
            get { return this.GetValue(v => v.DesignStampVariant); }
            set { this.SetValue(v => v.DesignStampVariant, value); }
        }

        public IStampColor StampColor
        {
            get { return this.GetValue(v => v.StampColor); }
            set { this.SetValue(v => v.StampColor, value); }
        }


        public IStamp Stamp
        {
            get { return this.DesignStampVariant.Maybe(sv => sv.Stamp); }
            set { this.DesignStampVariant = value.Maybe(stamp => stamp.GetByColor(this._bestColor)); }
        }

        public IStampVariant StampVariant
        {
            get
            {
                return this.Stamp.Maybe(s => s.GetByColor(this.StampColor));
            }
            set
            {
                this.Stamp = value.Maybe(v => v.Stamp);
                this.StampColor = value.Maybe(v => v.Color, this._bestColor);
            }
        }

        public bool ShowLegacy
        {
            get { return this.GetValue(v => v.ShowLegacy); }
            set { this.SetValue(v => v.ShowLegacy, value); }
        }

        public bool CloseDialog { get; } = true;

        public void Clear()
        {
            this.Stamp = null;
        }


        private void Refresh()
        {
            var request = from pair in this._baseVariants

                          let variant = pair.Key

                          where pair.Value || this.ShowShared

                          where !variant.Stamp.IsLegacy || this.ShowLegacy

                          orderby variant.GetOrderIndex(this._currentClass) descending

                          select variant;


            this.DesignStampVariants = request.ToObservableCollection();

            if (this.DesignStampVariant == null)
            {
                this.DesignStampVariant = this.DesignStampVariants.FirstOrDefault();
            }
        }
    }
}