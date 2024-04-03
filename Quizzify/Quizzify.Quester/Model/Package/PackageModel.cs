namespace Quizzify.Quester.Model.Package;
public class PackageModel
{
    public int PackageId { get; set; }
    public string PackageName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Difficulty { get; set; }
    public List<RoundModel> Rounds { get; set; }
}