using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class BonusSimpleDTO
    {
        [DataMember]
        public BonusTypeIdentityDTO Type;

        [DataMember]
        public List<decimal> Variables;


        public BonusSimpleDTO(IBonusBase bonus, IMappingService mappingService)
        {
            if (bonus == null) throw new ArgumentNullException(nameof(bonus));
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            this.Type = new BonusTypeIdentityDTO(bonus.Type);
            this.Variables = bonus.Variables.ToList();
        }
    }
}