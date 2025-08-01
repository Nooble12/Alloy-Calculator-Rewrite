using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;

namespace AlloyCalculatorRewrite
{
    public class Metal : INotifyPropertyChanged
    {
        [XmlIgnore]
        public int IngotCount { get; set; } = 0; // This will be incremented in the runtime.
        [XmlIgnore]
        public int MinIngot { get; set; } //Runtime 
        [XmlIgnore]
        public int MaxIngot { get; set; } // Runtime
        [XmlIgnore]
        public float IngotPercentOfAlloy { get; set; } // Runtime.

        private string _metalName = "N/A";
        [XmlElement("Metal_Name")]
        public string Name 
        {
            get => _metalName;
            set => SetProperty(ref _metalName, value);
        }

        private int _ingotVolume;
        [XmlElement("Ingot_Volume")]
        public int IngotVolume 
        {
            get => _ingotVolume;
            set => SetProperty(ref _ingotVolume, value);
        }

        private int _minimumPercent;
        [XmlElement("Minimum_Percent")]
        public int MinimumPercent 
        {
            get => _minimumPercent;
            set => SetProperty(ref _minimumPercent, value);
        }

        private int _maximumPercent;
        [XmlElement("Maximum_Percent")]
        public int MaximumPercent
        {
            get => _maximumPercent;
            set => SetProperty(ref _maximumPercent, value);
        }

        //No param for .xml serialization
        public Metal()
        {

        }
        public Metal(string name, int minimumPercent, int maximumPercent, int ingotVolume)
        {
            IngotVolume = ingotVolume;
            Name = name;
            MinimumPercent = minimumPercent;
            MaximumPercent = maximumPercent;
        }

        public string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("----------");
            builder.AppendLine("Name: " + Name);
            builder.AppendLine("Ingot Amount: " + IngotCount);
            builder.AppendLine("Single Ingot Volume: " + IngotVolume + "mb");
            builder.AppendLine("Minimum Percent: " + MinimumPercent);
            builder.AppendLine("Maximum Percent: " + MaximumPercent);
            builder.AppendLine("----------");
            return builder.ToString();
        }

        // Helper method for OnPropertyChanged
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
