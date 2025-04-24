namespace ViajesAPI.Models.DTO
{
    public class ResponseDTO
    {
        public Object ? Data {  get; set; }
        public bool IsSuccess { get; set; }=true;
        public String Message { get; set; } = "";
    }
}
