using ETicaretAPI.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //Test Controller
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;
        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IOrderWriteRepository orderWriteRepository, ICustomerWriteRepository customerWriteRepository, IOrderReadRepository orderReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _customerWriteRepository = customerWriteRepository;
            _orderReadRepository = orderReadRepository;
        }

        [HttpGet]
        public async Task Get()
        {
            //var customerId = Guid.NewGuid();
            //await _customerWriteRepository.AddAsync(new() { Id = customerId, Name = "Muhiddin" });

            //await _orderWriteRepository.AddAsync(new() { Description = "Deneme desc", Address = "Ankara Yenimahalle", CustomerId = customerId });
            //await _orderWriteRepository.AddAsync(new() { Description = "Deneme desc 2", Address = "Ankara Çankaya", CustomerId = customerId });

            //await _orderWriteRepository.SaveAsync();


            var order= await _orderReadRepository.GetByIdAsync("8d58b25d-4a39-42ea-a5a1-d36859534448");
            order.Address = "İstanbul";
            await _orderWriteRepository.SaveAsync();

        }
    }
}
