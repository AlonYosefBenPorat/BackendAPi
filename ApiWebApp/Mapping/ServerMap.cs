using ApiWebApp.DTOs;
using DAL.Models.ItemsModel;

namespace ApiWebApp.Mapping
{
    public static class ServerMap
    {
        public static Server ToEntity(this ServerDto serverDto)
        {
            return new Server
            {
                Id = Guid.NewGuid(),
                CustomerId = serverDto.CustomerId,
                Hostname = serverDto.Hostname,
                IpAddress = serverDto.IpAddress,
                Model = serverDto.Model,
                Vendor = serverDto.Vendor,
                Type = serverDto.Type,
                SerialNumber = serverDto.SerialNumber,
                Ram = serverDto.Ram,
                Storage = serverDto.Storage,
                OperatingSystem = serverDto.OperatingSystem,
                Roles = serverDto.Roles,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                WarrantyExpiration = serverDto.WarrantyExpiration
            };
        }

        public static void UpdateEntity(this ServerDto serverDto, Server server)
        {
            server.CustomerId = serverDto.CustomerId;
            server.Hostname = serverDto.Hostname;
            server.IpAddress = serverDto.IpAddress;
            server.Model = serverDto.Model;
            server.Vendor = serverDto.Vendor;
            server.Type = serverDto.Type;
            server.SerialNumber = serverDto.SerialNumber;
            server.Ram = serverDto.Ram;
            server.Storage = serverDto.Storage;
            server.OperatingSystem = serverDto.OperatingSystem;
            server.Roles = serverDto.Roles;
            server.WarrantyExpiration = serverDto.WarrantyExpiration;
            server.UpdatedAt = DateTime.UtcNow;
        }

        public static ServerDto ToDto(this Server server)
        {
            return new ServerDto
            {
                Id = server.Id,
                CustomerId = server.CustomerId,
                Hostname = server.Hostname,
                IpAddress = server.IpAddress,
                Model = server.Model,
                Vendor = server.Vendor,
                Type = server.Type,
                SerialNumber = server.SerialNumber,
                Ram = server.Ram,
                Storage = server.Storage,
                OperatingSystem = server.OperatingSystem,
                Roles = server.Roles,
                CreatedAt = server.CreatedAt,
                UpdatedAt = server.UpdatedAt,
                WarrantyExpiration = server.WarrantyExpiration
            };
        }
    }
}
