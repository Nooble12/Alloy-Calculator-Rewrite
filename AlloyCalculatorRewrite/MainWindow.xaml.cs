using AlloyCalculatorRewrite.DataSaving;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlloyCalculatorRewrite;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public ObservableCollection<Metal> metalList { get; set; } = new ObservableCollection<Metal>();
    private Alloy alloy;
    private string alloyName;
    private int maxAlloyVolume;

    //flag for if input data is valid. i.e integers
    private bool isInputDataValid = false;

    public MainWindow()
    {
        InitializeComponent();
        MetalListBox.ItemsSource = metalList;
    }

    private void SaveFileButton_Click(object sender, RoutedEventArgs e)
    {
        if (!isInputDataValid || metalList.Count < 2)
        {
            return;
        }

        alloy = new Alloy(alloyName, maxAlloyVolume, ConvertObservableCollectionToList(metalList));
        var saveFileWindow = new SaveFileWindow(alloy);
        bool? result = saveFileWindow.ShowDialog();
    }

    private List<Metal> ConvertObservableCollectionToList(ObservableCollection<Metal> collection)
    {
        List<Metal> alloyMetals = new List<Metal>();

        foreach (var metal in collection)
        {
            alloyMetals.Add(metal);
        }

        return alloyMetals;
    }
    private void LoadFile_Click(object sender, RoutedEventArgs e)
    {
        var loadFileWindow = new LoadFileWindow();
        bool? result = loadFileWindow.ShowDialog();
        Alloy tempAlloy = loadFileWindow.GetAlloyData();

        if (tempAlloy == null) // uh oh, something went wrong
        {
            MessageBox.Show("Error, could not load file :( ");
            return;
        }

        metalList.Clear();

        //Setting the current alloy to the loaded alloy
        alloy = tempAlloy;

        //Setting the collection to the alloy's metal contents to display new info
        foreach (var metal in alloy.MetalList)
        {
            metalList.Add(metal);
        }

        //Setting alloy display properties
        AlloyNameTextBox.Text = alloy.Name;
        MaxAlloyVolumeTextBox.Text = alloy.MaxAlloyVolume.ToString();
    }
    private void AddMetal_Click(object sender, RoutedEventArgs e)
    {
        Metal metal = new Metal("N/A", 0, 0);
        bool result = CreateNewMetalDialogBox(metal, false);

        if (result)
        {
            metalList.Add(metal);
        }
        else
        {
            Debug.WriteLine("Metal not created");
        }
    }

    //gets the name of all the metals in the collection. will be used to check if 2 metals have the same name to prevent crashing.
    private List<string> GetAllMetalNames(List<Metal> metalList)
    {
        List<string> nameList = new List<string>();
        foreach(var metal in metalList)
        {
            nameList.Add(metal.Name);
        }

        return nameList;
    }
    private bool CreateNewMetalDialogBox(Metal metal, bool isEdit)
    {
        List <Metal> list = metalList.ToList();
        List<string> metalNameList = GetAllMetalNames(list);
        var inputWindow = new CreateMetalWindow(metal, metalNameList, isEdit);
        bool? result = inputWindow.ShowDialog();

        return (bool)result;
    }

    private void ClearAllMetal_Click(object sender, RoutedEventArgs e)
    {
        if(metalList.Count <= 0)
        {
            return;
        }
        var inputWindow = new ConfirmWindow();
        bool? result = inputWindow.ShowDialog();

        bool confirmResult = inputWindow.GetResult();

        if (confirmResult)
        {
            metalList.Clear();
        }
    }

    private void CalculateAlloy_Click(object sender, RoutedEventArgs e)
    {
        if (!isInputDataValid || metalList.Count < 2)
        {
            return;
        }

        List<Metal> alloyMetals = ConvertObservableCollectionToList(metalList);

        alloy = new Alloy(alloyName, maxAlloyVolume, alloyMetals);

        CalculateAlloy alloyResults = new CalculateAlloy(alloy);

    }
    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button editButton)
        {
            var instance = editButton.DataContext;
            switch (instance)
            {
                case Metal metal:
                    CreateNewMetalDialogBox(metal, true);

                    int index = metalList.IndexOf(metal);
                    //Jank but simple way to update the metal info on screen.
                    metalList.Remove(metal);
                    metalList.Insert(index, metal);

                break;
            }
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button editButton)
        {
            var instance = editButton.DataContext;
            switch (instance)
            {
                case Metal metal:
                    metalList.Remove(metal);
                break;
            }
        }
    }

    private void AlloyNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        alloyName = AlloyNameTextBox.Text;
    }

    private void MaxAlloyVolumeTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        int number = 0;
        bool isSuccess = int.TryParse(MaxAlloyVolumeTextBox.Text, out number);

        if (isSuccess && number < uint.MaxValue)
        {
            maxAlloyVolume = number;
            MaxAlloyVolumeTextBox.Background = Brushes.White;
            isInputDataValid = true;
        }
        else
        {
            isInputDataValid = false;
            MaxAlloyVolumeTextBox.Background = Brushes.Red;
        }
    }

    private void About_Click(object sender, RoutedEventArgs e)
    {
        var inputWindow = new AboutWindow();
        bool? result = inputWindow.ShowDialog();
    }
}