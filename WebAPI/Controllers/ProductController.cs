using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{


    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        AppDBContext _db;
        public ProductController(AppDBContext db)
        {
            _db = db;
        }

        //api/product/getall
        [HttpGet]
        public IEnumerable<Product> GetAll()
        {
            return _db.Products;
        }

        //api/product/1
        [HttpGet("{id}")]
        public Product Get(int id)
         {
            return _db.Products.Find(id);
        }

        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [SwaggerOperation(Summary = "Add New Product based upon {id}", OperationId = "AddProduct")]
        [HttpPost]
        public IActionResult Add(Product model)
        {
            try
            {
                _db.Products.Add(model);
                _db.SaveChanges();
                //return Created("/product", model);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [SwaggerOperation(Summary = "Update existing Product based upon {id}", OperationId = "UpdateProduct")]

        [HttpPut("{id}")]
        public IActionResult Update(int id, Product model)
        {
            try
            {
                if (id != model.ProductId)
                    return BadRequest();

                _db.Products.Update(model);
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {

                Product model = _db.Products.Find(id);
                if (model != null)
                {
                    _db.Products.Remove(model);
                    _db.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
