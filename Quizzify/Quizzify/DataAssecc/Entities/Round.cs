using System;
using System.Collections.Generic;

namespace Quizzify.DataAssecc.Entities;

public class Round
{
    public int RoundId { get; set; }

    public int PackageId { get; set; }

    public string Name { get; set; }

    public string RoundType { get; set; }

    public virtual Package Package { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
