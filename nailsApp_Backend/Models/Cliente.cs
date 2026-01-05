using System.ComponentModel.DataAnnotations;

namespace nailsApp_Backend.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        
        [Required]
        public string Nome { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string CPF { get; set; }
        
        [Required]
        public string Status { get; set; }
        
        [Required]
        public DateTime DataNascimento { get; set; }
        
        public DateTime DataInclusao { get; set; } = DateTime.UtcNow;
        
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
    }
}