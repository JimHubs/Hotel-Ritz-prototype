using HotelRitz.Blazor.Data;
using HotelRitz.Blazor.Data.Model;
using HotelRitz.Blazor.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
namespace HotelRitz.Tests;

public class OpslagstavleTest
{

    private OpslagsTavle _opslagsTavle;

    public OpslagstavleTest()
    {
        // arrange før alle tests
        _opslagsTavle = new OpslagsTavle
        {
            Id = 1,
            Titel = "Standard titel",
            Besked = "Standard besked",
            Forfatter = "System",
            Oprettelsesdato = DateTime.Now
        };
    }

    [Fact]
    public void Opslag_titel_titelKanÆndres()
    {
        // Act
        _opslagsTavle.Titel = "Vigtig besked";

        // Assert
        Assert.Equal("Vigtig besked", _opslagsTavle.Titel);
    }

    [Fact]
    public void Opslag_besked_beskedKanÆndres()
    {
        //act
        _opslagsTavle.Besked = "vigtig besked";

        //assert
        Assert.Equal("vigtig besked", _opslagsTavle.Besked);
    }

    [Fact]
    public void Opslag_forfatter_forfatterKanÆndres()
    {
        _opslagsTavle.Forfatter = "forfatter";

        //assert

        Assert.Equal("forfatter", _opslagsTavle.Forfatter);
    }

    [Fact]
    public void Oplag_oprettelse_oprettelseKanÆndres()
    {
        //arrange

        var fastlaastDato = new DateTime(2026, 11, 11);

        //act

        _opslagsTavle.Oprettelsesdato = fastlaastDato;
        //assert

        Assert.Equal(fastlaastDato, _opslagsTavle.Oprettelsesdato);
    }


   

    [Fact]
    public void HentAlleOpslag_SkalReturnereEnListeAfOpslag()
    {
        // arrange. bruger de gamle test med her med den service og db
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite("DataSource=MinTestDatabase.db")
            .Options;
        
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureDeleted(); 
        context.Database.EnsureCreated(); 
    
        // opslag
        context.OpslagsTavler.Add(new OpslagsTavle 
        { 
            Id = 1, 
            Titel = "Arthur", 
            Besked = "Hej fra arthur", 
            Forfatter = "Arthur", 
            Oprettelsesdato = DateTime.Now 
        });
        context.SaveChanges();

        //act
        var service = new OpslagService(context);
        var resultat = service.HentAlleOpslag();

        // assert
        Assert.NotNull(resultat);
        Assert.Single(resultat); 
    }
}