public class BizumRefundRequest
{
    public string Data { get; set; }
    public string Order { get; set; }
    public string Importe { get; set; }          // Aquí sí usas Amount (en céntimos)
    public string LanguageTpv { get; set; }        // String que indica idioma, por ejemplo "1"
    public string UrlPageOk { get; set; }
    public string UrlPageKO { get; set; }
    public string BizumIdOper { get; set; }     // ID de operación para devolución
}