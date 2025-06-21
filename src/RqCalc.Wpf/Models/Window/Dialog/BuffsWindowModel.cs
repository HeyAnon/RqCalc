using System.Collections.ObjectModel;
using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Reactive;
using RqCalc.Domain;
using RqCalc.Domain._Extensions;
using RqCalc.Model;
using RqCalc.Model._Extensions;
using RqCalc.Wpf.Models.Window.Dialog._Base;

namespace RqCalc.Wpf.Models.Window.Dialog;

public class BuffsWindowModel : NotifyModelBase, IClearModel
{
    public BuffsWindowModel(ICharacterSource character)

    {
        if (character == null) throw new ArgumentNullException(nameof(character));

        this.ClassBuffList = character.Class.GetAllParents()
            .Reverse()
            .SelectMany(c => c.Buffs)
            .WhereVersion(this.Context.LastVersion)
            .Where(buff => buff.IsAllowed(character.Level, character.Talents))
            .ToObservableCollection(buff => new BuffModel(this.Context, buff));

        var sharedBuffList = this.Context.DataSource
            .GetFullList<IBuff>()
            .WhereVersion(this.Context.LastVersion)
            .Where(buff => buff.IsShared())
            .ToObservableCollection(buff => new BuffModel(this.Context, buff));

        this.SharedPositiveBuffList = sharedBuffList.Where(buff => !buff.SelectedObject.IsNegate).ToObservableCollection();

        this.SharedNegativeBuffList = sharedBuffList.Where(buff => buff.SelectedObject.IsNegate).ToObservableCollection();

        this.CardBuffList = character.GetCardBuffs().ToObservableCollection(buff => new BuffModel(this.Context, buff));

        this.StampBuffList = character.GetStampBuffs().ToObservableCollection(buff => new BuffModel(this.Context, buff));

        this.Buffs = character.Buffs;
    }




    public IReadOnlyDictionary<IBuff, int> Buffs
    {
        get { return this.ClassBuffs.Concat(this.SharedBuffs).Concat(this.CardBuffs).Concat(this.StampBuffs); }
        private set
        {
            this.ClassBuffs = value.Where(pair => pair.Key.Class != null).ToDictionary();
            this.SharedBuffs = value.Where(pair => pair.Key.IsShared()).ToDictionary();
            this.CardBuffs = value.Where(pair => pair.Key.Card != null).ToDictionary();
            this.StampBuffs = value.Where(pair => pair.Key.Stamp != null).ToDictionary();
        }
    }




    public IReadOnlyDictionary<IBuff, int> ClassBuffs
    {
        get { return this.ClassBuffList.Where(model => model.Value != 0).ToDictionary(pair => pair.SelectedObject, pair => pair.Value); }
        private set
        {
            foreach (var model in this.ClassBuffList)
            {
                model.Value = value.GetMaybeValue(model.SelectedObject).GetValueOrDefault();
            }
        }
    }

    public ObservableCollection<BuffModel> ClassBuffList
    {
        get { return this.GetValue(v => v.ClassBuffList); }
        private set { this.SetValue(v => v.ClassBuffList, value); }
    }



    public IReadOnlyDictionary<IBuff, int> SharedBuffs
    {
        get { return this.SharedPositiveBuffList.Concat(this.SharedNegativeBuffList).Where(model => model.Value != 0).ToDictionary(pair => pair.SelectedObject, pair => pair.Value); }
        private set
        {
            foreach (var model in this.SharedPositiveBuffList.Concat(this.SharedNegativeBuffList))
            {
                model.Value = value.GetMaybeValue(model.SelectedObject).GetValueOrDefault();
            }
        }
    }

    public ObservableCollection<BuffModel> SharedPositiveBuffList
    {
        get { return this.GetValue(v => v.SharedPositiveBuffList); }
        private set { this.SetValue(v => v.SharedPositiveBuffList, value); }
    }

    public ObservableCollection<BuffModel> SharedNegativeBuffList
    {
        get { return this.GetValue(v => v.SharedNegativeBuffList); }
        private set { this.SetValue(v => v.SharedNegativeBuffList, value); }
    }


    public IReadOnlyDictionary<IBuff, int> CardBuffs
    {
        get { return this.CardBuffList.Where(model => model.Value != 0).ToDictionary(pair => pair.SelectedObject, pair => pair.Value); }
        private set
        {
            foreach (var model in this.CardBuffList)
            {
                model.Value = value.GetMaybeValue(model.SelectedObject).GetValueOrDefault();
            }
        }
    }

    public ObservableCollection<BuffModel> CardBuffList
    {
        get { return this.GetValue(v => v.CardBuffList); }
        private set { this.SetValue(v => v.CardBuffList, value); }
    }

    public IReadOnlyDictionary<IBuff, int> StampBuffs
    {
        get { return this.StampBuffList.Where(model => model.Value != 0).ToDictionary(pair => pair.SelectedObject, pair => pair.Value); }
        private set
        {
            foreach (var model in this.StampBuffList)
            {
                model.Value = value.GetMaybeValue(model.SelectedObject).GetValueOrDefault();
            }
        }
    }

    public ObservableCollection<BuffModel> StampBuffList
    {
        get { return this.GetValue(v => v.StampBuffList); }
        private set { this.SetValue(v => v.StampBuffList, value); }
    }

        
    public bool CloseDialog => false;

    public virtual void Clear()
    {
        this.Buffs = new Dictionary<IBuff, int>();
    }
}