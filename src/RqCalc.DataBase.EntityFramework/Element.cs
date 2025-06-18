using System.ComponentModel.DataAnnotations.Schema;

using RqCalc.DataBase.EntityFramework._Base;
using RqCalc.Domain;

namespace RqCalc.DataBase.EntityFramework;

[Table("Element")]
public partial class Element : ImageDirectoryBase
{

}

public partial class Element : IElement
{
         
}