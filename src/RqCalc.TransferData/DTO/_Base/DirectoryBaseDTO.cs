using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public abstract class DirectoryBaseDTO
    {
        [DataMember]
        public int Id;

        [DataMember]
        public string Name;


        protected DirectoryBaseDTO(IDirectoryBase directoryBase, IMappingService mappingService)
        {
            if (directoryBase == null) throw new ArgumentNullException(nameof(directoryBase));
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            this.Id = directoryBase.Id;
            this.Name = directoryBase.Name;
        }
    }
}