using System.ComponentModel.DataAnnotations;

namespace Quizzify.Client.Host.Model;
public class ThemeModel
{
    public int ThemeId { get; set; }
    [Required(ErrorMessage = "Название темы обязателен")]
    public string ThemeName { get; set; }
    [Required(ErrorMessage = "Cписок вопросов обязателен")]
    public List<QuestionModel> Questions { get; set; }
}
