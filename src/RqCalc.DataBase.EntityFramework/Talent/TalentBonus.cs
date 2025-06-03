using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;
using Framework.Core;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("TalentBonus")]
    public partial class TalentBonus : PersistentDomainObjectBase
    {
        public virtual Talent Talent { get; set; }

        public virtual Aura AuraCondition { get; set; }

        public virtual Buff BuffCondition { get; set; }

        public virtual BonusType Type { get; set; }


        [Column("AuraCondition_Id")]
        public int? AuraConditionId { get; set; }

        [Column("BuffCondition_Id")]
        public int? BuffConditionId { get; set; }


        public decimal? SharedValue { get; set; }


        [Column("Talent_Id")]
        public int? TalentId { get; set; }
        

        public decimal Value { get; set; }



        [Column("Type_Id")]
        public virtual int TypeId { get; set; }


        public override string ToString()
        {
            return $"BonusType: {this.Type.Template} | Value: {this.Value}";
        }
    }

    public partial class TalentBonus : ITalentBonus
    {
        private readonly Lazy<List<decimal>> _lazyVariables;


        protected TalentBonus()
        {
            this._lazyVariables = LazyHelper.Create(() => new List<decimal> { this.Value });
        }

        ITalent ITalentBonus.Talent => this.Talent;

        IAura ITalentBonus.AuraCondition => this.AuraCondition;

        IBuff ITalentBonus.BuffCondition => this.BuffCondition;

        public IReadOnlyList<decimal> Variables => this._lazyVariables.Value;

        IBonusType Framework.Persistent.ITypeObject<IBonusType>.Type => this.Type;
    }
}