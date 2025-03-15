using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class User
{
    public string Login { get; set; } = null!;

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? HashPassword { get; set; }

    public byte[]? ProfileIcon { get; set; }

    public DateOnly? BirthDate { get; set; }
    public string? Description { get; set; }

    public virtual ICollection<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
