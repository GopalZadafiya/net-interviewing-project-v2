using System.ComponentModel.DataAnnotations;

namespace Insurance.Domain.Entities
{
    public class SurchargeRate
    {
        [Key]
        public int Id { get ; set; }

        [Required]
        public int ProductTypeId { get; set; }

        public float Value { get; set; }
    }
}
