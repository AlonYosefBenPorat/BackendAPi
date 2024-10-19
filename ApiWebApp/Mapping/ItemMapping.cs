using ApiWebApp.DAL.Model;
using ApiWebApp.Dto;
using DAL.Models;

namespace ApiWebApp.Mapping
{
    public static class ItemMapping
    {
        public static ItemDto ToDto(Server server) => new ItemDto
        {   CustomerId = server.CustomerId,
            Id = server.Id,
            Type = "Server",
            Details = server.ToDto()
        };

        // Add similar methods for other models
        public static ItemDto ToDto(NetworkDevice networkDevice) => new ItemDto
        {   CustomerId = networkDevice.CustomerId,
            Id = networkDevice.Id,
            Type = "NetworkDevice",
            Details = networkDevice.ToDto()

        };

        public static ItemDto ToDto(Firewall firewall) => new ItemDto
        {
            CustomerId = firewall.CustomerId,
            Id = firewall.Id,
            Type = "Firewall",
            Details = firewall.ToDto()
        };

        public static ItemDto ToDto(Asset asset) => new ItemDto
        {   CustomerId = asset.CustomerId,
            Id = asset.Id,
            Type = "Asset",
            Details = asset.ToDto()

        };

        public static ItemDto ToDto(Backup backup) => new ItemDto
        {
            CustomerId = backup.CustomerId,
            Id = backup.Id,
            Type = "Backup",
            Details = backup.ToDto()
        };
    }
}
