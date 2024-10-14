using ApiWebApp.DAL.Model;
using ApiWebApp.Dto;

namespace ApiWebApp.Mapping
{
    public static class CustomerMap
    {
        public static Customer ToEntity(this CustomerDto customerDto)
        {
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
                UpdatedAt = null,
                IsActive = customerDto.IsActive,
                Logo = new Image
                {
                    Alt = customerDto.Logo.Alt,
                    Src = customerDto.Logo.Src
                }
            };
        }

        public static void UpdateEntity(this CustomerDto customerDto, Customer customer)
        {
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
            customer.Logo.Alt = customerDto.Logo.Alt;
            customer.Logo.Src = customerDto.Logo.Src;
        }
        //Cheak id need this method
        //public static CustomerDto ToDto(this Customer customer)
        //{
        //    return new CustomerDto
        //    {
        //        Id = customer.Id,
        //        Name = customer.Name,
        //        Country = customer.Country,
        //        City = customer.City,
        //        Address = customer.Address,
        //        Phone = customer.Phone,
        //        ContactPerson = customer.ContactPerson,
        //        Domain = customer.Domain,
        //        BnNumber = customer.BnNumber,
        //        CreatedAt = customer.CreatedAt,
        //        UpdatedAt = customer.UpdatedAt,
        //        IsActive = customer.IsActive,
        //        Logo = new ImageDto
        //        {
        //            Alt = customer.Logo.Alt,
        //            Src = customer.Logo.Src
        //        }
        //    };
        }
    }

