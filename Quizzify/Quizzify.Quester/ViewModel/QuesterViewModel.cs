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


namespace Quizzify.Quester.ViewModel;

public class QuesterViewModel
{
    private readonly IMapper _mapper;

    public ICommand SaveToFileSerializedCommand { get; }
    public ICommand UploadFileDeserializeCommand { get; }

    public QuesterViewModel()
    {
        SaveToFileSerializedCommand = new GenericCommand<PackageModel>(SaveToFile);
        UploadFileDeserializeCommand = new GenericCommand<PackageModel>(UploadFile);
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
}