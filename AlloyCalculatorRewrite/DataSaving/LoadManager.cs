using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace AlloyCalculatorRewrite.DataSaving
{
    public class LoadManager
    {
        private string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string saveFolderPath;

        public LoadManager()
        {
            saveFolderPath = Path.Combine(projectDirectory, "SaveFiles");
        }

        public Alloy GetFileData(string inFilePath)
        {
            Alloy alloy;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Alloy));
            using (FileStream fileStream = new FileStream(inFilePath, FileMode.Open))
            {
                alloy = (Alloy)xmlSerializer.Deserialize(fileStream);
            }
            return alloy;
        }
        public void CheckIfDirectoryExists()
        {
            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);
                MessageBox.Show("Could not find save folder. \nCreated new folder @" + saveFolderPath);
            }
        }

        public string GetSaveFolderDirectory()
        {
            return saveFolderPath;
        }
    }
}
