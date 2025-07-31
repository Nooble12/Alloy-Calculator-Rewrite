using System.Collections.ObjectModel;
using System.Windows;

namespace AlloyCalculatorRewrite
{
    /// <summary>
    /// Interaction logic for AlloyResultsWindow.xaml
    /// </summary>
    public partial class AlloyResultsWindow : Window
    {
        private ObservableCollection<Metal> metalList = new ObservableCollection<Metal>();
        public AlloyResultsWindow(bool isSolutionFound, Alloy inAlloy)
        {
            InitializeComponent();

            if (isSolutionFound)
            {
                MetalListBox.ItemsSource = metalList;
                SetMetalList(inAlloy.MetalList);
                AlloyVolumeLabel.Content = inAlloy.Name + " Volume: " + inAlloy.AlloyVolume + "mb" + " or " + GetNumberOfIngots(inAlloy) + " bars";
            }
            else
            {
                HeaderGrid.Visibility = Visibility.Hidden;
                MetalListBox.Visibility = Visibility.Hidden;
                ErrorLabel.Visibility = Visibility.Visible;
            }
        }

        private int GetNumberOfIngots(Alloy inAlloy)
        {
            return inAlloy.AlloyVolume / inAlloy.MetalList.First().IngotVolume;
        }

        private void SetMetalList(List<Metal> inList)
        {
            foreach (Metal metal in inList)
            {
                metalList.Add(metal);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
