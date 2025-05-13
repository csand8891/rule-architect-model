using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuleArchitect.Entities
{
    [Table("ParameterMappings")]
    public class ParameterMapping
    {
        [Key]
        public int ParameterMappingId { get; set; }

        [MaxLength(255)]
        public string? RelatedSheetName { get; set; }

        [MaxLength(255)]
        public string? ConditionIdentifier { get; set; }

        [MaxLength(255)]
        public string? ConditionName { get; set; }

        [MaxLength(255)]
        public string? SettingContext { get; set; }

        public string? ConfigurationDetailsJson { get; set; }

        public int? SoftwareOptionId { get; set; }
        public virtual SoftwareOption? SoftwareOption { get; set; }
    }
}