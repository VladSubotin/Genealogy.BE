using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class FamilyMember
{
    public Guid Id { get; set; }

    public string? UserLogin { get; set; }

    public Guid? FamilyId { get; set; }

    public string? Role { get; set; }

    public virtual Family? Family { get; set; }

    public virtual User? UserLoginNavigation { get; set; }
}
