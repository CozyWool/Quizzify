﻿namespace Quizzify.Quester.Model.Package;
public class ThemeModel
{
    public Guid ThemeId { get; set; }
    public string ThemeName { get; set; }
    public List<QuestionModel> Questions { get; set; }

    public ThemeModel()
    {
        Questions = new List<QuestionModel>();
    }
}