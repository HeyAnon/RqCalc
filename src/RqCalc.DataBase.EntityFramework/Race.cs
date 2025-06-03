using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Race")]
    public partial class Race : DirectoryBase
    {
        public bool IsPvP { get; set; }
    }

    public partial class Race : IRace
    {
        
    }
}