using HotelRitz.Blazor.Data.Model;

namespace HotelRitz.Blazor.Services;

public class OpslagService
{
    public List<OpslagsTavle> HentAlleOpslag()
    {
        return new List<OpslagsTavle>
        {
            new OpslagsTavle
            {
                Id = 1,
                Titel = "Arthur",
                Besked = "Hej fra arthur",
                Forfatter = "Arthur",
                Oprettelsesdato = DateTime.Now
            }
        };
    }
}