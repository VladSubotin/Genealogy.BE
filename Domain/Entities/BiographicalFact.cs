using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class BiographicalFact
{
    public Guid Id { get; set; }

    public Guid? PersonId { get; set; }

    public string? FactType { get; set; }

    public virtual Person? Person { get; set; }

    public virtual ICollection<Version> Versions { get; set; } = new List<Version>();
}
