using HotelRitz.Blazor.Data.Model;
using HotelRitz.Blazor.Data;
using System.Linq;
using System.Collections.Generic;

namespace HotelRitz.Blazor.Services;

public class OpslagService
{

    private readonly ApplicationDbContext _context;

    public OpslagService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<OpslagsTavle> HentAlleOpslag()
    {
        return _context.OpslagsTavler.ToList();
    }

    public void OpretOpslag(OpslagsTavle nytOpslag)
    {
        _context.OpslagsTavler.Add(nytOpslag);
        _context.SaveChanges();
    }
}