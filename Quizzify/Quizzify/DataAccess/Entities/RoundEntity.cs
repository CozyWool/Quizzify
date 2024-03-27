using System;
using System.Collections.Generic;

namespace Quizzify.DataAssecc.Entities;

public class RoundEntity
{
    public int RoundId { get; set; }

    public int PackageId { get; set; }

    public string Name { get; set; }

    public string RoundType { get; set; }

    public virtual PackageEntity Package { get; set; }

    public virtual ICollection<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();
}
