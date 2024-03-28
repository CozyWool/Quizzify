using System.ComponentModel.DataAnnotations;

namespace Quizzify.Client.Model.Users;
public class RegistrationModel
{
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Логин обязателен")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Пароль обязателен")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Электронная почта обязательна")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Выбор секретного вопроса обязателен")]
    public int SelectedSecretQuestionId { get; set; }

    [Required(ErrorMessage = "Ответ на секретный вопрос обязателен")]
    public string SecretAnswer { get; set; }
}