using System.ComponentModel.DataAnnotations;

namespace TestDemo.Model
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(30)]
        public string Location { get; set; }

        public int JobTitleId { get; set; }

      //  public JobTitle JobTitle { get; set; }
    }
}
