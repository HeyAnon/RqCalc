using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Anon.RQ_Calc.Domain;

namespace Anon.RQ_Calc.DataBase.EntityFramework
{
    [Table("CollectedItem")]
    public partial class CollectedItem : PersistentDomainObjectBase
    {
        public virtual ICollection<CollectedItemBonus> Bonuses { get; set; }

        public virtual Equipment Equipment { get; set; }

        public virtual Pet Pet { get; set; }

        public virtual Trophy Trophy { get; set; }

        public virtual CollectedGroup Group { get; set; }

        public virtual Version NativeStartVersion { get; set; }

        public virtual Version NativeEndVersion { get; set; }


        public virtual Version StartVersion => this.NativeStartVersion ?? this.Equipment?.StartVersion;

        public virtual Version EndVersion => this.NativeEndVersion ?? this.Equipment?.EndVersion;


        public Gender Gender => this.Equipment?.Gender;

        public Image Image => this.Equipment?.Image ?? this.Pet?.Image;

        public string Name => this.Equipment?.Name ?? this.Pet?.Name ?? this.Trophy?.Name;

        
        public int OrderIndex { get; set; }
        

        [Column("Equipment_Id")]
        public int? EquipmentId { get; set; }

        [Column("Pet_Id")]
        public int? PetId { get; set; }

        [Column("Trophy_Id")]
        public int? TrophyId { get; set; }

        [Column("Group_Id")]
        public int GroupId { get; set; }

        [Column("StartVersion_Id")]
        public int? NativeStartVersionId { get; set; }

        [Column("EndVersion_Id")]
        public int? NativeEndVersionId { get; set; }


        public override string ToString()
        {
            return this.Name;
        }
    }

    public partial class CollectedItem : ICollectedItem
    {
        IEnumerable<ICollectedItemBonus> IBonusContainer<ICollectedItemBonus>.Bonuses => this.Bonuses;

        IImage IImageObject.Image => this.Image;

        IGender ICollectedItem.Gender => this.Gender;

        ICollectedGroup ICollectedItem.Group => this.Group;

        IVersion IVersionObject.StartVersion => this.StartVersion;

        IVersion IVersionObject.EndVersion => this.EndVersion;
    }
}