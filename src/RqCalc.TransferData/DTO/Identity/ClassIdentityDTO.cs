using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public struct ClassIdentityDTO
    {
        [DataMember] 
        public int Id;

        
        public ClassIdentityDTO(int id)
        {
            this.Id = id;
        }

        public ClassIdentityDTO(IClass @class)
        {
            if (@class == null) throw new ArgumentNullException(nameof(@class));

            this.Id = @class.Id;
        }


        public IClass ToDomainObject(IMappingService mappingService)
        {
            return mappingService.GetById<IClass>(this.Id);
        }
    }
}