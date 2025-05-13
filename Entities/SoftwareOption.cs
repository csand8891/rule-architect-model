using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuleArchitect.Entities
{
    /// <summary>
    /// Represents a main software option or rule sheet.
    /// </summary>
    [Table("SoftwareOptions")] // Explicit table naming for clarity with EF6
    public class SoftwareOption
    {
        public SoftwareOption() // Constructor to initialize collections
        {
            RegisteredOptionNumbers = new HashSet<OptionNumberRegistry>();
            SpecificationCodes = new HashSet<SoftwareOptionSpecificationCode>();
            Requirements = new HashSet<Requirement>();
            DependentRequirements = new HashSet<Requirement>(); // Requirements where this SO is the one being required
            ParameterMappings = new HashSet<ParameterMapping>();
        }

        [Key]
        public int SoftwareOptionId { get; set; }

        [Required]
        [MaxLength(255)]
        public string PrimaryName { get; set; } = null!;

        [MaxLength(500)]
        public string? AlternativeNames { get; set; }

        [MaxLength(255)]
        public string? SourceFileName { get; set; }

        [MaxLength(100)]
        public string? PrimaryOptionNumberDisplay { get; set; }

        public string? Notes { get; set; }

        [MaxLength(100)]
        public string? CheckedBy { get; set; }

        public DateTime? CheckedDate { get; set; } // DateTime? is fine for nullable dates

        // Foreign Key for ControlSystem
        public int? ControlSystemId { get; set; } // Nullable FK
        public virtual ControlSystem? ControlSystem { get; set; }

        // Navigation Properties
        public virtual ICollection<OptionNumberRegistry> RegisteredOptionNumbers { get; set; }
        public virtual ICollection<SoftwareOptionSpecificationCode> SpecificationCodes { get; set; }
        public virtual ICollection<Requirement> Requirements { get; set; }
        public virtual ICollection<Requirement> DependentRequirements { get; set; }
        public virtual ICollection<ParameterMapping> ParameterMappings { get; set; }
    }
}