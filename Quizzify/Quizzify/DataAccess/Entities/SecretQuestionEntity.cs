using System;
using System.Collections.Generic;

namespace Quizzify.DataAssecc.Entities;

public class SecretQuestion
{
    public int SecretQId { get; set; }

    public string QuestionText { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
