namespace Quizzify.DataAccess.Entities;

public class ThemeEntity
{
    public int ThemeId { get; set; }
    public int RoundId { get; set; }
    public string ThemeName { get; set; }
    
    public virtual RoundEntity Round { get; set; }
    public virtual ICollection<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();
}