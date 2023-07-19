using CaseStudyAPI.DAL;
using CaseStudyAPI.DAL.DAO;
using CaseStudyAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace CaseStudyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public ProductController(AppDbContext context)
        {
            _ctx = context;
        }

        [HttpGet]
        [Route("{brandid}")]
        public async Task<ActionResult<List<Product>>> Index(int brandid)
        {
            ProductDAO dao = new ProductDAO(_ctx);
            List<Product> productsForBrand = await dao.GetAllByBrand(brandid);
            return productsForBrand;
        }

        [HttpGet]
        public async Task<ActionResult<List<Brand>>> Index()
        {
            BrandDAO dao = new BrandDAO(_ctx);
            List<Brand> allBrands = await dao.GetAll();
            return allBrands;
        }

       
    }

}
