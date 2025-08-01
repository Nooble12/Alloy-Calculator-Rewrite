using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlloyCalculatorRewrite
{
    /// <summary>
    /// Interaction logic for CreateMetalWindow.xaml
    /// </summary>
    public partial class CreateMetalWindow : Window
    {
        private bool inputsAreValid = false;
        private bool nameInputIsValid = false;
        private bool isEdit = false; // if the user is editing and not creating
        private string originalName = "";
        private Metal metal;
        private List<string> metalNameList;
        private int ingotVolume = 144; // default
        private int minPercent = 0;
        private int maxPercent = 10;
        public CreateMetalWindow(Metal inMetal, List<string> inMetalList, bool isEdit)
        {
            InitializeComponent();
            Visibility = Visibility.Visible;
            metal = inMetal;
            metalNameList = inMetalList;
            originalName = metal.Name;
            this.isEdit = isEdit;
            SetInitalValues(inMetal);
        }

        private void SetInitalValues(Metal inMetal)
        {
            MetalNameTextBox.Text = inMetal.Name;
            IngotVolumeTextBox.Text = inMetal.IngotVolume.ToString();
            MinimumPercentTextBox.Text = inMetal.MinimumPercent.ToString();
            MaximumPercentTextBox.Text = inMetal.MaximumPercent.ToString();
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (inputsAreValid && nameInputIsValid && CheckIfNegativeInt())
            {
                metal.Name = MetalNameTextBox.Text;
                metal.IngotVolume = ingotVolume;
                metal.MinimumPercent = minPercent;
                metal.MaximumPercent = maxPercent;

                this.DialogResult = true;
                Close();
            }
        }
        private void IngotVolumeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputResult result = CheckIfValidInt(IngotVolumeTextBox);
            if (result.InputIsValid)
            {
                ingotVolume = result.InputValue;
            }
        }

        private void MinimumPercentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputResult result = CheckIfValidInt(MinimumPercentTextBox);
            if (result.InputIsValid)
            {
                minPercent = result.InputValue;
                CheckMinAndMax(MinimumPercentTextBox);
            }
        }

        private void MaximumPercentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputResult result = CheckIfValidInt(MaximumPercentTextBox);
            if (result.InputIsValid)
            {
                maxPercent = result.InputValue;
                CheckMinAndMax(MaximumPercentTextBox);
            }
        }

        private void CheckMinAndMax(TextBox inTextBox)
        {
            //Checks if min and max are correct
            if (maxPercent < minPercent || minPercent > maxPercent)
            {
                DisplayErrorMessage("Invalid Percent Range");
                HighlightTextBox(inTextBox, Brushes.Red);
                inputsAreValid = false;
            }
            else
            {
                List<TextBox> list = new List<TextBox>();
                list.Add(MinimumPercentTextBox);
                list.Add(MaximumPercentTextBox);
                UnHighlightBoxes(list);

                inputsAreValid = true;
                DisplayErrorMessage("");
            }
        }

        private void UnHighlightBoxes(List<TextBox> inList)
        {
            foreach (var box in inList)
            {
                if (box.Foreground != Brushes.White)
                {
                    HighlightTextBox(box, Brushes.White);
                }
            }
        }

        private InputResult CheckIfValidInt(TextBox inTextBox)
        {
            int number = 0;
            bool isSuccess = int.TryParse(inTextBox.Text, out number);

            if (isSuccess && number < int.MaxValue)
            {
                DisplayErrorMessage("");
                HighlightTextBox(inTextBox, Brushes.White);
            }
            else
            {
                DisplayErrorMessage("Error, invalid integer.");
                HighlightTextBox(inTextBox, Brushes.Red);
                inputsAreValid = false;
            }

            InputResult result = new InputResult(isSuccess, number);
            return result;

        }

        private bool CheckIfNegativeInt()
        {
            if (minPercent < 0 || maxPercent < 0 || ingotVolume < 0)
            {
                DisplayErrorMessage("Inputs must be positive intergers");
                return false;
            }
            else
            {
                return true;
            }
        }
        private void DisplayErrorMessage(string errorMessage)
        {
            ErrorMessageTextBlock.Text = errorMessage;
        }

        private void HideSubmitButton()
        {
            SubmitButton.Visibility = Visibility.Hidden;
        }
        private void ShowSubmitButton()
        {
            SubmitButton.Visibility = Visibility.Visible;
        }

        private void HighlightTextBox(TextBox textBox, SolidColorBrush color)
        {
            textBox.Background = color;
        }

        private void MetalNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string inputName = MetalNameTextBox.Text;

            CheckIfSameNames(inputName);
        }

        private void CheckIfSameNames(string inputName)
        {
            if (metalNameList.Contains(inputName))
            {
                if (isEdit && originalName.Equals(inputName)) // Allows the user to keep the same name when editing
                {
                    nameInputIsValid = true;
                    return;
                }
                nameInputIsValid = false;
                HighlightTextBox(MetalNameTextBox, Brushes.Red);
                DisplayErrorMessage("Can not use same names");
            }
            else
            {
                nameInputIsValid = true;
                HighlightTextBox(MetalNameTextBox, Brushes.White);
                DisplayErrorMessage("");
            }
        }
    }
}
