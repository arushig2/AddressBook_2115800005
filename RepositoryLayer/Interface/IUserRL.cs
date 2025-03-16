using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public UserEntity RegisterUser(RegistrationModel registration);
        public string LoginUser(LoginModel loginDto);
        public Task<string> ForgetPassword(string email);
        public bool ResetPassword(string newPassword, int userId);
    }
}
