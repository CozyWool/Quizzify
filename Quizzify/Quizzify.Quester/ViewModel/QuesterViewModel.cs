using System.IO;
using Quizzify.Quester.Model.Package;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Windows;
using Quizzify.Quester.Model.Package.ViewModels;
using AutoMapper;
using Quizzify.Quester.Mappers;
using System.Windows.Controls;
using System.ComponentModel;

namespace Quizzify.Quester.ViewModel;

public class QuesterViewModel : INotifyPropertyChanged
{
    private List<PackageTreeViewModel> packageTreeViews = new List<PackageTreeViewModel>();
    private readonly IMapper _mapper;

    public ICommand SaveToFileSerializedCommand { get; }
    public ICommand UploadFileDeserializeCommand { get; }


    private TreeView treeView;

    public TreeView TreeView
    {
        get { return treeView; }
        set
        {
            if(treeView != null)
            {
                treeView = value;
                OnPropertyChanged(nameof(treeView));
            }
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
        string jsonSerialized = JsonConvert.SerializeObject(packageModel, Newtonsoft.Json.Formatting.Indented);
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
                    packageTreeViews.Add(packageTreeView);

                    TreeView.ItemsSource = packageTreeViews;
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

    // TODO:  Это дерево переедет во View Quster. Оно тут временно
    //    <TreeView x:Name="TreeView">
    //      <TreeView.ItemTemplate>
    //        <HierarchicalDataTemplate ItemsSource = "{Binding Rounds}" >
    //            < TextBlock Text="{Binding PackageName}"/>
    //            <HierarchicalDataTemplate.ItemTemplate>
    //                <HierarchicalDataTemplate ItemsSource = "{Binding Questions}" >
    //                    < TextBlock Text="{Binding RoundName}"/>
    //                    <HierarchicalDataTemplate.ItemTemplate>
    //                        <DataTemplate>
    //                            <TextBlock Text = "{Binding QuestionText}"/>
    //                        </ DataTemplate >
    //                    </ HierarchicalDataTemplate.ItemTemplate >
    //                </ HierarchicalDataTemplate >
    //            </ HierarchicalDataTemplate.ItemTemplate >
    //        </ HierarchicalDataTemplate >
    //    </ TreeView.ItemTemplate >
    //</ TreeView >

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}