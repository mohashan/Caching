﻿using Caching.InMemory.Cache;
using Caching.InMemory.Data;
using Caching.InMemory.Model;
using Microsoft.AspNetCore.Mvc;

namespace Caching.InMemory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICacheService _cacheService;
        private static object _lock = new object();
        public ProductController(ApplicationDbContext dbContext, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }
        [HttpGet("products")]
        public IEnumerable<Product> Get()
        {
            var cacheData = _cacheService.GetData<IEnumerable<Product>>("product");
            if (cacheData != null)
            {
                return cacheData;
            }
            lock (_lock)
            {
                var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
                cacheData = _dbContext.Products.ToList();
                _cacheService.SetData<IEnumerable<Product>>("product", cacheData, expirationTime);
            }
            return cacheData;
        }
        [HttpGet("products/{id:int}")]
        public Product Get(int id)
        {
            Product filteredData;
            var cacheData = _cacheService.GetData<IEnumerable<Product>>("product");
            if (cacheData != null)
            {
                filteredData = cacheData.Where(x => x.ProductId == id).FirstOrDefault();
                return filteredData;
            }
            filteredData = _dbContext.Products.Where(x => x.ProductId == id).FirstOrDefault();
            return filteredData;
        }
        [HttpPost("products")]
        public async Task<Product> Post(Product value)
        {
            var obj = await _dbContext.Products.AddAsync(value);
            _cacheService.RemoveData("product");
            _dbContext.SaveChanges();
            return obj.Entity;
        }
        [HttpPut("products")]
        public void Put(Product product)
        {
            _dbContext.Products.Update(product);
            _cacheService.RemoveData("product");
            _dbContext.SaveChanges();
        }
        [HttpDelete("products")]
        public void Delete(int Id)
        {
            var filteredData = _dbContext.Products.Where(x => x.ProductId == Id).FirstOrDefault();
            _dbContext.Remove(filteredData);
            _cacheService.RemoveData("product");
            _dbContext.SaveChanges();
        }
    }
}
