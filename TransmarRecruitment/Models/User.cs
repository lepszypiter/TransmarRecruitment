using System.ComponentModel.DataAnnotations;

namespace TransmarRecruitment.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public string? Name { get; set; }
    }
}
