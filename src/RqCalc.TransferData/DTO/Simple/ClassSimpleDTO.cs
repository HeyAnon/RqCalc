using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class ClassSimpleDTO : DirectoryBaseDTO
    {
        [DataMember]
        public ClassIdentityDTO Parent;
        
        [DataMember]
        public bool AllowExtraWeapon;

        [DataMember]
        public StatIdentityDTO PrimaryStat;

        [DataMember]
        public StatIdentityDTO EnergyStat;
        
        [DataMember]
        public int MinLevel;

        [DataMember]
        public int MaxLevel;

        [DataMember]
        public int TalentCount;

        [DataMember]
        public bool IsMelee;


        public ClassSimpleDTO(IClass @class, IMappingService mappingService)
            : base (@class, mappingService)
        {
            this.Parent = @class.Parent.Maybe(v => new ClassIdentityDTO(v));
            this.AllowExtraWeapon = @class.AllowExtraWeapon;

            this.PrimaryStat = new StatIdentityDTO(@class.PrimaryStat);
            this.EnergyStat = new StatIdentityDTO(@class.EnergyStat);

            this.MinLevel = @class.GetMinLevel();
            this.MaxLevel = @class.Specialization.MaxLevel ?? mappingService.Context.LastVersion.MaxLevel;
            this.TalentCount = @class.GetSumBonusTalentCount();

            this.IsMelee = @class.IsMelee;
        }
    }
}