using System.ComponentModel.DataAnnotations;

namespace Quizzify.Model.Package;
public class PackageModel
{
    public int PackageId { get; set; }

    [Required(ErrorMessage = "Название пакета обязательно")]
    public string PackageName { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required(ErrorMessage = "Сложность пакета обязательна")]
    public int Difficulty { get; set; }

    public List<RoundModel> Rounds { get; set; }
}