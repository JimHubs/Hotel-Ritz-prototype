namespace HotelRitz.Blazor.Data.Model;

public class OpslagsTavle
{
    public int Id { get; set; }
    public string Titel { get; set; }
    public string Besked { get; set; }
    public string Forfatter { get; set; }
    public DateTime Oprettelsesdato { get; set; }
}