using System.Windows;

namespace AlloyCalculatorRewrite
{
    /// <summary>
    /// Interaction logic for ConfirmWindow.xaml
    /// </summary>
    public partial class ConfirmWindow : Window
    {
        private bool isConfirm = false;
        public ConfirmWindow()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            isConfirm = true;
            Close();
        }

        public bool GetResult()
        {
            return isConfirm;
        }
    }
}
