using ClienteObligatorioP3.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ClienteObligatorioP3.Controllers
{
    public class AuthController : Controller
    {
        private HttpClient _httpClient;
        public AuthController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://obligatoriowebapip3.azurewebsites.net/");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(DTOLogin login)
        {
            var json = JsonSerializer.Serialize(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Auth/login", content);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var tk = JsonSerializer.Deserialize<TokenResponse>(result, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                HttpContext.Session.SetString("Token", tk.Token);
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                ViewBag.Error = errorMessage;
                return View();
            }

            return RedirectToAction("Index", "Home"); ;

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult ModificarPass()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ModificarPassAsync(DTOModificarPassword dto)
        {
            string token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("/api/Password/Modificar", content);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Contraseña actualizada correctamente.";
                    return View();
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    ViewBag.Error = errorMessage;
                    return View(dto);
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Ocurrió un error inesperado. Intenta más tarde.";
                return View(dto);
            }
        }



    }
}

