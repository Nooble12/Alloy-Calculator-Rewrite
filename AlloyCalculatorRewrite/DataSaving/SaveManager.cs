using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace AlloyCalculatorRewrite.DataSaving
{
    public class SaveManager
    {
        private string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string saveFolderPath;
        public SaveManager()
        {
            saveFolderPath = Path.Combine(projectDirectory, "SaveFiles");
            CheckIfDirectoryExists();
        }

        public void CheckIfDirectoryExists()
        {
            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);

                //Display Error Message
                MessageBox.Show("Could not find save folder. \nCreated new folder @" + saveFolderPath);
            }
        }
        public void Save(Alloy inAlloy, string filePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Alloy));
            using (FileStream fileStream = new FileStream(filePath.Trim(), FileMode.Create))
            {
                xmlSerializer.Serialize(fileStream, inAlloy);
            }
        }
        public string GetSaveFolderDirectory()
        {
            return saveFolderPath;
        }
    }
}
