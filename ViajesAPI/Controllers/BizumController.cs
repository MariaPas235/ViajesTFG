using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ViajesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public PaymentsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpPost("bizum")]
        public async Task<IActionResult> PayWithBizum([FromBody] BizumPaymentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var receivedData = new
            {
                Data = request.Data,
                Order = request.Order,
                Importe = request.Importe,
                UrlPageOk = request.UrlPageOk,
                UrlPageKO = request.UrlPageKO,
                RedsysNotificationApi = request.RedsysNotificationApi,
                LanguageTpv = request.LanguageTpv
            };

            var jsonReceivedData = JsonSerializer.Serialize(receivedData, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("🟡 Datos recibidos desde el frontend:\n" + jsonReceivedData);

            var bizumApiUrl = "/api/v1/Bizum/InitBizumPayment";
            var fullUrl = "http://localhost:5000" + bizumApiUrl;

            var requestBody = JsonSerializer.Serialize(request);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // Generar el token HMAC
            var clientId = "178e124f-a127-49ec-aeeb-d8d1c576ddf8";
            var secretKey = "XjfpOdT+D9uYn40adDA7A0QOtsfT81PO+KEEfsLsqKc=";

            var token = HMACHelper.GenerateHmacToken("POST", bizumApiUrl, clientId, secretKey, requestBody);


            // Preparar la solicitud con token en la cabecera
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = content
            };
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("HMAC", token);

            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Mostrar respuesta en consola
                Console.WriteLine("✅ Respuesta recibida desde Bizum:\n" + responseContent);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new
                    {
                        message = "Datos recibidos y reenviados a Bizum exitosamente.",
                        receivedData = jsonReceivedData,
                        bizumResponse = responseContent
                    });
                }

                return StatusCode((int)response.StatusCode, new
                {
                    message = "Error al enviar a Bizum.",
                    receivedData = jsonReceivedData,
                    bizumResponse = responseContent
                });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("❌ Excepción HTTP al conectar con Bizum: " + ex.Message);

                return StatusCode(500, new
                {
                    message = "Excepción al conectar con Bizum.",
                    error = ex.Message,
                    receivedData = jsonReceivedData
                });
            }
        }
    }
}
