using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Version
{
    public Guid Id { get; set; }

    public Guid? BiographicalFactId { get; set; }

    public string? Role { get; set; }

    public string? Place { get; set; }

    public string? Location { get; set; }

    public DateOnly? DateFrom { get; set; }

    public DateOnly? DateTo { get; set; }

    public string? Note { get; set; }

    public string? Veracity { get; set; }

    public string? Source { get; set; }

    public virtual BiographicalFact? BiographicalFact { get; set; }
}
