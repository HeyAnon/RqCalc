using System.Collections.ObjectModel;
using Framework.Core;
using Framework.Reactive;
using RqCalc.Domain;
using RqCalc.Model;

namespace RqCalc.Wpf.Models;

public class TalentDescriptionModel : NotifyModelBase
{
    public TalentDescriptionModel(IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats, ITalentDescription talentDescription)

    {
        if (evaluateStats == null) throw new ArgumentNullException(nameof(evaluateStats));
        if (talentDescription == null) throw new ArgumentNullException(nameof(talentDescription));

        this.Body = new TextTemplateModel(this.Context, evaluateStats, talentDescription.Body);
        this.Buffs = talentDescription.Buffs.ToObservableCollection(buff => new TextTemplateModel(this.Context, evaluateStats, buff));
    }


    public TextTemplateModel Body
    {
        get { return this.GetValue(v => v.Body); }
        private set { this.SetValue(v => v.Body, value); }
    }

    public ObservableCollection<TextTemplateModel> Buffs
    {
        get { return this.GetValue(v => v.Buffs); }
        private set { this.SetValue(v => v.Buffs, value); }
    }
}