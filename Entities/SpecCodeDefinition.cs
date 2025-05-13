using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuleArchitect.Entities
{
    [Table("SpecCodeDefinitions")]
    public class SpecCodeDefinition
    {
        public SpecCodeDefinition()
        {
            SoftwareOptionSpecificationCodes = new HashSet<SoftwareOptionSpecificationCode>();
            DependentRequirements = new HashSet<Requirement>();
        }

        [Key]
        public int SpecCodeDefinitionId { get; set; }

        [Required]
        [MaxLength(50)]
        // For composite unique index, see OnModelCreating or multiple IndexAttributes
        [Index("IX_SpecCodeNoBitMachineType", 1, IsUnique = true)]
        public string SpecCodeNo { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        [Index("IX_SpecCodeNoBitMachineType", 2, IsUnique = true)]
        public string SpecCodeBit { get; set; } = null!;

        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = null!;

        [Required]
        [Index("IX_SpecCodeNoBitMachineType", 3, IsUnique = true)]
        public int MachineTypeId { get; set; }
        public virtual MachineType MachineType { get; set; } = null!;

        public virtual ICollection<SoftwareOptionSpecificationCode> SoftwareOptionSpecificationCodes { get; set; }
        public virtual ICollection<Requirement> DependentRequirements { get; set; }
    }
}