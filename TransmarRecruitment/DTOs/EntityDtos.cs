using System.ComponentModel.DataAnnotations;

namespace TransmarRecruitment.DTOs
{
    public class AssemblyLineDto
    {
        [Required]
        public string Name { get; set; } = null!;

        public bool Active { get; set; } = true;

        public int? ProductId { get; set; }
    }

    public class AssemblyLineUpdateDto
    {
        [Required]
        public string Name { get; set; } = null!;

        public bool Active { get; set; } = true;

        public int? ProductId { get; set; }
    }

    public class CreateWorkstationDto
    {
        [Required]
        public string ShortName { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public string? PcName { get; set; }
    }

    public class UpdateWorkstationDto
    {
        [Required]
        public string ShortName { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public string? PcName { get; set; }
    }
}
