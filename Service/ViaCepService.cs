using ConsultorioAPI.Models;

namespace ConsultorioAPI.Service
{
    public class ViaCepService
    {
        private readonly HttpClient _httpClient;
        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("https://viacep.com.br/ws/");
        }

        public async Task<ViaCepResponse> AcharEnderecoAsync(string cep)
        {
            var endereco = await _httpClient.GetFromJsonAsync<ViaCepResponse>($"{cep}/json/");
            return endereco;
        }
    }
}
