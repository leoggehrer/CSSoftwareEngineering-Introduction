using System.ComponentModel.DataAnnotations;

namespace Introduction.AspMvc.Models
{
    public class Student
    {
        private static int Counter = 0;
        public Student()
        {
            Id = ++Counter;
        }
        public int Id { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(10)]
        [Display(Name = "Nummer")]
        public string MatriculationNumber { get; set; } = string.Empty;
        [Required]
        [MinLength(1)]
        [Display(Name = "Vorname")]
        public string Firstname { get; set; } = string.Empty;
        [Required]
        [MinLength(1)]
        [Display(Name = "Nachname")]
        public string Lastname { get; set; } = string.Empty;
    }
}
