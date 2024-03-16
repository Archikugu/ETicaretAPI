using ETicaretAPI.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }

        [HttpGet]
        public async Task Get()
        {
           //await TestProduct();

            var product = await _productReadRepository.GetByIdAsync("e0cd520a-d9ea-461a-b96d-f6ff0cb32226", false);
            product.Name = "Product deneme";
            await _productWriteRepository.SaveAsync();
        }

        private async Task TestProduct()
        {
            await _productWriteRepository.AddRangeAsync(new()
            {
                new(){Id = Guid.NewGuid(),Name="Product 1",Price=100,CreatedDate=DateTime.UtcNow,Stock=10},
                new(){Id = Guid.NewGuid(),Name="Product 2",Price=200,CreatedDate=DateTime.UtcNow,Stock=20},
                new(){Id = Guid.NewGuid(),Name="Product 3",Price=300,CreatedDate=DateTime.UtcNow,Stock=30},
            });
            await _productWriteRepository.SaveAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var product = await _productReadRepository.GetByIdAsync(id);
            return Ok(product);
        }
    }
}
