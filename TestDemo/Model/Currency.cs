using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestDemo.Model
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(6)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Abbreviation { get; set; }

        [Required]
        [StringLength(6)]
        public string Symbol { get; set; }

        [Required]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal ConversionRateToUSD { get; set; }
    }
}
