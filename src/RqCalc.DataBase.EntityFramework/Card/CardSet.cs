using System;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("CardSet")]
    public partial class CardSet : DirectoryBase
    {
    }

    public partial class CardSet : ICardSet
    {
    }
}