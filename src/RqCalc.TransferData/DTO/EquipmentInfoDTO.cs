using System.Runtime.Serialization;

namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class EquipmentInfoDTO
    {
        [DataMember]
        public int Defense;

        [DataMember] 
        public int Attack;

        [DataMember] 
        public decimal AttackSpeed;


        public EquipmentInfoDTO(EquipmentBaseInfo baseInfo)
        {
            baseInfo.Match(
                info =>
                {
                    this.Defense = info.Defense;
                }, info =>
                {
                    this.Attack = info.Attack;
                    this.AttackSpeed = info.AttackSpeed;
                }, info =>
                {
                    this.Attack = info.Attack;
                    this.Defense = info.Defense;
                    this.AttackSpeed = info.AttackSpeed;
                });
        }
    }
}