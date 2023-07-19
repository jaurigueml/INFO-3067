using CaseStudyAPI.DAL;
using CaseStudyAPI.DAL.DAO;
using CaseStudyAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace CaseStudyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public BrandController(AppDbContext context)
        {
            _ctx = context;
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
