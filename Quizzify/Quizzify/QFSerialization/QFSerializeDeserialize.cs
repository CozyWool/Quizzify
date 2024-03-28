using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SerializeTest
{
    public class QFSerializeDeserialize
    {

        public async Task QFSerialize(PackEntity _packEntity)
        {
            try
            {

            string jsonSerialized = JsonConvert.SerializeObject(_packEntity).ToString();
            var saveFile = new SaveFileDialog();
            if (saveFile.ShowDialog() == true)
            {
                await File.WriteAllTextAsync(saveFile.FileName + ".txt", jsonSerialized, Encoding.UTF8);
            }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task QFDeserialize(PackEntity _packEntity, ObservableCollection<PackEntity> _packs)
        {
            var openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                try
                {
                    var jsonData = await File.ReadAllTextAsync(openFile.FileName);
                    var jsonDeserialize = JsonConvert.DeserializeObject<PackEntity>(jsonData);
                    _packEntity = jsonDeserialize;
                    _packs.Add(_packEntity);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
