using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("State")]
    public partial class State : DirectoryBase
    {

    }

    public partial class State : IState
    {

    }
}