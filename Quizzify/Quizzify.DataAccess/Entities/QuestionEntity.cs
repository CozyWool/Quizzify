namespace Quizzify.DataAccess.Entities;

public class QuestionEntity
{
    public int QuestionId { get; set; }

    public int ThemeId { get; set; }

    public string QuestionText { get; set; }
    public byte[] QuestionImageUrl { get; set; }

    public int QuestionCost { get; set; }

    public string QuestionComment { get; set; }

    public string AnswerText { get; set; }

    public byte[] AnswerImageUrl { get; set; }

    public virtual ThemeEntity Theme { get; set; }
}