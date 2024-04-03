namespace Quizzify.Quester.Model.Package;
public class RoundModel
{
    public int RoundId { get; set; }
    public string RoundName { get; set; }
    public string RoundType { get; set; }
    public List<ThemeModel> Themes { get; set; }
}