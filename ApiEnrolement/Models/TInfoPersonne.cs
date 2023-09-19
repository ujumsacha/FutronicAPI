using System;
using System.Collections.Generic;

namespace ApiEnrolement.Models;

public partial class TInfoPersonne
{
    public string RId { get; set; } = null!;

    public string? RNumCni { get; set; }

    public string? RNumUnique { get; set; }

    public string RNom { get; set; } = null!;

    public string RPrenom { get; set; } = null!;

    public DateTime[]? RDateNaissance { get; set; }

    public string? RSexe { get; set; }

    public int? RTaille { get; set; }

    public string? RNationnalite { get; set; }

    public string? RLieuDeNaissance { get; set; }

    public DateTime? RDateDExpiration { get; set; }

    public string? RNni { get; set; }

    public string? RProfession { get; set; }

    public DateTime? RDateEmission { get; set; }

    public string? RLieuEmission { get; set; }

    public string? RCreatedBy { get; set; }

    public DateTime? RCreatedOn { get; set; }

    public string? RUpdatedBy { get; set; }

    public DateTime? RUpdatedOn { get; set; }

    public bool RIsLock { get; set; }

    public string? RDescriptionLock { get; set; }

    public virtual ICollection<TEmpreinte> TEmpreintes { get; set; } = new List<TEmpreinte>();
}
