namespace SynchData.Model
{
    public class InventoryItemExcel
    {
        public string SkuName { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public decimal ProfitPerItem { get; set; }
        public int Quantity { get; set; }
        public int WasDeleted { get; set; }
    }
}
