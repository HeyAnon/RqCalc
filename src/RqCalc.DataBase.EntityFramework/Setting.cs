using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("Setting")]
public partial class Setting 
{
    [Key]
    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;
}
    
public partial class Setting : ISetting
{

}