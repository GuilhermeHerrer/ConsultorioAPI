using System.ComponentModel.DataAnnotations;
using ConsultorioAPI.Service;

namespace ConsultorioAPI.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required, EmailAddress(ErrorMessage = "O formato do E-mail está errado.")]
        public string Email { get; set; }
        [Required, ValidadorCpfServices]
        public string CPF { get; set; }
    }
}
