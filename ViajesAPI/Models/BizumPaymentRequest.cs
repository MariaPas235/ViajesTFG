/// <summary>
/// Modelo que representa una solicitud de pago mediante Bizum.
/// Contiene toda la informaci�n necesaria para procesar el pago.
/// </summary>
public class BizumPaymentRequest
{
    /// <summary>
    /// Datos cifrados o relevantes para la transacci�n.
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    /// Identificador o n�mero del pedido asociado al pago.
    /// </summary>
    public string Order { get; set; }

    /// <summary>
    /// Importe a pagar, usualmente como string para preservar formato.
    /// </summary>
    public string Importe { get; set; }

    /// <summary>
    /// URL de redirecci�n en caso de pago exitoso.
    /// </summary>
    public string UrlPageOk { get; set; }

    /// <summary>
    /// URL de redirecci�n en caso de fallo en el pago.
    /// </summary>
    public string UrlPageKO { get; set; }

    /// <summary>
    /// URL para recibir notificaciones desde Redsys sobre el estado del pago.
    /// </summary>
    public string RedsysNotificationApi { get; set; }

    /// <summary>
    /// C�digo del idioma para el TPV (Terminal Punto de Venta).
    /// </summary>
    public int LanguageTpv { get; set; }
}
