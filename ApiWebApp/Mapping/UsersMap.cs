using ApiWebApp.DAL.Model;
using ApiWebApp.Dto;

namespace ApiWebApp.Mapping
{
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
                ProfileImage = new Image
                {
                    Alt = userDto.ProfileAlt ?? string.Empty,
                    Src = userDto.ProfileSrc ?? string.Empty
                }
            };
        }
    }
}
