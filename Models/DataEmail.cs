using System.ComponentModel.DataAnnotations;

namespace SimpleEmailApp.Models
{
    public class DataEmail
    {
        [Key]
        public int IdEmail { get; set; }
        [Required]
        public string To { get; set; } 
        public string Subject { get; set; } 
        public string Body { get; set; } 
    } 
}
