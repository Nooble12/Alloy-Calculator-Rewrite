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
    private int ingotVolume = 144; // default

    //flag for if input data is valid. i.e integers
    private bool isMaxVolumeInputValid = false;
    private bool isIngotVolumeInputValid = false;

    public MainWindow()
    {
        InitializeComponent();
        MetalListBox.ItemsSource = metalList;
    }

    private void SaveFileButton_Click(object sender, RoutedEventArgs e)
    {
        if (!isMaxVolumeInputValid || metalList.Count < 2 || !isIngotVolumeInputValid)
        {
            return;
        }

        alloy = new Alloy(alloyName, maxAlloyVolume, metalList.ToList());
        var saveFileWindow = new SaveFileWindow(alloy);
        bool? result = saveFileWindow.ShowDialog();
    }

    private void LoadFile_Click(object sender, RoutedEventArgs e)
    {
        var loadFileWindow = new LoadFileWindow();
        bool? result = loadFileWindow.ShowDialog();
        Alloy tempAlloy = loadFileWindow.GetAlloyData();

        if (tempAlloy == null) // uh oh, something went wrong
        {
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
        IngotVolumeTextBox.Text = alloy.MetalList.First().IngotVolume.ToString(); // We assume all ingots have the same volume
    }
    private void AddMetal_Click(object sender, RoutedEventArgs e)
    {
        Metal metal = new Metal("N/A", 0, 0, ingotVolume);
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

        //Display Message Box
        string messageBoxText = "Do you want to clear all metals?";
        string caption = "Confirm Action";

        MessageBoxResult result = MessageBox.Show(messageBoxText, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning);

        switch(result)
        {
            case MessageBoxResult.Yes:
                metalList.Clear();
            break;
        }
    }

    private void CalculateAlloy_Click(object sender, RoutedEventArgs e)
    {
        if (!isMaxVolumeInputValid || metalList.Count < 2 || !isIngotVolumeInputValid)
        {
            return;
        }

        List<Metal> alloyMetals = metalList.ToList();

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

        if (isSuccess && number < int.MaxValue)
        {
            maxAlloyVolume = number;
            MaxAlloyVolumeTextBox.Background = Brushes.White;
            isMaxVolumeInputValid = true;
        }
        else
        {
            isMaxVolumeInputValid = false;
            MaxAlloyVolumeTextBox.Background = Brushes.Red;
        }
    }

    private void IngotVolumeTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        int number = 0;
        bool isSuccess = int.TryParse(IngotVolumeTextBox.Text, out number);

        if (isSuccess && number < int.MaxValue && number > 0)
        {
            ingotVolume = number;
            IngotVolumeTextBox.Background = Brushes.White;
            isIngotVolumeInputValid = true;

            ApplyNewIngotVolume();
        }
        else
        {
            isIngotVolumeInputValid = false;
            IngotVolumeTextBox.Background = Brushes.Red;
        }
    }

    //if metals are already in the list, apply the new volume to those objects
    private void ApplyNewIngotVolume()
    {
        if (metalList.Count > 0)
        {
            foreach (var metal in metalList)
            {
                if (metal.IngotVolume != ingotVolume)
                {
                    metal.IngotVolume = ingotVolume;
                }
            }
        }
    }

    private void About_Click(object sender, RoutedEventArgs e)
    {
        var inputWindow = new AboutWindow();
        bool? result = inputWindow.ShowDialog();
    }
}