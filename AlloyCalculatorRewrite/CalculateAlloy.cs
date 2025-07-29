using System.CodeDom;
using System.Diagnostics;
using System.Printing.IndexedProperties;

namespace AlloyCalculatorRewrite
{
    public class CalculateAlloy
    {
        private Alloy alloy;

        //Note: Code will return the alloy object that contains the updated ingot values within the metals table

        public CalculateAlloy(Alloy inAlloy)
        {
            alloy = inAlloy;
            bool isSolutionFound = Calculate();

            DisplayResultsWindow(isSolutionFound, inAlloy);

        }
        private void DisplayResultsWindow(bool isSolutionFound, Alloy alloy)
        {
            var inputWindow = new AlloyResultsWindow(isSolutionFound, alloy);
            bool? result = inputWindow.ShowDialog();
        }

        private void ClearIngotCount()
        {
            foreach(var metal in alloy.MetalList)
            {
                metal.IngotCount = 1; // 1 is the lowest value
                metal.MinIngot = 1;
                metal.MaxIngot = 1;
            }
        }

        //Brute force algo to determine intger values for each ingot. Not the most efficient but data set is usually small...
        private bool Calculate()
        {
            ClearIngotCount(); // Clears the count for each ingot
            bool result = FindCombination();
            return result;
        }

        private bool FindCombination()
        {
            int singleIngotVolume = alloy.MetalList.First().IngotVolume;
            int maxIngotCount = GetMaxIngotCount(alloy.MaxAlloyVolume, singleIngotVolume); // For example, 4000 mb / 144 mb = 27 ingots
            int maxAlloyVolume = maxIngotCount * singleIngotVolume; // the max volume allowed for the alloy (upper limit)

            //Find the min and max ingots for a given max alloy volume. Possible combinations that could work.
            foreach(var metal in alloy.MetalList)
            {
                metal.MinIngot = GetMinimumIngotsRequired(maxAlloyVolume, metal);
                metal.MaxIngot = GetMaximumIngotsRequired(maxAlloyVolume, metal);
            }
            
            bool results = TryCombinations(alloy.MetalList, new Dictionary<string, int>(), singleIngotVolume, maxAlloyVolume, 0);

            return results;
        }

        //A brute force recursive method with backtracking to find the correct combo, if possible. At most 4 different ingots, so performance should not be a major issue.
        private bool TryCombinations(List<Metal> metalList, Dictionary<string, int> currentCombo, int singleIngotVolume, int maxAlloyVolume, int index)
        {
            if(index == metalList.Count)
            {
                int totalAlloyVolume = currentCombo.Values.Sum() * singleIngotVolume; // Gets the volume of the combo.
                
                if (totalAlloyVolume <= maxAlloyVolume)
                {
                    int correctIngots = 0; // counter to count # of correct ingots. if correct, we "mark" the ingot as correct and if counter == length of metalList, all ingots should be correct.
                    foreach (var ingot in metalList)
                    {
                        float ingotPercentOfAlloy = GetPercentOfAlloyVolume(ingot.IngotVolume * currentCombo[ingot.Name], maxAlloyVolume);
                        if (ingotPercentOfAlloy >= ingot.minimumPercent && ingotPercentOfAlloy <= ingot.maximumPercent)
                        {
                            ingot.IngotCount = currentCombo[ingot.Name];
                            correctIngots++;
                        }
                    }

                    //Solution has been found
                    if(correctIngots == metalList.Count)
                    {
                        return true;
                    }
                }
                
                return false;
            }

            Metal metal = metalList.ElementAt(index);
            for (int count = metal.MinIngot; count <= metal.MaxIngot; count++)
            {
                currentCombo.Add(metal.Name, count);
                Debug.WriteLine(metal.Name + ": " + currentCombo[metal.Name]);

                if (TryCombinations(metalList, currentCombo, singleIngotVolume, maxAlloyVolume, index + 1))
                {
                    return true;
                }

                currentCombo.Remove(metal.Name);

            }

            return false;
        }

        private int GetMaximumIngotsRequired(int volume, Metal inMetal)
        {
            int max = (int)Math.Floor((inMetal.maximumPercent / 100f) * volume / inMetal.IngotVolume);
            return max;
        }

        private int GetMinimumIngotsRequired(int volume, Metal inMetal)
        {
            int min = (int)Math.Ceiling((inMetal.minimumPercent / 100f) * volume / inMetal.IngotVolume);
            return min;
        }

        //Gets the max integer ingot count that can fit in a given MaxVolume
        private int GetMaxIngotCount(int maxVolume, int singleIngotVolume)
        {
            int maxIngots = maxVolume / singleIngotVolume;
            return maxIngots;
        }

        private float GetPercentOfAlloyVolume(int inVolume, int totalAlloyVolume)
        {
            float volumeFloat = (float)inVolume;
            float maxVolumeFloat = (float)totalAlloyVolume;
            return (volumeFloat / maxVolumeFloat) * 100;
        }
        
    }
}

