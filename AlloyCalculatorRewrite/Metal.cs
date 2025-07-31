using System.Text;
using System.Xml.Serialization;

namespace AlloyCalculatorRewrite
{
    public class Metal
    {
        [XmlElement("Metal_Name")]
        public string Name { get; set; } = "N/A";
        [XmlIgnore]
        public int IngotCount { get; set; } = 0; // This will be incremented in the runtime.
        [XmlIgnore]
        public int MinIngot { get; set; } //Runtime 
        [XmlIgnore]
        public int MaxIngot { get; set; } // Runtime
        [XmlIgnore]
        public float IngotPercentOfAlloy { get; set; } // Runtime.
        [XmlElement("Ingot_Volume")]
        public int IngotVolume { get; set; } = 144; // default ingot volume
        [XmlElement("Minimum_Percent")]
        public int MinimumPercent { get; set; }
        [XmlElement("Maximum_Percent")]
        public int MaximumPercent { get; set; }

        //No param for .xml serialization
        public Metal()
        {

        }
        public Metal(string name, int minimumPercent, int maximumPercent)
        {
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
    }
}
