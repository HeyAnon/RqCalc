using System;
using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class TextTemplateVariableSimpleDTO
    { 
        [DataMember]
        public decimal Value;
        
        [DataMember]
        public TextTemplateVariableType Type;


        public TextTemplateVariableSimpleDTO(ITextTemplateVariableBase textTemplateVariable, IMappingService mappingService)
        {
            if (textTemplateVariable == null) throw new ArgumentNullException(nameof(textTemplateVariable));
            if (mappingService == null) throw new ArgumentNullException(nameof(mappingService));

            this.Value = textTemplateVariable.Value;
            this.Type = textTemplateVariable.Type;
        }
    }
}