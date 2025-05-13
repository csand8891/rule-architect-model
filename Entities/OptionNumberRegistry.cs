using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuleArchitect.Entities
{
    [Table("OptionNumberRegistries")]
    public class OptionNumberRegistry
    {
        [Key]
        public int OptionNumberRegistryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string OptionNumber { get; set; } = null!;

        public int SoftwareOptionId { get; set; }
        public virtual SoftwareOption SoftwareOption { get; set; } = null!;
    }
}