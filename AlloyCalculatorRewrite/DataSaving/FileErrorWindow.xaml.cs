using System.Windows;

namespace AlloyCalculatorRewrite.DataSaving
{
    /// <summary>
    /// Interaction logic for FileErrorWindow.xaml
    /// </summary>
    public partial class FileErrorWindow : Window
    {
        public FileErrorWindow(string errorMessage)
        {
            InitializeComponent();
            ErrorMessageLabel.Text = errorMessage;
        }
    }
}
