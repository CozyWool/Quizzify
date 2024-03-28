using System.ComponentModel.DataAnnotations;

namespace Quizzify.Client.Model.Users;

public class AuthorizationModel
{
    [Required(ErrorMessage = "Логин или электронная почта обязательны")]
    public string LoginOrEmail { get; set; }

    [Required(ErrorMessage = "Пароль обязателен")]
    public string Password { get; set; }
}