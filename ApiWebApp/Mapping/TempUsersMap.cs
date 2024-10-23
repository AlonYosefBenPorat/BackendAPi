using ApiWebApp.Dto;
using DAL.Models.UsersModel;
using DAL.Models.utilitiesModel; 

namespace ApiWebApp.Mapping;

public class TempUsersMap
{
    public static AppUsersTemp ToModel(UserTempDto userTempDto)
    {
        return new AppUsersTemp
        {   
            FirstName = userTempDto.FirstName,
            LastName = userTempDto.LastName,
            DateOfBirth = userTempDto.DateOfBirth,
            JobTitle = userTempDto.JobTitle,
            Email = userTempDto.Email,
            PhoneNumber = userTempDto.PhoneNumber, 
            CreatedAt = DateTime.UtcNow,
            ProfileImage = new Image
            {
                Alt = userTempDto.ProfileAlt ?? string.Empty,
                Src = userTempDto.ProfileSrc ?? string.Empty
            }
           
        };
    }

   
}
