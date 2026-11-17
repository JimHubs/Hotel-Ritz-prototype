using HotelRitz.Blazor.Data.Model;
using HotelRitz.Blazor.Data;
using System.Linq;
using System.Collections.Generic;
using System;

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

    public bool SletOpslag(int id)
    {
        var opslag = _context.OpslagsTavler.Find(id);
        if ((opslag == null))
            return false;

        _context.OpslagsTavler.Remove(opslag);
        _context.SaveChanges();
        return true;
    }

    public bool OpdaterOpslag(int id)
    {
        var opslag = _context.OpslagsTavler.Find(id);
        if (opslag == null)
            return false;

        opslag.ErFaerdiggjort = true;
        opslag.FaerdiggjortDato = DateTime.Now;

        _context.OpslagsTavler.Update(opslag);
        _context.SaveChanges();
        return true;
    }

    public bool MarkerSomLaest(int opslagId, string brugerId)
    {
        var opslag = _context.OpslagsTavler.Find(opslagId);
        if (opslag == null)
            return false;

        if (ErlaestAf(opslagId, brugerId))
            return false;
        
        {
            var laesning = new OpslagLaesning()
            {
                OpslagId = opslagId,
                BrugerId = brugerId,
                LaesningsDato = DateTime.Now
            };
            _context.Opslagslaesninger.Add(laesning);
            _context.SaveChanges();
            return true;
        }
    }
    
    public bool ErlaestAf(int opslagId, string brugerId)
    {
        return _context.Opslagslaesninger.Any(l => l.OpslagId == opslagId && l.BrugerId == brugerId);
    }
    
    public List<OpslagLaesning> HentLaesere(int opslagId)
    {
        return _context.Opslagslaesninger.Where(l => l.OpslagId == opslagId).ToList();
    }
}