using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class TextTemplateSimpleDTO
    {
        [DataMember]
        public string Message;

        [DataMember]
        public string Header;

        
        public TextTemplateSimpleDTO(ITextTemplate textTemplate, IMappingService mappingService)
        {
            if (textTemplate == null) throw new ArgumentNullException(nameof(textTemplate));
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            this.Header = textTemplate.Header;
            this.Message = textTemplate.Message;
        }
    }
}