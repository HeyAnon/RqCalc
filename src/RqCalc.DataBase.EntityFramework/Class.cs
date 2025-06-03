using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using Framework.Core;
using Framework.Persistent;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Class")]
    public partial class Class : ImageDirectoryBase
    {
        public virtual ICollection<Class> SubClasses { get; set; }

        public virtual ICollection<ClassBonus> NativeBonuses { get; set; }

        public virtual ICollection<ClassLevelHpBonus> LevelHpBonuses { get; set; }

        public virtual ICollection<TalentBranch> TalentBranches { get; set; }


        public virtual ICollection<Aura> Auras { get; set; }

        public virtual ICollection<Buff> Buffs { get; set; }


        public virtual Class Base { get; set; }

        public virtual Stat NativePrimaryStat { get; set; }

        public virtual Stat NativeEnergyStat { get; set; }

        public virtual ClassSpecialization Specialization { get; set; }



        [Column("IsMelee")]
        public bool? NativeIsMelee { get; set; }

        [Column("AllowExtraWeapon")]
        public bool? NativeAllowExtraWeapon { get; set; }

        [Column("HpPerVitality")]
        public decimal? NativeHpPerVitality { get; set; }


        [Column("Base_Id")]
        public int? BaseId { get; set; }

        [Column("PrimaryStat_Id")]
        public int? NativePrimaryStatId { get; set; }

        [Column("EnergyStat_Id")]
        public int? NativeEnergyStatId { get; set; }

        [Column("Specialization_Id")]
        public int? SpecializationId { get; set; }
    }


    public partial class Class : IClass
    {
        private readonly Lazy<bool> _lazyAllowExtraWeapon;
        
        private readonly Lazy<bool> _lazyIsMelee;

        private readonly Lazy<decimal> _lazyHpPerVitality;

        private readonly Lazy<Stat> _lazyPrimaryStat;

        private readonly Lazy<Stat> _lazyEnergyStat;

        private readonly Lazy<IReadOnlyList<ClassBonus>> _lazyBonuses;


        public Class()
        {
            this._lazyAllowExtraWeapon = LazyHelper.Create(() => this.GetFirst(c => c.NativeAllowExtraWeapon));

            this._lazyIsMelee = LazyHelper.Create(() => this.GetFirst(c => c.NativeIsMelee));

            this._lazyHpPerVitality = LazyHelper.Create(() => this.GetFirst(c => c.NativeHpPerVitality));

            this._lazyPrimaryStat = LazyHelper.Create(() => this.GetFirst(c => c.NativePrimaryStat));

            this._lazyEnergyStat = LazyHelper.Create(() => this.GetFirst(c => c.NativeEnergyStat));
            
            this._lazyBonuses = LazyHelper.Create(() => this.GetMany(c => c.NativeBonuses));
        }


        public bool AllowExtraWeapon => this._lazyAllowExtraWeapon.Value;

        public bool IsMelee => this._lazyIsMelee.Value;

        public decimal HpPerVitality => this._lazyHpPerVitality.Value;

        public IStat PrimaryStat => this._lazyPrimaryStat.Value;

        public IStat EnergyStat => this._lazyEnergyStat.Value;

        IEnumerable<IAura> IClass.Auras => this.Auras;

        IEnumerable<IBuff> IClass.Buffs => this.Buffs;


        public IEnumerable<IClassBonus> Bonuses => this._lazyBonuses.Value;


        IEnumerable<ITalentBranch> IClass.TalentBranches => this.TalentBranches;


        IEnumerable<IClass> IChildrenSource<IClass>.Children => this.SubClasses;

        IEnumerable<IClassLevelHpBonus> IClass.LevelHpBonuses => this.LevelHpBonuses;

        IClassSpecialization IClass.Specialization => this.Specialization;

        IClass IParentSource<IClass>.Parent => this.Base;


        private T GetFirst<T>(Func<Class, T> selector)
            where T : class
        {
            var request = from c in this.GetAllElements(v => v.Base)

                          let val = selector(c)

                          where val != null

                          select val;

            return request.First();
        }

        private T GetFirst<T>(Func<Class, T?> selector)
            where T : struct 
        {
            var request = from c in this.GetAllElements(v => v.Base)

                          let val = selector(c)

                          where val != null

                          select val.Value;

            return request.First();
        }

        private IReadOnlyList<T> GetMany<T>(Func<Class, IEnumerable<T>> selector, bool reverse = true)
        {
            var request = from c in this.GetAllElements(v => v.Base).Pipe(reverse, c => c.Reverse())

                          from val in selector(c)

                          select val;

            return request.ToList();
        }
    }
}