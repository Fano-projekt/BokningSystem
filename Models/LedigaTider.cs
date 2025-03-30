using System;
using System.ComponentModel.DataAnnotations;

namespace BokningSystem.Models;

    public class LedigaTider
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Tid { get; set; }
    }

