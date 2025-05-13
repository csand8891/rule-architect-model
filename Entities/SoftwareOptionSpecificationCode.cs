using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuleArchitect.Entities
{
    [Table("SoftwareOptionSpecificationCodes")]
    public class SoftwareOptionSpecificationCode
    {
        [Key]
        public int SoftwareOptionSpecificationCodeId { get; set; }

        public int SoftwareOptionId { get; set; }
        public virtual SoftwareOption SoftwareOption { get; set; } = null!;

        public int SpecCodeDefinitionId { get; set; }
        public virtual SpecCodeDefinition SpecCodeDefinition { get; set; } = null!;

        [MaxLength(255)]
        public string? ActivationRule { get; set; }

        public string? ValueInterpretation { get; set; }
    }
}