using ApiWebApp.DAL.Model;
using ApiWebApp.DTOs;
using Microsoft.Identity.Client;

namespace ApiWebApp.Mapping;

public static class ServerMap
{
    public static Server ToEntity(this ServerDto serverDto)
    {
        return new Server
        {
            Id = Guid.NewGuid(),
            Hostname = serverDto.Hostname,
            IpAddress = serverDto.IpAddress,
            Model = serverDto.Model,
            Brand = serverDto.Brand,
            Type = serverDto.Type,
            SerialNumber = serverDto.SerialNumber,
            Vendor = serverDto.Vendor,
            Ram = serverDto.Ram,
            Storage = serverDto.Storage,
            OperatingSystem = serverDto.OperatingSystem,
            Roles = serverDto.Roles,
            Description = serverDto.Description,
            WarrantyExpiration = serverDto.WarrantyExpiration,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            CustomerId = serverDto.CustomerId
        };    
    }
    public static void UpdateEntity(this ServerDto serverDto, Server server)
    {
        server.CustomerId = serverDto.CustomerId;
        server.Hostname = serverDto.Hostname;
        server.IpAddress = serverDto.IpAddress;
        server.Model = serverDto.Model;
        server.Brand = serverDto.Brand;
        server.Type = serverDto.Type;
        server.SerialNumber = serverDto.SerialNumber;
        server.Vendor = serverDto.Vendor;
        server.Ram = serverDto.Ram;
        server.Storage = serverDto.Storage;
        server.OperatingSystem = serverDto.OperatingSystem;
        server.Roles = serverDto.Roles;
        server.Description = serverDto.Description;
        server.WarrantyExpiration = serverDto.WarrantyExpiration;
        server.UpdatedAt = DateTime.UtcNow;
        server.CustomerId = serverDto.CustomerId;
    }
    public static ServerDto ToDto(this Server server)
    {
        return new ServerDto
        {
            Id = server.Id,
            Hostname = server.Hostname,
            IpAddress = server.IpAddress,
            Model = server.Model,
            Brand = server.Brand,
            Type = server.Type,
            SerialNumber = server.SerialNumber,
            Vendor = server.Vendor,
            Ram = server.Ram,
            Storage = server.Storage,
            OperatingSystem = server.OperatingSystem,
            Roles = server.Roles,
            Description = server.Description,
            WarrantyExpiration = server.WarrantyExpiration,
            CreatedAt = server.CreatedAt,
            UpdatedAt = server.UpdatedAt,
            CustomerId = server.CustomerId
        };
    }


}
