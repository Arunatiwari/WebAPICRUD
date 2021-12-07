using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.Models;
using DAL;
using System.Text;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        HttpClient _client;
        Uri _baseAddress;
        IConfiguration _configuration;
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
            _baseAddress = new Uri(_configuration["ApiAddress"]);
            _client = new HttpClient();
            _client.BaseAddress = _baseAddress;
        }


        public IActionResult Index()
        {
            IEnumerable<ProductModel> model = new List<ProductModel>();
            var response = _client.GetAsync(_baseAddress + "/product/getall").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                model = JsonSerializer.Deserialize<IEnumerable<ProductModel>>(data);
            }
            return View(model);
        }

        private IEnumerable<Category> GetCategories()
        {
            IEnumerable<Category> model = new List<Category>();
            var response = _client.GetAsync(_baseAddress + "/category/getall").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                model = JsonSerializer.Deserialize<IEnumerable<Category>>(data);
            }
            return model;
        }

        public IActionResult Create()
        {
            ViewBag.Categories = GetCategories();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product model)
        {
            try
            {
                ModelState.Remove("ProductId");
                if (ModelState.IsValid)
                {
                    string data = JsonSerializer.Serialize(model);
                    StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = _client.PostAsync(_client.BaseAddress + "/product/add", stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }

                }

            }
            catch (Exception ex)
            {

            }
            ViewBag.Categories = GetCategories();
            return View();
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = GetCategories();
            ProductModel model = new ProductModel();
            var response = _client.GetAsync(_client.BaseAddress + "/product/get/" + id).Result;
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                model = JsonSerializer.Deserialize<ProductModel>(data);
            }
            return View("Create", model);
        }

        [HttpPost]
        public IActionResult Edit(ProductModel model)
        {
            try
            {               
                string data = JsonSerializer.Serialize(model);
                StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                var response = _client.PutAsync(_client.BaseAddress + "/product/update/" + model.ProductId ,stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }               
            }
            catch (Exception ex)
            {

            }
            ViewBag.Categories = GetCategories();
            return View();
        }

        public IActionResult Delete(int id)
        {
            var response = _client.DeleteAsync(_client.BaseAddress + "/product/delete/" + id).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
