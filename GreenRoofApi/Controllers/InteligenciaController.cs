using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Dynamic;

namespace GreenRoofApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteligenciaController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string openWeatherApiKey = "84674b82ba66ef10c72996e0eb4ee152";
        private readonly string agroMonitoringApiKey = "613d1afb9e5bdef76ca5b04626254376";

        public InteligenciaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [HttpGet("sugestoes-cultivo")]
        public async Task<IActionResult> GetSugestoesCultivo(string cidade, double latitude, double longitude, string epocaAno)
        {
            var client = _httpClientFactory.CreateClient();

            // Solicitação de clima com base na cidade
            var climaResponse = await client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={cidade}&appid={openWeatherApiKey}&units=metric&lang=pt");
            var agroResponse = await client.GetAsync($"http://api.agromonitoring.com/agro/1.0/weather?lat={latitude}&lon={longitude}&appid={agroMonitoringApiKey}");

            if (!climaResponse.IsSuccessStatusCode || !agroResponse.IsSuccessStatusCode)
            {
                return StatusCode(500, "Erro ao obter dados de clima ou dados agrícolas.");
            }

            // Parseando os dados de clima e dados agrícolas como objetos dinâmicos
            var climaData = JsonConvert.DeserializeObject<ExpandoObject>(await climaResponse.Content.ReadAsStringAsync());
            dynamic agroData = JsonConvert.DeserializeObject<ExpandoObject>(await agroResponse.Content.ReadAsStringAsync());

            // Convertendo temperaturas de Kelvin para Celsius em dadosAgricolas
            if (agroData.main != null)
            {
                agroData.main.temp = ConvertKelvinToCelsius(agroData.main.temp);
                agroData.main.feels_like = ConvertKelvinToCelsius(agroData.main.feels_like);
                agroData.main.temp_min = ConvertKelvinToCelsius(agroData.main.temp_min);
                agroData.main.temp_max = ConvertKelvinToCelsius(agroData.main.temp_max);
            }

            // Exemplo de lógica usando epocaAno para sugestão de cultivo
            var sugestaoCultivo = ObterSugestaoCultivoPorEstacao(epocaAno);

            var resultado = new
            {
                Clima = climaData,
                DadosAgricolas = agroData,
                SugestaoCultivo = sugestaoCultivo
            };

            return Ok(resultado);
        }

        // Método para obter sugestões de cultivo com base na estação do ano
        private string ObterSugestaoCultivoPorEstacao(string epocaAno)
        {
            return epocaAno.ToLower() switch
            {
                "primavera" => "Cultivo sugerido: tomate, pepino, pimentão.",
                "verao" => "Cultivo sugerido: milho, melancia, abóbora.",
                "outono" => "Cultivo sugerido: cenoura, batata-doce, alface.",
                "inverno" => "Cultivo sugerido: espinafre, couve, brócolis.",
                _ => "Estação desconhecida. Cultivos padrão recomendados."
            };
        }

        // Função para converter de Kelvin para Celsius e arredondar o resultado
        private double ConvertKelvinToCelsius(double kelvinTemp)
        {
            return Math.Round(kelvinTemp - 273.15, 1);
        }
    }
}