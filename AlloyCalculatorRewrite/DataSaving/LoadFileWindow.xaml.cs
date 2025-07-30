using System.IO;
using System.Windows;

namespace AlloyCalculatorRewrite.DataSaving
{
    /// <summary>
    /// Interaction logic for LoadFileWindow.xaml
    /// </summary>
    public partial class LoadFileWindow : Window
    {
        private LoadManager data = new LoadManager();
        private Alloy alloy;
        public LoadFileWindow()
        {
            InitializeComponent();
            data.CheckIfDirectoryExists();
            DisplaySaveFiles();
        }
        private void DisplaySaveFiles()
        {
            string[] files = Directory.GetFiles(data.GetSaveFolderDirectory());

            foreach (string file in files)
            {
                FileListBox.Items.Add(Path.GetFileName(file));
            }
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (FileListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a file to load.");
                return;
            }

            string selectedFile = FileListBox.SelectedItem.ToString();

            string filePath = Path.Combine(data.GetSaveFolderDirectory(), selectedFile);

            try
            {
                string fileContent = File.ReadAllText(filePath);
                //MessageBox.Show($"File loaded: {filePath}\nContent:\n{fileContent}");
                alloy = data.GetFileData(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load file: {ex.Message}");
            }
            Close();
        }

        public Alloy GetAlloyData()
        {
            return alloy;
        }
        private void OpenDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Open the SaveFiles directory in the default file explorer
                Close();
                System.Diagnostics.Process.Start("explorer.exe", data.GetSaveFolderDirectory());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open directory: {ex.Message}");
            }
        }
        private void searchBox_Changed(object sender, EventArgs e)
        {
            //Ignores the initial text changed on window start
            if (SearchBox.Text.Equals("Search Here"))
            {
                return;
            }
            DisplaySearchedFile(SearchBox.Text);
        }
        private void DisplaySearchedFile(string targetSearch)
        {
            string[] fileArray = Directory.GetFiles(data.GetSaveFolderDirectory());

            FileListBox.Items.Clear();

            foreach (string file in fileArray)
            {
                if (Path.GetFileName(file).IndexOf(targetSearch, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    FileListBox.Items.Add(Path.GetFileName(file));
                }
            }
        }
    }
}
