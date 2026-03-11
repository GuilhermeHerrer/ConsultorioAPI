using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ConsultorioAPI.Service
{
    public class ValidadorCpfServices : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var cpf = value as string;
            
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return new ValidationResult("O CPF é obrigatório.");
            }

            cpf = Regex.Replace(cpf,"[^0-9]","");

            if (cpf.Length != 11)
            {
                return new ValidationResult("O CPF deve conter 11 dígitos.");
            }

            return ValidationResult.Success;
        }
    }
}
