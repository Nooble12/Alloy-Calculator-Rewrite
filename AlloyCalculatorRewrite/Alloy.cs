using System.Text;
using System.Xml.Serialization;

namespace AlloyCalculatorRewrite
{
    public class Alloy
    {
        [XmlElement("Alloy_Name")]
        public string Name { get; set; } = "N/A";
        [XmlElement("Max_Alloy_Volume")]
        public int MaxAlloyVolume { get; set; }

        // Holds the metal objects to form the alloy
        [XmlElement("Alloy_Contents")]
        public List<Metal> MetalList { get; set; }

        [XmlIgnore]
        public int AlloyVolume {get;set;} 

        //No param for xml serialization
        public Alloy()
        {

        }

        public Alloy(string name, int maxAlloyVolume, List<Metal> metalList)
        {
            Name = name;
            MaxAlloyVolume = maxAlloyVolume;
            MetalList = metalList;
        }

        public string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Alloy Name: " + Name);
            builder.AppendLine("Max Volume: " + MaxAlloyVolume + "mb");

            builder.AppendLine("Metals In Alloy" + " (" + MetalList.Count + ")");
            foreach (var metal in MetalList)
            {
                builder.AppendLine(metal.ToString());
            }
            return builder.ToString();
        }
    }
}
