using System;
using System.Runtime.Serialization;



namespace Anon.RQ_Calc.TransferData
{
    [DataContract]
    public class EquipmentUpgradeInfoDTO
    {
        [DataMember]
        public int Defense;

        [DataMember] 
        public int Hp;

        [DataMember] 
        public int Attack;

        [DataMember]
        public int AllStatBonus;


        public EquipmentUpgradeInfoDTO(EquipmentUpgradeBaseInfo baseInfo)
        {
            if (baseInfo == null) throw new ArgumentNullException(nameof(baseInfo));

            this.AllStatBonus = baseInfo.AllStatBonus;

            baseInfo.Match(info =>
                           {
                               this.Defense = info.Defense;
                               this.Hp = info.Hp;
                           }, info =>
                           {
                               this.Attack = info.Attack;
                           }, info =>
                           {
                               this.Attack = info.Attack;
                               this.Defense = info.Defense;
                           });
        }
    }
}