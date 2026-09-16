using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TransmarRecruitment.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = null!;

        // Optional: create assembly lines together with product
        public List<AssemblyLineDto>? AssemblyLines { get; set; }
    }

    public class UpdateProductDto
    {
        [Required]
        public string Name { get; set; } = null!;

        // Optional: replace assembly lines when provided
        public List<AssemblyLineDto>? AssemblyLines { get; set; }
    }
}
