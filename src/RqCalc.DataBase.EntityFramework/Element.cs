using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Element")]
    public partial class Element : ImageDirectoryBase
    {

    }

    public partial class Element : IElement
    {
         
    }
}