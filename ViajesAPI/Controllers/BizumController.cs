using System;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace ViajesAPI.Controllers
{
    /// <summary>
    /// Controlador para manejar pagos y devoluciones utilizando Bizum.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor del controlador PaymentsController.
        /// </summary>
        /// <param name="httpClientFactory">Factoría para crear instancias de HttpClient.</param>
        public PaymentsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Endpoint para realizar un pago utilizando Bizum.
        /// </summary>
        /// <param name="request">Datos del pago en formato BizumPaymentRequest.</param>
        /// <returns>ActionResult que representa el resultado de la operación.</returns>
        [HttpPost("bizum")]
        public async Task<IActionResult> PayWithBizum([FromBody] BizumPaymentRequest request)
        {
            // Verifica que el modelo recibido sea válido
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // URL del endpoint de la API de Bizum para inicializar el pago
            var bizumApiUrl = "/api/v1/Bizum/InitBizumPayment";
            var fullUrl = "http://localhost:5000" + bizumApiUrl;

            // Serializa el objeto request a JSON para enviarlo en la solicitud
            var requestBody = JsonSerializer.Serialize(request);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // Credenciales necesarias para firmar la petición
            var clientId = "178e124f-a127-49ec-aeeb-d8d1c576ddf8";
            var secretKey = "XjfpOdT+D9uYn40adDA7A0QOtsfT81PO+KEEfsLsqKc=";

            // Genera el token HMAC para autenticación
            var token = HMACHelper.GenerateHmacToken("POST", bizumApiUrl, clientId, secretKey, requestBody);
            Console.WriteLine("Token HMAC generado: " + token);

            // Crea el mensaje HTTP con encabezado de autenticación HMAC
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = content
            };
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("HMAC", token);

            try
            {
                // Envía la solicitud al servidor de Bizum
                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Verifica si la respuesta fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Comprueba si la respuesta contiene un formulario HTML
                    if (responseContent.Contains("</form>"))
                    {
                        Console.WriteLine("🔥 HTML recibido de Bizum:");
                        Console.WriteLine(responseContent);

                        // Agrega un script para autoenviar el formulario recibido
                        var autoSubmitHtml = responseContent.Replace("</form>", "</form><script>document.forms[0].submit();</script>");
                        return Content(autoSubmitHtml, "text/html");
                    }
                    else
                    {
                        // Si no contiene formulario, se considera inválido
                        return BadRequest(new { message = "Respuesta de Bizum no contenía formulario HTML.", bizumResponse = responseContent });
                    }
                }

                // Devuelve error si el código de respuesta no fue exitoso
                return StatusCode((int)response.StatusCode, new
                {
                    message = "Error al enviar a Bizum.",
                    bizumResponse = responseContent
                });
            }
            catch (HttpRequestException ex)
            {
                // Manejo de excepciones de red o de conexión
                return StatusCode(500, new
                {
                    message = "Excepción al conectar con Bizum.",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Endpoint para realizar una devolución utilizando Bizum.
        /// </summary>
        /// <param name="refundRequest">Datos de la devolución en formato BizumRefundRequest.</param>
        /// <returns>ActionResult que representa el resultado de la operación.</returns>
        [HttpPost("bizumRefund")]
        public async Task<IActionResult> RefundBizum([FromBody] BizumRefundRequest refundRequest)
        {
            // Valida que el modelo sea correcto
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // URL del endpoint para devoluciones
                var refundApiUrl = "/api/v1/Bizum/RefundBizumPayment";
                var fullUrl = "http://localhost:5000" + refundApiUrl;

                // Se crea una nueva instancia para serializar solo lo necesario
                var bizumRefundDto = new BizumRefundRequest
                {
                    Data = refundRequest.Data,
                    Order = refundRequest.Order,
                    Importe = refundRequest.Importe,
                    LanguageTpv = refundRequest.LanguageTpv,
                    UrlPageOk = refundRequest.UrlPageOk,
                    UrlPageKO = refundRequest.UrlPageKO,
                    BizumIdOper = refundRequest.BizumIdOper,
                };

                // Serializa los datos de la devolución
                var requestBody = JsonSerializer.Serialize(bizumRefundDto);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                // Credenciales necesarias para la autenticación HMAC
                var clientId = "178e124f-a127-49ec-aeeb-d8d1c576ddf8";
                var secretKey = "XjfpOdT+D9uYn40adDA7A0QOtsfT81PO+KEEfsLsqKc=";

                // Genera el token HMAC para autenticación
                var token = HMACHelper.GenerateHmacToken("POST", refundApiUrl, clientId, secretKey, requestBody);
                Console.WriteLine("Token HMAC generado para refund: " + token);

                // Configura el mensaje HTTP con la cabecera de autorización
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, fullUrl)
                {
                    Content = content
                };
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("HMAC", token);

                // Envía la solicitud de devolución
                var response = await _httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Si la respuesta es exitosa, verifica si contiene formulario HTML
                if (response.IsSuccessStatusCode)
                {
                    if (responseContent.Contains("</form>"))
                    {
                        // Agrega script para autoenviar el formulario HTML
                        var autoSubmitHtml = responseContent.Replace("</form>", "</form><script>document.forms[0].submit();</script>");
                        return Content(autoSubmitHtml, "text/html");
                    }
                    else
                    {
                        // Si no contiene formulario HTML, se devuelve un error
                        return BadRequest(new { message = "Respuesta de Bizum no contenía formulario HTML.", bizumResponse = responseContent });
                    }
                }

                // Devuelve un código de estado personalizado si falló la petición
                return StatusCode((int)response.StatusCode, new
                {
                    message = "Error al enviar a Bizum.",
                    bizumResponse = responseContent
                });
            }
            catch (Exception ex)
            {
                // Captura excepciones generales durante la devolución
                Console.WriteLine("Excepción en RefundBizum: " + ex);
                return StatusCode(500, new
                {
                    message = "Excepción al procesar la devolución.",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}
