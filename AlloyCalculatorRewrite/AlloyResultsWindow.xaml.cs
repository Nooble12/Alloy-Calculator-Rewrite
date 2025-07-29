using System.Text;
using System.Windows;

namespace AlloyCalculatorRewrite
{
    /// <summary>
    /// Interaction logic for AlloyResultsWindow.xaml
    /// </summary>
    public partial class AlloyResultsWindow : Window
    {
        public AlloyResultsWindow(bool isSolutionFound, Alloy inAlloy)
        {
            InitializeComponent();

            if (isSolutionFound)
            {
                WriteResultsToTextBox(inAlloy);
            }
            else
            {
                ResultsTextBox.Text = "Error, could not find solution!";
            }
        }

        private float GetIngotPercent(Metal inMetal, Alloy inAlloy)
        {
            float ingotCountFloat = (float)inMetal.IngotCount;
            float ingotVolumeFloat = (float)inMetal.IngotVolume;

            return ((ingotCountFloat * ingotVolumeFloat) / GetAlloyVolume(inAlloy) * 100);
        }

        private void WriteResultsToTextBox(Alloy inAlloy)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(inAlloy.Name);
            builder.AppendLine("Max Volume:\t" + inAlloy.MaxAlloyVolume + "mb");
            builder.AppendLine("Alloy Volume:\t" + GetAlloyVolume(inAlloy) + "mb");
            builder.AppendLine("---------------------");
            foreach (var metal in inAlloy.MetalList)
            {
                float ingotPercent = GetIngotPercent(metal, inAlloy);
                string roundedIngotPercent = ingotPercent.ToString("F2");
                builder.AppendLine(metal.Name + ":\t" + metal.IngotCount + " (" + roundedIngotPercent + "%)");
            }
            ResultsTextBox.Text = builder.ToString();
        }

        private int GetAlloyVolume(Alloy inAlloy)
        {
            int maxIngots = inAlloy.MaxAlloyVolume / inAlloy.MetalList.First().IngotVolume;
            int alloyVolume = maxIngots * inAlloy.MetalList.First().IngotVolume;
            return alloyVolume;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
