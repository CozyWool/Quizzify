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

namespace Quizzify.Quester.ViewModel;

public class QuesterViewModel : INotifyPropertyChanged
{
    private List<PackageTreeViewModel> _packageTreeViews = [];
    private readonly IMapper _mapper;

    public ICommand SaveToFileSerializedCommand { get; }
    public ICommand UploadFileDeserializeCommand { get; }

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

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingPackage>());
        _mapper = config.CreateMapper();
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

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}