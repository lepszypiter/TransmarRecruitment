using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TransmarRecruitment.Models
{
    public class Workstation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ShortName { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public string? PcName { get; set; }

        public ICollection<AssemblyLineWorkstation>? Allocations { get; set; }
    }
}
