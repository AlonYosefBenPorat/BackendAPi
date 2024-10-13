
//using ApiWebApp.DAL.Model;
//using Microsoft.EntityFrameworkCore;

//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace ApiWebApp.Repositories
//{
//    public class CustomerRepository(WebAppContext context) : ICustomerRepository
//    {
//        private readonly WebAppContext _context = context;

//        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
//        {
//            return await _context.Customers.ToListAsync();
//        }

//        public async Task<Customer> GetCustomerByIdAsync(Guid id)

//        {
//            var customer = await _context.Customers.FindAsync(id);
//            if (customer == null)
//                throw new NotImplementedException($"{id} Of Customer Item not found.");
//            return customer;
//        }

//        public async Task AddCustomerAsync(Customer customer)
//        {
//            await _context.Customers.AddAsync(customer);
//            await _context.SaveChangesAsync();
//        }

//        public async Task UpdateCustomerAsync(Customer customer)
//        {
//            _context.Customers.Update(customer);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteCustomerAsync(Guid id)
//        {
//            var customer = await _context.Customers.FindAsync(id);
//            if (customer != null)
//            {
//                _context.Customers.Remove(customer);
//                await _context.SaveChangesAsync();
//            }
//        }




//        public async Task<IEnumerable<Customer>> GetCustomerWithAssetsAsync(Guid id)
//        {
//            return await _context.Customers
//                .Include(c => c.Assets)
//                .Where(c => c.Id == id)
//                .ToListAsync();
//        }

       
//    }
//}
