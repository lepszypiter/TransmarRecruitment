using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TransmarRecruitment.DTOs
{
    public class CreateAssemblyLineDto
    {
        [Required]
        public string Name { get; set; } = null!;

        public bool Active { get; set; } = true;
    }

    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = null!;

        // Optional: create assembly lines together with product
        public List<CreateAssemblyLineDto>? AssemblyLines { get; set; }
    }
}
