using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuleArchitect.Entities
{
    [Table("ControlSystems")]
    public class ControlSystem
    {
        public ControlSystem()
        {
            SoftwareOptions = new HashSet<SoftwareOption>();
        }

        [Key]
        public int ControlSystemId { get; set; }

        [Required]
        [MaxLength(100)]
        [Index("IX_ControlSystemName", IsUnique = true)] // EF6 way to specify unique index via attribute
        public string Name { get; set; } = null!;

        public virtual ICollection<SoftwareOption> SoftwareOptions { get; set; }
    }
}