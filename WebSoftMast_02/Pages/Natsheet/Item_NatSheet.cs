namespace WebSoftMast_02.Pages.Natsheet
{
    public class Item_NatSheet
    {
        public long NatSheetId { get; set; }
        public string TrainNumber { get; set; } = "";
        public string TrainIndexCombined { get; set; } = ""; // номер состава
        public DateTime WhenLastOperation { get; set; } 
        public string ToStationName { get; set; } = "";

        public int TrainIndex {
            get
            {
                return int.Parse(TrainIndexCombined.Split('-')[1]);
            }}

    }
}
