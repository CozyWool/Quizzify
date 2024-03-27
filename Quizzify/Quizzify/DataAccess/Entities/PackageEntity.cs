using System;
using System.Collections.Generic;

namespace Quizzify.DataAssecc.Entities;

public class PackageEntity
{
    public int PackageId { get; set; }

    public string PackageName { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public int Difficulty { get; set; }

    public virtual ICollection<RoundEntity> Rounds { get; set; } = new List<RoundEntity>();
}
