using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using Framework.Core;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("BonusType")]
    public partial class BonusType : PersistentDomainObjectBase
    {
        public virtual ICollection<BonusTypeVariable> Variables { get; set; }

        public virtual ICollection<BonusTypeStat> Stats { get; set; }


        public bool IsSingle { get; set; }

        public string Template { get; set; }
        

        public decimal? StampQualityMinCoef { get; set; }

        public decimal? StampQualityMaxCoef { get; set; }


        public override string ToString()
        {
            return this.Template;
        }
    }

    public partial class BonusType : IBonusType
    {
        private readonly Lazy<bool?> _lazyIsMultiply;

        public BonusType()
        {
            this._lazyIsMultiply = new Lazy<bool?>(() =>
                this.Stats.IsEmpty()               ? null
              : this.Stats.All(s => s.IsMultiply)  ? new bool?(true)
              : this.Stats.All(s => !s.IsMultiply) ? new bool?(false)
                                                   : null);
        }

        IEnumerable<IBonusTypeStat> IBonusType.Stats => this.Stats;

        IEnumerable<IBonusTypeVariable> IBonusType.Variables => this.Variables;


        public StampQualityInfo StampQuality => this.GetStampQuality();
        
        public bool? IsMultiply => this._lazyIsMultiply.Value;


        private StampQualityInfo GetStampQuality()
        {
            if (this.StampQualityMinCoef == null && this.StampQualityMaxCoef == null)
            {
                return null;
            }
            else if (this.StampQualityMinCoef != null && this.StampQualityMaxCoef != null)
            {
                return new StampQualityInfo(this.StampQualityMinCoef.Value, this.StampQualityMaxCoef.Value);
            }
            else
            {
                throw new Exception("Invalid StampQuality");
            }
        }
    }
}