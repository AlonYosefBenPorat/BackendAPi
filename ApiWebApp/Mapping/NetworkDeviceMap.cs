using ApiWebApp.Model;
using DAL.Models;

namespace ApiWebApp.Mapping;

public static  class NetworkDeviceMap { 

    public static NetworkDevice ToEntity(this NetworkDeviceDto networkDeviceDto)
    {
        return new NetworkDevice
        {
            Id = Guid.NewGuid(),
            Model = networkDeviceDto.Model,
            Brand = networkDeviceDto.Brand,
            Type = networkDeviceDto.Type,
            Vendor = networkDeviceDto.Vendor,
            IpAddress = networkDeviceDto.IpAddress,
            SerialNumber = networkDeviceDto.SerialNumber,
            Description = networkDeviceDto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            WarrantyExpiration = networkDeviceDto.WarrantyExpiration,
            CustomerId = networkDeviceDto.CustomerId
         
        };
    }
    public static void UpdateEntity(this NetworkDeviceDto networkDeviceDto, NetworkDevice networkDevice)
    {
        networkDevice.Model = networkDeviceDto.Model;
        networkDevice.Brand = networkDeviceDto.Brand;
        networkDevice.Type = networkDeviceDto.Type;
        networkDevice.Vendor = networkDeviceDto.Vendor;
        networkDevice.IpAddress = networkDeviceDto.IpAddress;
        networkDevice.SerialNumber = networkDeviceDto.SerialNumber;
        networkDevice.Description = networkDeviceDto.Description;
        networkDevice.WarrantyExpiration = networkDeviceDto.WarrantyExpiration;
        networkDevice.UpdatedAt = DateTime.UtcNow;
        networkDevice.CustomerId = networkDeviceDto.CustomerId;
    }

    public static NetworkDeviceDto ToDto(this NetworkDevice networkDevice)
    {
        return new NetworkDeviceDto
        {
            Id = networkDevice.Id,
            Model = networkDevice.Model,
            Brand = networkDevice.Brand,
            Type = networkDevice.Type,
            Vendor = networkDevice.Vendor,
            IpAddress = networkDevice.IpAddress,
            SerialNumber = networkDevice.SerialNumber,
            Description = networkDevice.Description,
            CreatedAt = networkDevice.CreatedAt,
            UpdatedAt = networkDevice.UpdatedAt,
            WarrantyExpiration = networkDevice.WarrantyExpiration,
            CustomerId = networkDevice.CustomerId
        };
    }

}
