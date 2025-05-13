using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuleArchitect.Entities
{
    [Table("MachineTypes")]
    public class MachineType
    {
        public MachineType()
        {
            SpecCodeDefinitions = new HashSet<SpecCodeDefinition>();
        }

        [Key]
        public int MachineTypeId { get; set; }

        [Required]
        [MaxLength(100)]
        [Index("IX_MachineTypeName", IsUnique = true)] // EF6 way to specify unique index via attribute
        public string Name { get; set; } = null!;

        public virtual ICollection<SpecCodeDefinition> SpecCodeDefinitions { get; set; }
    }
}