using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseStudyAPI.DAL.DomainClasses
{
    [Route("api/[controller]")]
    [ApiController]
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }
    }
}
