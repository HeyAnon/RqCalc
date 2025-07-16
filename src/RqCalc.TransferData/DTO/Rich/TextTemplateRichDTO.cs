using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.Core;


using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class TextTemplateRichDTO : TextTemplateSimpleDTO
    {
        [DataMember] 
        public List<TextTemplateVariableSimpleDTO> Variables;


        public TextTemplateRichDTO(ITextTemplate textTemplate, IMappingService mappingService)
            : base(textTemplate, mappingService)
        {
            this.Variables = textTemplate.Variables.ToList(var => new TextTemplateVariableSimpleDTO(var, mappingService));
        }
    }
}