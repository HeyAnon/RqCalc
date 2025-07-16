using System.Runtime.Serialization;



using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class BuffBonusSimpleDTO
    {
        [DataMember]
        public string Template;

        [DataMember]
        public decimal PerStackValue;


        public BuffBonusSimpleDTO(IBonus buffBonus, IMappingService mappingService)
        {
            this.Template = buffBonus.PartialEvaluateTemplate();
            this.PerStackValue = buffBonus.Variables[0];
        }
    }
}