using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private AppDBContext _db;
        public CategoryController(AppDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAll()
        {
            var data = _db.Categories.ToList();
            return Ok(data);
        }

    }
}
