using HotelRitz.Blazor.Data;
using HotelRitz.Blazor.Data.Model;
using HotelRitz.Blazor.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HotelRitz.Tests;


public class OpgaveTest
{
    private Opgave _opgave;

    public OpgaveTest()
    {
        _opgave = new Opgave()
        {
            Id = 1,
            OpgaveOpretter = "",
            OpgaveTitel = "",
            OpgaveBeskrivelse = "",
            Status = "",
            TildeltRolle = "",
            TildeltMedarbejder = "",
        };
    }
    



}