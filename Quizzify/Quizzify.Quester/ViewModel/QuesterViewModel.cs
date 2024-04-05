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
using Quizzify.Quester.View;

namespace Quizzify.Quester.ViewModel;

public class QuesterViewModel : INotifyPropertyChanged
{
    private List<PackageTreeViewModel> _packageTreeViews = [];
    private readonly IMapper _mapper;

    private PackageModel package;
    private ObservableCollection<RoundModel> rounds= new ObservableCollection<RoundModel>();

    public ICommand SaveToFileSerializedCommand { get; set; }
    public ICommand UploadFileDeserializeCommand { get; }

    public ICommand NewPackageCommand { get; set; }
    public ICommand AddRoundCommand { get; set; }
    public ICommand AddThemeCommand { get; set; }
    public ICommand AddQuestionCommand { get; set; }

    private string _packageNameTextBox;
    public string PackageNameTextBox
    {
        get => _packageNameTextBox;
        set
        {
            _packageNameTextBox = value;
            OnPropertyChanged(nameof(_packageNameTextBox));
        }
    }

    private int _diffTextBox1;
    public int DiffTextBox1
    {
        get => _diffTextBox1;
        set
        {
            _diffTextBox1 = value;
            OnPropertyChanged(nameof(_diffTextBox1));
        }
    }

    private int _costQuestionTextBox;
    public int СostQuestionTextBox
    {
        get => _costQuestionTextBox;
        set
        {
            _costQuestionTextBox = value;
            OnPropertyChanged(nameof(_costQuestionTextBox));
        }
    }
    private string _questionTextBox;
    public string QuestionTextBox
    {
        get => _questionTextBox;
        set
        {
            _questionTextBox = value;
            OnPropertyChanged(nameof(_questionTextBox));
        }
    }

    private string _answerTextBox;
    public string AnswerTextBox
    {
        get => _answerTextBox;
        set
        {
            _answerTextBox = value;
            OnPropertyChanged(nameof(_answerTextBox));
        }
    }

    private ComboBox _roundComboBox;
    public ComboBox RoundComboBox
    {
        get => _roundComboBox;
        set
        {
            if (_themeComboBox == null) return;
            _roundComboBox = value;
            OnPropertyChanged(nameof(_roundComboBox));
        }
    }

    private ObservableCollection<string> _roundItems = new ObservableCollection<string>();
    public ObservableCollection<string> RoundItems
    {
        get => _roundItems;
        set
        {
            _roundItems = value;
            OnPropertyChanged(nameof(RoundItems));
        }
    }

    private string _selectedRound;
    public string SelectedRound
    {
        get => _selectedRound;
        set
        {
            _selectedRound = value;
            OnPropertyChanged(nameof(_selectedRound));
        }
    }

    private ComboBox _themeComboBox;
    public ComboBox ThemeComboBox
    {
        get => _themeComboBox;
        set
        {
            if (_themeComboBox == null) return;
            _themeComboBox = value;
            OnPropertyChanged(nameof(ThemeComboBox));
        }
    }

    private Dictionary<string, ThemeModel> _themeItems = new Dictionary<string, ThemeModel>();
    public Dictionary<string, ThemeModel> ThemeItems
    {
        get => _themeItems;
        set
        {
            _themeItems = value;
            OnPropertyChanged(nameof(_themeItems));
        }
    }
    private string _selectedTheme;
    public string SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            _selectedTheme = value;
            OnPropertyChanged(nameof(_selectedTheme));
        }
    }


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
        Random rand = new Random();

        package.PackageId = rand.Next(1,100000);
        package.PackageName = PackageNameTextBox;
        package.Difficulty = DiffTextBox1;
    }

    // TODO: Переписать. Добавление раунда
    private void AddRound(PackageModel model)
    {
        string roundName = Microsoft.VisualBasic.Interaction.InputBox("Введите название раунда", "Добавление раунда", "");

        Random rand = new Random();
        if (!string.IsNullOrEmpty(roundName))
        {
            if (package.PackageName != null)
            {
                var round = new RoundModel
                {
                    RoundId = rand.Next(1, 1000000),
                    RoundName = roundName
                };

                RoundItems.Add(round.RoundName);
                package.Rounds.Add(round);
            }
            else
            {
                MessageBox.Show("Пакета не существует");
            }
        }
    }


    private void AddTheme(PackageModel model)
    {
        string themeName = Microsoft.VisualBasic.Interaction.InputBox("Введите название темы", "Добавление темы", "");

        Random rand = new Random();//Костыль

        if (!string.IsNullOrEmpty(themeName))
        {
            var selectedRound = package.Rounds.FirstOrDefault(r => r.RoundName == RoundItems.FirstOrDefault().ToString());
            if (selectedRound != null)
            {
                if (!selectedRound.Themes.ContainsKey(themeName))
                {
                    if (package.PackageName != null)
                    {
                        var theme = new ThemeModel
                        {
                            ThemeId = rand.Next(1,1000000),
                            ThemeName = themeName,
                            Questions = new List<QuestionModel>()
                        };

                        selectedRound.Themes.Add(themeName, theme);
                        ThemeComboBox.Items.Add(themeName);
                    }
                    else
                    {
                        MessageBox.Show("Пакета не существует");
                    }
                }
                else
                {
                    MessageBox.Show("Такая тема уже существует в этом раунде");
                }
            }
            else
            {
                MessageBox.Show("Сначала нужно выбрать раунд");
            }
        }
    }

    private void AddQuestion(PackageModel model)
    {
        if (!string.IsNullOrEmpty(QuestionTextBox) && !string.IsNullOrEmpty(AnswerTextBox))
        {
            var selectedRound = package.Rounds.FirstOrDefault(r => r.RoundName == RoundComboBox.SelectedItem.ToString());

            if (selectedRound != null)
            {
                string? selectedThemeName = ThemeComboBox.SelectedItem as string;

                if (!string.IsNullOrEmpty(selectedThemeName) && selectedRound.Themes.ContainsKey(selectedThemeName))
                {
                    var selectedTheme = selectedRound.Themes[selectedThemeName];

                    if (selectedTheme != null)
                    {
                        var question = new QuestionModel
                        {
                            QuestionText = QuestionTextBox,
                            AnswerText = AnswerTextBox,
                            QuestionCost= СostQuestionTextBox
                        };

                        selectedTheme.Questions.Add(question);
                    }
                }
                else
                {
                    MessageBox.Show("Не выбрана тема");
                }
            }
        }
    }

    // TODO: Переписать. Очистка выпадающего списка при выборе раунда. Это событие повешенное на комбобокс с раундами.
    /*private void roundComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ThemeComboBox.Items.Clear();

        var selectedRound = package.Rounds.FirstOrDefault(r => r.RoundName == RoundComboBox.SelectedItem.ToString());

        if (selectedRound != null)
        {
            foreach (var themeName in selectedRound.Themes.Keys)
            {
                ThemeComboBox.Items.Add(themeName);
            }
        }
    }*/

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}