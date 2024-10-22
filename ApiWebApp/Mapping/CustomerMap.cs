using ApiWebApp.Dto;
using DAL.Models.ItemsModel;

namespace ApiWebApp.Mapping
{
    public static class CustomerMap
    {
        public static Customer ToEntity(this CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                throw new ArgumentNullException(nameof(customerDto));
            }

            return new Customer
            {
                Id = Guid.NewGuid(),
                Name = customerDto.Name,
                Country = customerDto.Country,
                City = customerDto.City,
                Address = customerDto.Address,
                Phone = customerDto.Phone,
                ContactPerson = customerDto.ContactPerson,
                Domain = customerDto.Domain,
                BnNumber = customerDto.BnNumber,
                CreatedAt = DateTime.UtcNow,
                IsActive = customerDto.IsActive
            };
        }

        public static void UpdateEntity(this CustomerDto customerDto, Customer customer)
        {
            if (customerDto is null)
            {
                throw new ArgumentNullException(nameof(customerDto));
            }

            if (customer is null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            customer.Name = customerDto.Name;
            customer.Country = customerDto.Country;
            customer.City = customerDto.City;
            customer.Address = customerDto.Address;
            customer.Phone = customerDto.Phone;
            customer.ContactPerson = customerDto.ContactPerson;
            customer.Domain = customerDto.Domain;
            customer.BnNumber = customerDto.BnNumber;
            customer.IsActive = customerDto.IsActive;
            customer.UpdatedAt = DateTime.UtcNow;
        }

        public static CustomerDto ToDto(this Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Country = customer.Country,
                City = customer.City,
                Address = customer.Address,
                Phone = customer.Phone,
                ContactPerson = customer.ContactPerson,
                Domain = customer.Domain,
                BnNumber = customer.BnNumber,
                CreatedAt = customer.CreatedAt,
                IsActive = customer.IsActive,
                UpdatedAt = customer.UpdatedAt
            };
        }
    }
}
