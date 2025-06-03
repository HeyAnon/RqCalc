using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

using Framework.Core;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Legacy_GuildBonus")]
    public partial class Legacy_GuildBonus : ImageDirectoryBase
    {
        public virtual BonusType Type { get; set; }


        public int Value { get; set; }

        public int MaxStackCount { get; set; }
        
        

        [Column("Type_Id")]
        public int? TypeId { get; set; }
    }

    public partial class Legacy_GuildBonus : ILegacy_GuildBonus
    {
        private readonly Lazy<List<decimal>> _lazyVariables;


        protected Legacy_GuildBonus()
        {
            this._lazyVariables = LazyHelper.Create(() => new List<decimal> { this.Value });
        }


        public IReadOnlyList<decimal> Variables => this._lazyVariables.Value;


        IBonusType Framework.Persistent.ITypeObject<IBonusType>.Type => this.Type;
    }
}