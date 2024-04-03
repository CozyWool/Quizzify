using System.ComponentModel.DataAnnotations;

namespace Quizzify.Quester.Model.Package;
public class RoundModel
{
    public int RoundId { get; set; }

    [Required(ErrorMessage = "Название раунда обязательно")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Тип раунда обязателен")]
    public string RoundType { get; set; }

    public List<ThemeModel> Themes { get; set; }
}
