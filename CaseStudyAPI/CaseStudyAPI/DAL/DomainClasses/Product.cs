using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CaseStudyAPI.DAL.DomainClasses
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Required]
        public string? Id { get; set; }
        [ForeignKey("BrandId")]
        public Brand? Brand { get; set; } // generates FK

        [Required]
        public int BrandId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public string? GraphicName { get; set; }
        [Required]
        public decimal CostPrice { get; set; }
        [Required]
        public decimal MSRP { get; set; }
        [Required]
        public int QtyOnHand { get; set; }
        [Required]
        public int QtyOnBackOrder { get; set; }
        [Required]
        public string? Description { get; set; }
    }
}
