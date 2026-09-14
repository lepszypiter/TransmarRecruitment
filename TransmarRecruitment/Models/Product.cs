using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TransmarRecruitment.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        // Navigation
        public ICollection<AssemblyLine>? AssemblyLines { get; set; }
    }
}
