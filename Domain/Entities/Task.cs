using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Task
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateOnly? CreationDate { get; set; }

    public bool? IsDone { get; set; }

    public string? UserLogin { get; set; }

    public Guid? FamilyId { get; set; }

    public virtual Family? Family { get; set; }

    public virtual ICollection<Step> Steps { get; set; } = new List<Step>();

    public virtual User? UserLoginNavigation { get; set; }
}
