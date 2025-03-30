using System;
using System.ComponentModel.DataAnnotations;

namespace BokningSystem.Models
{
    public class Bokning
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Personnummer")]
        public string PatientId { get; set; }

        [Required]
        [Display(Name = "Namn")]
        public string PatientNamn { get; set; }

        [Required]
        [Display(Name = "Vem vill du träffa")]
        public string StaffMemberPosition { get; set; } 

        [Required]
        [Display(Name = "Önskad Vårdpersonal")]
        public string StaffMemberName { get; set; } 

        [Required]
        [Display(Name = "Anledning till besöket")]
        public string Anledning { get; set; }

        [Required]
        [Display(Name = "Tid för besöket")]
        public DateTime Tid { get; set; }
    }
}
