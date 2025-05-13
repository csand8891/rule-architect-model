using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuleArchitect.Entities
{
    [Table("Requirements")]
    public class Requirement
    {
        [Key]
        public int RequirementId { get; set; }

        [Required]
        public int SoftwareOptionId { get; set; } // The SoftwareOption that HAS this requirement.
        public virtual SoftwareOption SoftwareOption { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string RequirementType { get; set; } = null!;

        [MaxLength(100)]
        public string? Condition { get; set; }

        public string? GeneralRequiredValue { get; set; }

        public int? RequiredSoftwareOptionId { get; set; } // ID of the SoftwareOption that IS required.
        public virtual SoftwareOption? RequiredSoftwareOption { get; set; }

        public int? RequiredSpecCodeDefinitionId { get; set; } // ID of the SpecCodeDefinition that IS required.
        public virtual SpecCodeDefinition? RequiredSpecCodeDefinition { get; set; }

        [MaxLength(255)]
        public string? OspFileName { get; set; }

        [MaxLength(50)]
        public string? OspFileVersion { get; set; }

        public string? Notes { get; set; }
    }
}