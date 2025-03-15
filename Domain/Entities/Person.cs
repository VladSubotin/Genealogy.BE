using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Person
{
    public Guid Id { get; set; }

    public string? Prefix { get; set; }

    public string? Suffix { get; set; }

    public string? Gender { get; set; }

    public string? Nationality { get; set; }

    public string? Religion { get; set; }

    public string? Biography { get; set; }

    public Guid? FamilyId { get; set; }

    public virtual ICollection<BiographicalFact> BiographicalFacts { get; set; } = new List<BiographicalFact>();

    public virtual Family? Family { get; set; }

    public virtual ICollection<PersonImage> PersonImages { get; set; } = new List<PersonImage>();

    public virtual ICollection<PersonName> PersonNames { get; set; } = new List<PersonName>();

    public virtual ICollection<Relation> RelationPersonIs { get; set; } = new List<Relation>();

    public virtual ICollection<Relation> RelationToPeople { get; set; } = new List<Relation>();
}
