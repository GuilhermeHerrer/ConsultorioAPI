using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ConsultorioAPI.Models
{
    public class Medico
    {
       public int Id { get; set; }
       [Required]
       public string Nome { get; set; }
       [Required]
       public string Crm { get; set; }
       [Required]
       public int ConsultorioId { get; set; }
       [JsonIgnore]
       public Consultorio? Consultorio { get; set; }
    }
}
