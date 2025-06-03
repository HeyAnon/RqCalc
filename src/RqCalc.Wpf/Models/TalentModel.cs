using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

using Framework.Reactive;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class TalentModel : ActiveImageChangeModel<ITalent>
    {
        public readonly TalentBranchModel Branch;


        public TalentModel(IApplicationContext context, IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats, TalentBranchModel branch, bool isUltimate, ITalent talent)
            : base(context)
        {
            if (evaluateStats == null) throw new ArgumentNullException(nameof(evaluateStats));

            this.Branch = branch ?? throw new ArgumentNullException(nameof(branch));
            this.IsUltimate = isUltimate;
            this.SelectedObject = talent ?? throw new ArgumentNullException(nameof(talent));

            this.MainDescription = new TalentDescriptionModel(this.Context, evaluateStats, talent.GetMainDescription());
            this.PassiveDescription = talent.GetPassiveDescription().Maybe(desc => new TalentDescriptionModel(this.Context, evaluateStats, desc));
        }


        public TalentDescriptionModel MainDescription
        {
            get { return this.GetValue(v => v.MainDescription); }
            private set { this.SetValue(v => v.MainDescription, value); }
        }


        public TalentDescriptionModel PassiveDescription
        {
            get { return this.GetValue(v => v.PassiveDescription); }
            private set { this.SetValue(v => v.PassiveDescription, value); }
        }

        public bool IsUltimate
        {
            get { return this.GetValue(v => v.IsUltimate); }
            private set { this.SetValue(v => v.IsUltimate, value); }
        }

        public bool HasEquipmentCondition => !string.IsNullOrWhiteSpace(this.SelectedObject.EquipmentCondition);

        public bool HasPassive => this.PassiveDescription != null;

        public int HIndex => this.SelectedObject.HIndex;

        public IEnumerable<TalentModel> ParallelTalents => this.Branch.TalentList.Where(tal => tal.HIndex == this.HIndex && tal != this);


        public void SwitchActive()
        {
            this.Branch.WindowModel.SwitchActive(this);
        }
    }
}