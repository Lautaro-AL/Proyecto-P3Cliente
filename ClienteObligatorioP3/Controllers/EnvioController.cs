using ClienteObligatorioP3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ClienteObligatorioP3.Controllers
{
    public class EnvioController : Controller
    {
        private HttpClient _httpClient;
        public EnvioController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://obligatoriowebapip3.azurewebsites.net/");
        }
        public IActionResult RastrearEnvio()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RastrearEnvioAsync(int numTrack)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/Envio/{numTrack}");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var envios = JsonSerializer.Deserialize<DTOEnvio>(result, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return View(envios);
                }
                else
                {
                    ViewBag.Mensaje = "Error";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EnviosUsuarioAsync()
        {

            string token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("/api/Envio/MisEnvios");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var listaenvios = JsonSerializer.Deserialize<List<DTOEnvio>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return View(listaenvios);
            }
            else
            {
                Console.WriteLine($" Error: {response.StatusCode} - {response.ReasonPhrase}");
                ViewBag.Error = "No se pudieron obtener los envíos. Intenta más tarde.";
                return View(new List<DTOEnvio>());
            }


        }

        public async Task<IActionResult> DetalleSeguimientoAsync(int numTracking, List<DTOEnvio> listaEnvios)
        {
            string token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Envio/{numTracking}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                var envio = JsonSerializer.Deserialize<DTOEnvio>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(envio.Seguimiento);
            }
            else
            {
                ViewBag.Error = "Error al obtener el detalle del envío. Intenta más tarde.";
                return View(new List<DTOSeguimiento>());
            }

        }

        [HttpGet]
        public IActionResult PorFechas()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PorFechas(DTOEnvioPorFechas dto)
        {
            string token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("/api/Envio/PorFechas", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var listaEnvios = JsonSerializer.Deserialize<List<DTOEnvio>>(result, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    ViewBag.Envios = listaEnvios;
                    return View(dto);
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    ViewBag.Error = errorMessage;
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Ocurrió un error inesperado. Intenta más tarde.";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> BuscarPorComentario(string palabra)
        {
            string token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.GetAsync($"/api/Envio/PorComentarios?palabra={palabra}");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var envios = JsonSerializer.Deserialize<List<DTOEnvio>>(result, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(envios);
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    ViewBag.Error = error;
                }
            }
            catch
            {
                ViewBag.Error = "Ocurrió un error inesperado. Intenta más tarde.";
                return View();

            }

            return View();
        }




    }
}