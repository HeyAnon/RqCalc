using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Setting")]
    public partial class Setting 
    {
        [Key]
        public string Key { get; set; }

        public string Value { get; set; }
    }
    
    public partial class Setting : ISetting
    {

    }
}