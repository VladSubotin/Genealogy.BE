using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Relation
{
    public Guid Id { get; set; }

    public Guid? PersonIsId { get; set; }

    public string? RelationType { get; set; }

    public Guid? ToPersonId { get; set; }

    public virtual Person? PersonIs { get; set; }

    public virtual Person? ToPerson { get; set; }
}
