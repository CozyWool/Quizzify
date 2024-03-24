using System;
using System.Collections.Generic;

namespace Quizzify.DataContext;

public class Secretquestion
{
    public int SecretQId { get; set; }

    public string QuestionText { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
