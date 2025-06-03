using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    public abstract partial class VersionObject
    {
        public virtual Version StartVersion { get; set; }

        public virtual Version EndVersion { get; set; }


        [Column("StartVersion_Id")]
        public int? StartVersionId { get; set; }

        [Column("EndVersion_Id")]
        public int? EndVersionId { get; set; }
    }
    

    public abstract partial class VersionObject : IVersionObject
    {
        IVersion IVersionObject.StartVersion => this.StartVersion;

        IVersion IVersionObject.EndVersion => this.EndVersion;
    }
}