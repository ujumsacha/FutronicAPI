using System;
using System.Collections.Generic;

namespace ApiEnrolement.Models;

public partial class TEmpreinte
{
    public long RId { get; set; }

    public string RType { get; set; } = null!;

    public byte[]? RValeur { get; set; }

    public string? RLien { get; set; }

    public string? RIdPersonneFk { get; set; }

    public string RCreatedBy { get; set; } = null!;

    public string RCreatedOn { get; set; } = null!;

    public virtual TInfoPersonne? RIdPersonneFkNavigation { get; set; }
}
