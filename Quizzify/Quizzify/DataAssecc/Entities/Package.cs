using System;
using System.Collections.Generic;

namespace Quizzify.DataAssecc.Entities;

public class Package
{
    public int PackageId { get; set; }

    public string PackageName { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public int Difficulty { get; set; }

    public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();
}
