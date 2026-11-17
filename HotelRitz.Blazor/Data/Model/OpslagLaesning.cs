namespace HotelRitz.Blazor.Data.Model;


public class OpslagLaesning
{
    public int Id { get; set; }
    public int OpslagId { get; set; }
    public string BrugerId { get; set; } = "";
    public DateTime LaesningsDato { get; set; }
}