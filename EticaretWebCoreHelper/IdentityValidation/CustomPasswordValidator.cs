using EticaretWebCoreEntity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreHelper
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if(password.ToLower().Contains(user.UserName.ToLower()))
            {

                if(!user.Email.Contains(user.UserName))
                {
                    errors.Add(new IdentityError() { Code = "PasswordContainsUserName", Description = "Şifre Alanı Kullanıcı Adı İçeremez" });
                }
            }

           if(errors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
           else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));

            }
        }
    }
}
