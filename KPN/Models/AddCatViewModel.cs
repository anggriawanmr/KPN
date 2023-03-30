using KPN.Models.Domain;

namespace KPN.Models
{
    public class AddCatViewModel
    {
        public int GuildId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Type { get; set; }
        public string Colour { get; set; }
        public string Food { get; set; }
        public double Amount { get; set; }

        public List<CatGenderTypeCount> CatGenderTypeCounts { get; set; }

        public List<Cat> Cats { get; set; }

        public List<CatPivotTableRow> PivotTable { get; set; }

        public List<CatFoodAmount> CatFoodAmounts { get; set; }

        public List<CatFoodPivotTableRow> CatFoodPivotTable { get; set; }
    }

    public class CatFoodAmount
    {
        public string Type { get; set; }
        public string Food { get; set; }
        public double Amount { get; set; }
    }
    public class CatPivotTableRow
    {
        public string Gender { get; set; }
        public int Persian { get; set; }
        public int Mainecoon { get; set; }
        public int Sphynx { get; set; }
    }
    public class CatGenderTypeCount
    {
        public string Type { get; set; }
        public string Gender { get; set; }
        public int Count { get; set; }
    }
    public class CatFoodPivotTableRow
    {
        public string Food { get; set; }
        public double Persian { get; set; }
        public double Mainecoon { get; set; }
        public double Sphynx { get; set; }
    }
}
