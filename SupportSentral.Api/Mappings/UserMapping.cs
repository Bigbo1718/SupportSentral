using Microsoft.EntityFrameworkCore;
using SupportSentral.Api.Contracts;
using SupportSentral.Api.Entities;

namespace SupportSentral.Api.Mappings;

public static class UserMapping
{
   public static UserContract MapToContract(this User user)
   {
      return new(user.Id, user.Email, user.Name);
   }

   public static User MapToEntity(this UpdateUserContract user, Guid id )
   {
      return new User
      {
         Id = id,
         Email = user.Email,
         Name = user.Name
      };
   }
}