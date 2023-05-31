using Caching.InMemory.Cache;
using Caching.InMemory.Data;
using Caching.InMemory.Model;
using Microsoft.AspNetCore.Mvc;

namespace Caching.InMemory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCachlessController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductCachlessController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("products")]
        public IEnumerable<Product> Get()
        {
            return _dbContext.Products.ToList();
        }
        [HttpGet("products/{id:int}")]
        public Product Get(int id)
        {
            return _dbContext.Products.Where(x => x.ProductId == id).FirstOrDefault();
        }
        [HttpPost("products")]
        public async Task<Product> Post(Product value)
        {
            var obj = await _dbContext.Products.AddAsync(value);
            _dbContext.SaveChanges();
            return obj.Entity;
        }
        [HttpPut("products")]
        public void Put(Product product)
        {
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
        }
        [HttpDelete("products")]
        public void Delete(int Id)
        {
            var filteredData = _dbContext.Products.Where(x => x.ProductId == Id).FirstOrDefault();
            _dbContext.Remove(filteredData);
            _dbContext.SaveChanges();
        }
    }
}
