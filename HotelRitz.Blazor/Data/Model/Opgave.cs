namespace HotelRitz.Blazor.Data.Model;

using System;
using System.Collections.Generic;

public class Opgave
{
    
    public int Id { get; set; } 
    
    public string OpgaveTitel { get; set; } = string.Empty;
    public string OpgaveBeskrivelse { get; set; } = string.Empty;
    public string OpgaveOpretter { get; set; } = string.Empty;
    public DateTime OprettelsesTidspunkt { get; set; } = DateTime.Now;
    

    public string TildeltRolle { get; set; } = string.Empty; 
    public string TildeltMedarbejder { get; set; } = string.Empty; 
    
    public string Status { get; set; }

   
    public List<string> Modtagere { get; set; } = new();
}