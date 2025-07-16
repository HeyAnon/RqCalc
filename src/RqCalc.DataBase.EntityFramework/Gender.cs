using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("Gender")]
public partial class Gender : ImageDirectoryBase
{

}

public partial class Gender : IGender
{
        
}