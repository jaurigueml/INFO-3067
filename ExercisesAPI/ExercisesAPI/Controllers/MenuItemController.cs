using ExercisesAPI.DAL;
using ExercisesAPI.DAL.DAO;
using ExercisesAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace ExercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        readonly AppDbContext? _ctx;
        public MenuItemController(AppDbContext context) // injected here
        {
            _ctx = context;
        }

        [HttpGet]
        [Route("{catid}")]
        public async Task<ActionResult<List<MenuItem>>> Index(int catid)
        {
            MenuItemDAO dao = new(_ctx!);
            List<MenuItem> itemsForCategory = await dao.GetAllByCategory(catid);
            return itemsForCategory;
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
