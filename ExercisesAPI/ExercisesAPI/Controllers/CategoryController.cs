using ExercisesAPI.DAL;
using ExercisesAPI.DAL.DAO;
using ExercisesAPI.DAL.DomainClasses;
using ExercisesAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.Json;
namespace ExercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        readonly AppDbContext? _ctx;
        public CategoryController(AppDbContext context) // injected here
        {
            _ctx = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> Index()
        {
            CategoryDAO dao = new(_ctx!);
            List<Category> allCategories = await dao.GetAll();
            return allCategories;
        }

        private static async Task<String> GetMenuItemJsonFromWebAsync()
        {
            string url = "https://raw.githubusercontent.com/elauersen/info3067/master/mcdonalds.json";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

    }

}
