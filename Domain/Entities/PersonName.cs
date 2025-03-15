using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class PersonName
{
    public Guid Id { get; set; }

    public Guid? PersonId { get; set; }

    public string? NameType { get; set; }

    public string? Name { get; set; }

    public bool? IsMain { get; set; }

    public virtual Person? Person { get; set; }
}
