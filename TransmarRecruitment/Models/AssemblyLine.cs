using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TransmarRecruitment.Models
{
    public class AssemblyLine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public bool Active { get; set; } = true;

        // Foreign key to Product
        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        // Many-to-many via join entity to preserve order if needed
        public ICollection<AssemblyLineWorkstation>? Allocations { get; set; }
    }
}
