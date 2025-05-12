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

            var bizumApiUrl = "/api/v1/Bizum/InitBizumPayment";
            var fullUrl = "http://localhost:5000" + bizumApiUrl;

            var requestBody = JsonSerializer.Serialize(request);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var clientId = "178e124f-a127-49ec-aeeb-d8d1c576ddf8";
            var secretKey = "XjfpOdT+D9uYn40adDA7A0QOtsfT81PO+KEEfsLsqKc=";

            var token = HMACHelper.GenerateHmacToken("POST", bizumApiUrl, clientId, secretKey, requestBody);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = content
            };
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("HMAC", token);

            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    if (responseContent.Contains("</form>"))
                    {
                        var autoSubmitHtml = responseContent.Replace("</form>", "</form><script>document.forms[0].submit();</script>");
                        return Content(autoSubmitHtml, "text/html");
                    }
                    else
                    {
                        return BadRequest(new { message = "Respuesta de Bizum no contenía formulario HTML.", bizumResponse = responseContent });
                    }
                }

                return StatusCode((int)response.StatusCode, new
                {
                    message = "Error al enviar a Bizum.",
                    bizumResponse = responseContent
                });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Excepción al conectar con Bizum.",
                    error = ex.Message
                });
            }
        }
    }
}
