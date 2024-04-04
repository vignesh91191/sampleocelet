using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace TestDemo.Model
{
    public class ItemsSoldInfo
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateTimeSold { get; set; }

        public int QuantitySold { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal Cost { get; set; }

        public int CostCurrencyId { get; set; }

        public Currency CostCurrency { get; set; }

        public int PaidCurrencyId { get; set; }

        public Currency PaidCurrency { get; set; }

        public int Quantity { get; set; }

        public int WasDeleted { get; set; }

        public int SoldById { get; set; }

        public Person SoldBy { get; set; }
    }
}
