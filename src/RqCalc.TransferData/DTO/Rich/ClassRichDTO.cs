using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class ClassRichDTO : ClassSimpleDTO
    {
        [DataMember]
        public List<TalentBranchRichDTO> TalentBranches;

        [DataMember]
        public List<AuraRichDTO> Auras;

        [DataMember]
        public List<BuffRichDTO> Buffs;


        public ClassRichDTO(IClass @class, IMappingService mappingService)
            : base(@class, mappingService)
        {
            this.TalentBranches = @class.TalentBranches.ToList(tb => new TalentBranchRichDTO(tb, mappingService));

            this.Auras = @class.Auras.WhereVersion(mappingService.Context.LastVersion)
                                     .ToList(aura => new AuraRichDTO(aura, mappingService));

            this.Buffs = @class.Buffs.WhereVersion(mappingService.Context.LastVersion).ToList(buff => new BuffRichDTO(buff, mappingService));
        }
    }
}