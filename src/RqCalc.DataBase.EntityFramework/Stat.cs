using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using Framework.Core;
using Framework.Persistent;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Stat")]
    public partial class Stat : ImageDirectoryBase
    {
        public virtual ICollection<StatSource> NativeSources { get; set; }

        public virtual ICollection<StatBonus> Bonuses { get; set; }

        public virtual ICollection<Stat> Children { get; set; }


        public virtual Race Race { get; set; }

        public virtual Element Element { get; set; }

        public virtual Stat Parent { get; set; }

        public virtual Formula DescriptionFormula { get; set; }


        public virtual StatType Type { get; set; }


        public bool? IsMelee { get; set; }

        public int? RoundDigits { get; set; }

        public bool IsEditable { get; set; }

        public decimal DefaultValue { get; set; }

        public bool IsPercent { get; set; }

        public string ProgressName { get; set; }

        public int OrderIndex { get; set; }

        public string DescriptionTemplate { get; set; }

        [Column("DescriptionFormula_Id")]
        public int? DescriptionFormulaId { get; set; }


        [Column("Race_Id")]
        public int? RaceId { get; set; }

        [Column("Element_Id")]
        public int? ElementId { get; set; }

        [Column("Parent_Id")]
        public int? ParentId { get; set; }
    }

    public partial class Stat : IStat
    {
        private readonly Lazy<Dictionary<RestoreStatType, IStat>> _lazyRestoreStats;

        private readonly Lazy<Formula[]> _lazySources;


        public Stat()
        {
            this._lazyRestoreStats = LazyHelper.Create(() => this.Children.Where(c => c.Type < 0).ToDictionary(stat => (RestoreStatType)stat.Type, stat => (IStat)stat));

            this._lazySources = LazyHelper.Create(() => this.NativeSources.Where(s => s.Formula.Enabled).ToArray(s => s.Formula));
        }




        public Dictionary<RestoreStatType, IStat> RestoreStats => this._lazyRestoreStats.Value;

        public IEnumerable<IFormula> Sources => this._lazySources.Value;


        IEnumerable<IStatBonus> IBonusContainer<IStatBonus>.Bonuses => this.Bonuses;

        IRace IStat.Race => this.Race;

        IElement IStat.Element => this.Element;

        IStat IParentSource<IStat>.Parent => this.Parent;


        IReadOnlyDictionary<RestoreStatType, IStat> IStat.RestoreStats => this.RestoreStats;

        IFormula IStat.DescriptionFormula => this.DescriptionFormula;
    }
}