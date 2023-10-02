using System.Reflection.Metadata;

namespace InventoryManagement.ViewModel
{
    public class InventoryViewModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string ItemSku { get; set; }
        public int ItemQuantity { get; set; }
        public int ItemMinimumQuantity { get; set; }
        public Blob ItemImage { get; set; }
    }
}
