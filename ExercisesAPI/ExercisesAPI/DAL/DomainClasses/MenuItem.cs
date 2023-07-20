using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExercisesAPI.DAL.DomainClasses
{
    public class MenuItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; } // generates FK
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int Calories { get; set; }
        [Required]
        public int Carbs { get; set; }
        [Required]
        public int Cholesterol { get; set; }
        [Required]
        public float Fat { get; set; }
        [Required]
        public int Fibre { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        [StringLength(200)]

        public int Protein { get; set; }
        [Required]
        public int Salt
        {
            get; set;
        }
    }
}
