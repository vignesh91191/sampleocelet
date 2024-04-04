using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestDemo.Model
{
    public class InventoryItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string SkuName { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(6, 2)")]
        public decimal Cost { get; set; }

        public int CostCurrencyId { get; set; }

        public Currency CostCurrency { get; set; }

        [Required]
        [Column(TypeName = "decimal(6, 2)")]
        public decimal ProfitPerItem { get; set; }

        public int Quantity { get; set; }

        public int WasDeleted { get; set; }
    }
}
