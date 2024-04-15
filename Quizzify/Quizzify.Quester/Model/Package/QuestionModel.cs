namespace Quizzify.Quester.Model.Package;

public class QuestionModel
{
    public Guid QuestionId { get; set; }
    public string QuestionText { get; set; }
    public byte[] QuestionImageUrl { get; set; }
    public int QuestionCost { get; set; }
    public string QuestionComment { get; set; }
    public string AnswerText { get; set; }
    public byte[] AnswerImageUrl { get; set; }
}