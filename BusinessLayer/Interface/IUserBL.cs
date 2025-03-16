using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public UserDto RegisterUser(RegistrationModel userRegistration);
        public string LoginUser(LoginModel userLoginDto);
        public Task<string> ForgetPassword(string email);
        public bool ResetPassword(string newPassword, string token);

    }
}
