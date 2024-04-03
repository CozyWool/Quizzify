namespace Quizzify.DataAccess.Entities;

public class RoundEntity
{
    public int RoundId { get; set; }

    public int PackageId { get; set; }

    public string Name { get; set; }

    public string RoundType { get; set; }

    public virtual PackageEntity Package { get; set; }

    public virtual ICollection<ThemeEntity> Themes { get; set; } = new List<ThemeEntity>();
}
