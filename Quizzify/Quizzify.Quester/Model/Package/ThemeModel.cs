namespace Quizzify.Quester.Model.Package;
public class ThemeModel
{
    public int ThemeId { get; set; }
    public string ThemeName { get; set; }
    public List<QuestionModel> Questions { get; set; }
}
