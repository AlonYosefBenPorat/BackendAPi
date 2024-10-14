using ApiWebApp.Dto;
using DAL.Models;

namespace ApiWebApp.Mapping;

public static class FirewallMap
{
    public static Firewall ToEntity(this FirewallDto firewallDto)
    {
        return new Firewall
        {
            Id = Guid.NewGuid(),
           
            Version = firewallDto.Version,
            Model = firewallDto.Model,
            SerialNumber = firewallDto.SerialNumber,
            IpAddress = firewallDto.IpAddress,
            MacAddress = firewallDto.MacAddress,
            License = firewallDto.License,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            IsActive = firewallDto.IsActive, 
            CustomerId = firewallDto.CustomerId,
        };
    }
    public static void UpdateEntity(this FirewallDto firewallDto, Firewall firewall)
    {
        
        firewall.Version = firewallDto.Version;
        firewall.Model = firewallDto.Model;
        firewall.SerialNumber = firewallDto.SerialNumber;
        firewall.IpAddress = firewallDto.IpAddress;
        firewall.MacAddress = firewallDto.MacAddress;
        firewall.License = firewallDto.License;
        firewall.UpdatedAt = DateTime.UtcNow;
        firewall.IsActive = firewallDto.IsActive;
        firewall.CustomerId = firewallDto.CustomerId;
    }
    public static FirewallDto ToDto(this Firewall firewall)
    {
        return new FirewallDto
        {
            Id = firewall.Id,
            
            Version = firewall.Version,
            Model = firewall.Model,
            SerialNumber = firewall.SerialNumber,
            IpAddress = firewall.IpAddress,
            MacAddress = firewall.MacAddress,
            License = firewall.License,
            CreatedAt = firewall.CreatedAt,
            IsActive = firewall.IsActive,
            UpdatedAt = firewall.UpdatedAt,
            CustomerId = firewall.CustomerId,
        };
    }
}
