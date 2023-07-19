using CaseStudyAPI.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace CaseStudyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        readonly AppDbContext? _ctx;
        public DataController(AppDbContext context) // injected here
        {
            _ctx = context;
        }

        [HttpGet]
        public async Task<ActionResult<String>> Index()
        {
            DataUtility util = new(_ctx!);
            string payload = "";
            var db = await GetMenuItemJsonFromWebAsync();
            try
            {
                payload = (await util.LoadDataFromWebToDb(db)) ? "tables loaded" : "problem loading tables";
            }
            catch (Exception ex)
            {
                payload = ex.Message;
            }
            return JsonSerializer.Serialize(payload);
        }

        private async Task<string> GetMenuItemJsonFromWebAsync()
        {
            string url = "https://raw.githubusercontent.com/jaurigueml/INFO-3067/main/casestudydata3.json";

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }



    }
}
