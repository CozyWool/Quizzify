using System.IO;
using Quizzify.Quester.Model.Package;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Windows;
using AutoMapper;
using Quizzify.Quester.Mappers;
using System.Windows.Controls;
using System.ComponentModel;
using Quizzify.Infrastructure.WPF.Command;
using Quizzify.Quester.Model.Package.TreeViewModels;
using System.IO.Packaging;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;

namespace Quizzify.Quester.ViewModel;

public class QuesterViewModel : INotifyPropertyChanged
{
    private List<PackageTreeViewModel> _packageTreeViews = [];
    private readonly IMapper _mapper;

    private PackageModel package;

    public ICommand SaveToFileSerializedCommand { get; }
    public ICommand UploadFileDeserializeCommand { get; }

    public ICommand NewPackageCommand { get; }
    public ICommand AddRoundCommand { get; }
    public ICommand AddThemeCommand { get; }
    public ICommand AddQuestionCommand { get; }


    private TreeView _treeView;
    public TreeView TreeView
    {
        get => _treeView;
        set
        {
            if (_treeView == null) return;
            
            _treeView = value;
            OnPropertyChanged(nameof(_treeView));
        }
    }

    public QuesterViewModel()
    {
        SaveToFileSerializedCommand = new GenericCommand<PackageModel>(async (model) => await SaveToFile(model));
        UploadFileDeserializeCommand = new GenericCommand<PackageModel>(async (model) => await UploadFile(model));

        NewPackageCommand = new GenericCommand<PackageModel>(NewPackage);
        AddRoundCommand = new GenericCommand<PackageModel>(AddRound);
        AddThemeCommand = new GenericCommand<PackageModel>(AddTheme);
        AddQuestionCommand = new GenericCommand<PackageModel>(AddQuestion);



        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingPackage>());
        _mapper = config.CreateMapper();

        package = new PackageModel();
        package.Rounds = new List<RoundModel>();
    }

    private async Task SaveToFile(PackageModel packageModel)
    {
        var jsonSerialized = JsonConvert.SerializeObject(packageModel, Formatting.Indented);
        var saveFile = new SaveFileDialog();
        if (saveFile.ShowDialog() == true)
        {
            await File.WriteAllTextAsync(saveFile.FileName, jsonSerialized, Encoding.UTF8);
        }
    }

    private async Task UploadFile(PackageModel packageModel)
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "JSON files (*.json)|*.json";

        if (openFileDialog.ShowDialog() == true)
        {
            string jsonData = await File.ReadAllTextAsync(openFileDialog.FileName);
            try
            {
                var package = JsonConvert.DeserializeObject<PackageModel>(jsonData);

                try
                {
                    var packageTreeView = _mapper.Map<PackageTreeViewModel>(package);
                    _packageTreeViews.Add(packageTreeView);

                    TreeView.ItemsSource = _packageTreeViews;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка преобразования пакета: {ex.Message}");
                }
            } 
            catch (JsonException ex)
            {
                MessageBox.Show($"Некорректный JSON: {ex.Message}") ;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}");
            }
        }
    }

    // TODO: Переписать. В самом начале нужно указать имя пакета и его сложность. Нажать "Сохранить"
    private void NewPackage(PackageModel model)
    {
        //package.PackageId = Guid.NewGuid();
        //package.PackageName = packageNameTextBox.Text;
    }

    // TODO: Переписать. Добавление раунда
    private void AddRound(PackageModel model)
    {
        //string roundName = Microsoft.VisualBasic.Interaction.InputBox("Введите название раунда", "Добавление раунда", "");

        //if (!string.IsNullOrEmpty(roundName))
        //{
        //    if (package.PackageName != null)
        //    {
        //        var round = new Round
        //        {
        //            RoundId = Guid.NewGuid(),
        //            RoundName = roundName
        //        };

        //        roundComboBox.Items.Add(round.RoundName);
        //        package.Rounds.Add(round);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Пакета не существует");
        //    }
        //}
    }

    // TODO: Переписать. Добавление темы к конкретному раунду
    private void AddTheme(PackageModel model)
    {
        //string themeName = Microsoft.VisualBasic.Interaction.InputBox("Введите название темы", "Добавление темы", "");

        //if (!string.IsNullOrEmpty(themeName))
        //{
        //    var selectedRound = package.Rounds.FirstOrDefault(r => r.RoundName == roundComboBox.SelectedItem.ToString());
        //    if (selectedRound != null)
        //    {
        //        if (!selectedRound.Themes.ContainsKey(themeName))
        //        {
        //            if (package.PackageName != null)
        //            {
        //                var theme = new Theme
        //                {
        //                    ThemeId = Guid.NewGuid(),
        //                    ThemeName = themeName,
        //                    Questions = new List<Question>()
        //                };

        //                selectedRound.Themes.Add(themeName, theme);
        //                topicComboBox.Items.Add(themeName);
        //            }
        //            else
        //            {
        //                MessageBox.Show("Пакета не существует");
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Такая тема уже существует в этом раунде");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Сначала нужно выбрать раунд");
        //    }
        //}
    }

    // TODO: Переписать. Добавление вопроса к конкрутной теме конкретного раунда
    private void AddQuestion(PackageModel model)
    {
        //string questionText = questionTextBox.Text;
        //string answerText = answerTextBox.Text;

        //if (!string.IsNullOrEmpty(questionText) && !string.IsNullOrEmpty(answerText))
        //{
        //    var selectedRound = package.Rounds.FirstOrDefault(r => r.RoundName == roundComboBox.SelectedItem.ToString());

        //    if (selectedRound != null)
        //    {
        //        string? selectedThemeName = topicComboBox.SelectedItem as string;

        //        if (!string.IsNullOrEmpty(selectedThemeName) && selectedRound.Themes.ContainsKey(selectedThemeName))
        //        {
        //            var selectedTheme = selectedRound.Themes[selectedThemeName];

        //            if (selectedTheme != null)
        //            {
        //                var question = new Question
        //                {
        //                    QuestionText = questionText,
        //                    AnswerText = answerText
        //                };

        //                selectedTheme.Questions.Add(question);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Не выбрана тема");
        //        }
        //    }
        //}
    }

    // TODO: Переписать. Очистка выпадающего списка при выборе раунда. Это событие повешенное на комбобокс с раундами.
    private void roundComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //topicComboBox.Items.Clear();

        //var selectedRound = package.Rounds.FirstOrDefault(r => r.RoundName == roundComboBox.SelectedItem.ToString());

        //if (selectedRound != null)
        //{
        //    foreach (var themeName in selectedRound.Themes.Keys)
        //    {
        //        topicComboBox.Items.Add(themeName);
        //    }
        //}
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}