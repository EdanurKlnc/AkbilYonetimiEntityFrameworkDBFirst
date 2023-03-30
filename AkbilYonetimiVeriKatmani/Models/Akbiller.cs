using System;
using System.Collections.Generic;

namespace AkbilYonetimiVeriKatmani.Models;

public partial class Akbiller
{
    public string AkbilNo { get; set; } = null!;

    public string AkbilTipi { get; set; } = null!;

    public decimal Bakiye { get; set; }

    public DateTime EklenmeTarihi { get; set; }

    public int AkbilinSahibiId { get; set; }

    public DateTime? VizelendigiTarih { get; set; }

    //virtual ilişkiyi belirtir.
    public virtual Kullanicilar AkbilinSahibi { get; set; } = null!;

    public virtual ICollection<Talimatlar> Talimatlars { get; } = new List<Talimatlar>();
}
