using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("Event")]
    public partial class Event : DirectoryBase
    {
        public virtual Version StartVersion { get; set; }

        public virtual Version EndVersion { get; set; }


        [Column("StartVersion_Id")]
        public int? StartVersionId { get; set; }

        [Column("EndVersion_Id")]
        public int? EndVersionId { get; set; }
    }

    public partial class Event : IEvent
    {
        IVersion IVersionObject.StartVersion => this.StartVersion;

        IVersion IVersionObject.EndVersion => this.EndVersion;
    }
}