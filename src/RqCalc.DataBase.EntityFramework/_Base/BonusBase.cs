using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Framework.Core;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    public abstract partial class BonusBase
    {
        public virtual BonusType Type { get; set; }


        public decimal Value { get; set; }

        
        
        [Column("Type_Id")]
        public virtual int TypeId { get; set; }


        public override string ToString()
        {
            return $"BonusType: {this.Type.Template} | Value: {this.Value}";
        }
    }

    public abstract partial class BonusBase : IBonusBase
    {
        private readonly Lazy<List<decimal>> _lazyVariables;


        protected BonusBase()
        {
            this._lazyVariables = LazyHelper.Create(() => new List<decimal> { this.Value });
        }


        public IReadOnlyList<decimal> Variables => this._lazyVariables.Value;


        IBonusType Framework.Persistent.ITypeObject<IBonusType>.Type => this.Type;
    }
}