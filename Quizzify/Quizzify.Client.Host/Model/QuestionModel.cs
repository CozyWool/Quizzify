using System.ComponentModel.DataAnnotations;

namespace Quizzify.Client.Host.Model;

public class QuestionModel
{
    public int QuestionId { get; set; }
    public int ThemeId { get; set; }
    public string QuestionText { get; set; }
    public string QuestionTheme { get; set; }
    public byte[] QuestionImageUrl { get; set; }

    [Required(ErrorMessage = "Стоимость вопроса обязательна")]
    public int QuestionCost { get; set; }
    public string QuestionComment { get; set; }
    public string AnswerText { get; set; }
    public byte[] AnswerImageUrl { get; set; }
}
