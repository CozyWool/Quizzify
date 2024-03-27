using System;
using System.Collections.Generic;

namespace Quizzify.DataAssecc.Entities;

public class SecretQuestionEntity
{
    public int SecretQId { get; set; }

    public string QuestionText { get; set; }

    public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
