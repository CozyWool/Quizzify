namespace Quizzify.Quester.Model.Package;
public class RoundModel
{
    public Guid RoundId { get; set; }
    public string RoundName { get; set; }
    public string RoundType { get; set; }
    public Dictionary<string, ThemeModel> Themes { get; set; }

    public RoundModel()
    {
        Themes= new Dictionary<string, ThemeModel>();
    }
}