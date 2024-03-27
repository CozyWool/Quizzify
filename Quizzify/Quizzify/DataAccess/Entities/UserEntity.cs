using System;
using System.Collections.Generic;

namespace Quizzify.DataAssecc.Entities;

public class UserEntity
{
    public Guid UserId { get; set; }

    public string Login { get; set; }

    public string PasswordHash { get; set; }

    public string Email { get; set; }

    public int? SelectedSecretQuestionId { get; set; }

    public string SecretAnswerHash { get; set; }

    public string TwofaAuthMethod { get; set; }

    public byte[] GoogleAuthorization { get; set; }

    public virtual PlayerEntity Player { get; set; }

    public virtual SecretQuestionEntity SelectedSecretQuestion { get; set; }
}
