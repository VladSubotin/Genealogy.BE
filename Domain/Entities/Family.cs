using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Family
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public byte[]? ProfileIcon { get; set; }

    public int? PrivacyLevel { get; set; }

    public virtual ICollection<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();

    public virtual ICollection<Person> People { get; set; } = new List<Person>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
