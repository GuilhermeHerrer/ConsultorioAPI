namespace ConsultorioAPI.Models
{
    public class ViaCepResponse
    {
        public string? Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public string Numero { get; set; }

        internal async Task AcharEnderecoAsync(string cep)
        {
            throw new NotImplementedException();
        }
    }
}
