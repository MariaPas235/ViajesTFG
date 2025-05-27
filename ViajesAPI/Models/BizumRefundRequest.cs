/// <summary>
/// Modelo que representa una solicitud de devolución (refund) mediante Bizum.
/// Contiene la información necesaria para procesar la devolución.
/// </summary>
public class BizumRefundRequest
{
    /// <summary>
    /// Datos cifrados o relevantes para la transacción.
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    /// Identificador o número del pedido asociado a la devolución.
    /// </summary>
    public string Order { get; set; }

    /// <summary>
    /// Importe a devolver, en céntimos (ejemplo: 100 = 1 euro).
    /// </summary>
    public string Importe { get; set; }

    /// <summary>
    /// Código de idioma para el TPV (Terminal Punto de Venta), expresado como string (ejemplo: "1").
    /// </summary>
    public string LanguageTpv { get; set; }

    /// <summary>
    /// URL de redirección en caso de devolución exitosa.
    /// </summary>
    public string UrlPageOk { get; set; }

    /// <summary>
    /// URL de redirección en caso de fallo en la devolución.
    /// </summary>
    public string UrlPageKO { get; set; }

    /// <summary>
    /// Identificador de la operación Bizum original para la que se solicita la devolución.
    /// </summary>
    public string BizumIdOper { get; set; }
}
