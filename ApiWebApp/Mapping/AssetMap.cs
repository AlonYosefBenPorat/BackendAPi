using ApiWebApp.Dto;
using DAL.Models;

namespace ApiWebApp.Mapping
{
    public static class AssetMap
    {
        public static Asset ToEntity(this AssetDto assetDto)
        {
            return new Asset
            {
                Id = Guid.NewGuid(),
                Type = assetDto.Type,
                IpAddress = assetDto.IpAddress,
                Url = assetDto.Url,
                License = assetDto.License,
                CreatedAt = DateTime.UtcNow,
                SupportExpiration = assetDto.SupportExpiration,
                Notes = assetDto.Notes,
                UpdatedAt = null,
                CustomerId = assetDto.CustomerId
            };
        }

        public static void UpdateEntity(this AssetDto assetDto, Asset asset)
        {
            asset.Type = assetDto.Type;
            asset.IpAddress = assetDto.IpAddress;
            asset.Url = assetDto.Url;
            asset.License = assetDto.License;
            asset.SupportExpiration = assetDto.SupportExpiration;
            asset.Notes = assetDto.Notes;
            asset.UpdatedAt = DateTime.UtcNow;
            asset.CustomerId = assetDto.CustomerId;
        }

        public static AssetDto ToDto(this Asset asset)
        {
            return new AssetDto
            {
                Id = asset.Id,
                Type = asset.Type,
                IpAddress = asset.IpAddress,
                Url = asset.Url,
                License = asset.License,
                CreatedAt = asset.CreatedAt,
                SupportExpiration = asset.SupportExpiration,
                Notes = asset.Notes,
                UpdatedAt = asset.UpdatedAt,
                CustomerId = asset.CustomerId
            };
        }
    }
}
