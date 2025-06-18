using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("Race")]
public partial class Race : DirectoryBase
{
    public bool IsPvP { get; set; }
}

public partial class Race : IRace
{
        
}