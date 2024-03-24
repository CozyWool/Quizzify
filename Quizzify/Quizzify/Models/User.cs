using System;
using System.Collections.Generic;

namespace Quizzify.DataContext;

public class User
{
    public Guid UserId { get; set; }

    public string Login { get; set; }

    public string PasswordHash { get; set; }

    public string Email { get; set; }

    public int? SelectedSecretQuestionId { get; set; }

    public string SecretAnswerHash { get; set; }

    public string TwofaAuthMethod { get; set; }

    public byte[] GoogleAuthorization { get; set; }

    public virtual Player Player { get; set; }

    public virtual Secretquestion SelectedSecretQuestion { get; set; }
}
