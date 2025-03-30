using System.ComponentModel.DataAnnotations;

namespace BokningSystem.Models
{
    public class StaffMember
    {
        [Key]
        public string StaffMemberId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Namn")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Specialitet")]
        public string Position { get; set; } = string.Empty;
    }

}
