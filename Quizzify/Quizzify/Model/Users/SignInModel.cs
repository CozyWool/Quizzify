using System.ComponentModel.DataAnnotations;

namespace Quizzify.Model.Users;

public class SignInModel
{
    [Required(ErrorMessage = "Логин или электронная почта обязательны")]
    public string LoginOrEmail { get; set; }

    [Required(ErrorMessage = "Пароль обязателен")]
    public string Password { get; set; }
}