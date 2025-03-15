using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class PersonImage
{
    public Guid Id { get; set; }

    public Guid? PersonId { get; set; }

    public byte[]? Image { get; set; }

    public virtual Person? Person { get; set; }
}
