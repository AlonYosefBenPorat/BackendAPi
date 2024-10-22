using ApiWebApp.Dto;
using DAL.Models.UsersModel;
using DAL.Models.utilitiesModel;

namespace ApiWebApp.Mapping;

public static class UsersMap
{
    public static object ToDto(AppUsers user, IEnumerable<string> roles)
    {
        return new
        {
            user.Id,
            user.Email,
            user.PhoneNumber,
            user.FirstName,
            user.LastName,
            user.DateOfBirth,
            user.JobTitle,
            user.IsEnabled,
            user.CreatedAt,
            user.UpdatedAt,
            user.LastPasswordUpdated,
            Roles = roles,
            ProfileImage = new
            {
                Alt = user.ProfileImage?.Alt ?? string.Empty,
                Src = user.ProfileImage?.Src ?? string.Empty
            }
        };
    }

    public static AppUsers ToModel(UserDto userDto)
    {
        return new AppUsers
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            UserName = userDto.Email,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            DateOfBirth = userDto.DateOfBirth,
            JobTitle = userDto.JobTitle ?? string.Empty,
            IsEnabled = userDto.IsEnabled,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            LastPasswordUpdated= null,
            ProfileImage = new Image
            {
                Alt = userDto.ProfileAlt ?? string.Empty,
                Src = userDto.ProfileSrc ?? string.Empty
            }
        };
    }
    public static void UpdateModel(AppUsers user, UserDto userDto)
    {
        user.FirstName = userDto.FirstName ?? user.FirstName;
        user.LastName = userDto.LastName ?? user.LastName;
        user.PhoneNumber = userDto.PhoneNumber ?? user.PhoneNumber;
        user.JobTitle = userDto.JobTitle ?? user.JobTitle;
        user.IsEnabled = userDto.IsEnabled;
        user.UpdatedAt = DateTime.UtcNow;

        user.ProfileImage ??= new Image();
        user.ProfileImage.Alt = userDto.ProfileAlt ?? user.ProfileImage.Alt ?? string.Empty;
        user.ProfileImage.Src = userDto.ProfileSrc ?? user.ProfileImage.Src ?? string.Empty;
    }



}
