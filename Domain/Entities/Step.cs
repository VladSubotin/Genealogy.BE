using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Step
{
    public Guid Id { get; set; }

    public Guid? TaskId { get; set; }

    public int? StepNum { get; set; }

    public string? Purpose { get; set; }

    public string? Source { get; set; }

    public string? SourceLocation { get; set; }

    public DateOnly? Term { get; set; }

    public string? Result { get; set; }

    public bool? IsDone { get; set; }

    public virtual Task? Task { get; set; }
}
