using Framework.Core;
using Framework.DataBase;
using Framework.HierarchicalExpand;
using Framework.Reactive;

using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain._Extensions;
using RqCalc.Model;
using RqCalc.Model._Extensions;
using RqCalc.Wpf.Models.Window.Dialog._Base;

namespace RqCalc.Wpf.Models.Window.Dialog;

public class BuffsWindowModel : NotifyModelBase, IClearModel
{
    public BuffsWindowModel(IDataSource<IPersistentDomainObjectBase> dataSource, IVersion lastVersion, ICharacterSource character)
    {
        this.ClassBuffList = character.Class.GetAllParents()
                                      .Reverse()
                                      .SelectMany(c => c.Buffs)
                                      .WhereVersion(lastVersion)
                                      .Where(buff => buff.IsAllowed(character.Level, character.Talents))
                                      .ToObservableCollection(buff => new BuffModel(buff));

        var sharedBuffList = dataSource
            .GetFullList<IBuff>()
            .WhereVersion(lastVersion)
            .Where(buff => buff.IsShared())
            .ToObservableCollection(buff => new BuffModel(buff));

        this.SharedPositiveBuffList = sharedBuffList.Where(buff => !buff.SelectedObject.IsNegate).ToObservableCollection();

        this.SharedNegativeBuffList = sharedBuffList.Where(buff => buff.SelectedObject.IsNegate).ToObservableCollection();

        this.CardBuffList = character.GetCardBuffs().ToObservableCollection(buff => new BuffModel(buff));

        this.StampBuffList = character.GetStampBuffs().ToObservableCollection(buff => new BuffModel(buff));

        this.Buffs = character.Buffs;
    }




    public IReadOnlyDictionary<IBuff, int> Buffs
    {
        get => this.ClassBuffs.Concat(this.SharedBuffs).Concat(this.CardBuffs).Concat(this.StampBuffs);
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
        get => this.ClassBuffList.Where(model => model.Value != 0).ToDictionary(pair => pair.SelectedObject, pair => pair.Value);
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
        get => this.GetValue(v => v.ClassBuffList);
        private set => this.SetValue(v => v.ClassBuffList, value);
    }



    public IReadOnlyDictionary<IBuff, int> SharedBuffs
    {
        get => this.SharedPositiveBuffList.Concat(this.SharedNegativeBuffList).Where(model => model.Value != 0).ToDictionary(pair => pair.SelectedObject, pair => pair.Value);
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
        get => this.GetValue(v => v.SharedPositiveBuffList);
        private set => this.SetValue(v => v.SharedPositiveBuffList, value);
    }

    public ObservableCollection<BuffModel> SharedNegativeBuffList
    {
        get => this.GetValue(v => v.SharedNegativeBuffList);
        private set => this.SetValue(v => v.SharedNegativeBuffList, value);
    }


    public IReadOnlyDictionary<IBuff, int> CardBuffs
    {
        get => this.CardBuffList.Where(model => model.Value != 0).ToDictionary(pair => pair.SelectedObject, pair => pair.Value);
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
        get => this.GetValue(v => v.CardBuffList);
        private set => this.SetValue(v => v.CardBuffList, value);
    }

    public IReadOnlyDictionary<IBuff, int> StampBuffs
    {
        get => this.StampBuffList.Where(model => model.Value != 0).ToDictionary(pair => pair.SelectedObject, pair => pair.Value);
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
        get => this.GetValue(v => v.StampBuffList);
        private set => this.SetValue(v => v.StampBuffList, value);
    }


    public bool CloseDialog => false;

    public virtual void Clear() => this.Buffs = new Dictionary<IBuff, int>();
}