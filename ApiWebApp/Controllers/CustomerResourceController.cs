//using ApiWebApp.Repositories;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ApiWebApp.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CustomerResourceController : ControllerBase
//    {
//        private readonly ICustomerRepository _customerRepository;

//        public CustomerResourceController(ICustomerRepository customerRepository)
//        {
//            _customerRepository = customerRepository;
//        }


//        // Get customer with assets by CustomerID
//        [HttpGet("{id}/assets")]
//        public async Task<IActionResult> GetCustomerWithAssets(Guid id)
//        {
//            var customer = await _customerRepository.GetCustomerWithAssetsAsync(id);
//            if (customer == null)
//            {
//                return NotFound(new { Message = $"Customer with ID:{id} not found." });
//            }

//            var result = new
//            {
//                customer.
//                customer.Id,
//                customer.Name,
//                customer.Country,
//                customer.City,
//                customer.Address,
//                customer.Phone,
//                customer.ContactPerson,
//                customer.Domain,
//                customer.BnNumber,
//                customer.CreatedAt,
//                customer.UpdatedAt,
//                customer.IsActive,
//                customer.Logo,
//                Assets = customer.Assets.Select(a => new
//                {
//                    a.Id,
//                    a.Type,
//                    a.IpAddress,
//                    a.Url,
//                    a.License,
//                    a.SupportExpiration,
//                    a.Notes,
//                    a.UpdatedAt
//                })
//            };

//            return Ok(result);
//        }
//    }
//}
