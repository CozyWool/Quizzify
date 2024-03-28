using System.IO;
using Quizzify.Quester.Model.Package;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;
using Newtonsoft.Json;


namespace Quizzify.Quester.ViewModel;

public class QuesterViewModel
{
    public ICommand SaveToFileSerializedCommand { get; }
    public ICommand UploadTpFileSerializedCommand { get; }

    public QuesterViewModel()
    {
        SaveToFileSerializedCommand = new GenericCommand<PackageModel>(SaveToFile);
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
}