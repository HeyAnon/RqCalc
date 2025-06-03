using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("EquipmentForge")]
    public partial class EquipmentForge
    {
        [Key]
        public int Level { get; set; }

        public decimal Attack { get; set; }
        
        public decimal Defense { get; set; }

        public int AllStatBonus { get; set; }
    }

    public partial class EquipmentForge : IEquipmentForge
    {
      
    }
}