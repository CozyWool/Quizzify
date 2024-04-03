using System.ComponentModel.DataAnnotations;

namespace Quizzify.Client.Host.Model;
public class RoundModel
{
    public int RoundId { get; set; }

    [Required(ErrorMessage = "Название раунда обязательно")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Тип раунда обязателен")]
    public string RoundType { get; set; }

    [Required(ErrorMessage = "Cписок тем обязателен")]
    public List<ThemeModel> Themes { get; set; }
}
