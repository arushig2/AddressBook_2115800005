using BusinessLayer.Interface;
using RepositoryLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Token;
using System.Security.Claims;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        IUserRL _userRl;
        JwtToken _jwtToken;

        public UserBL(IUserRL userRl, JwtToken jwtToken)
        {
            _userRl = userRl;
            _jwtToken = jwtToken;
        }

        public UserDto RegisterUser(RegistrationModel userRegistration)
        {
            var user = _userRl.RegisterUser(userRegistration);
            if (user != null)
            {
                return new UserDto()
                {
                    username = user.username,
                    email = user.email
                };
            }
            return null;
        }

        public string LoginUser(LoginModel userLoginDto)
        {
            return _userRl.LoginUser(userLoginDto);
        }

        public Task<string> ForgetPassword(string email)
        {
            return _userRl.ForgetPassword(email);
        }
        public bool ResetPassword(string newPassword, string token)
        {
            var principal = _jwtToken.GetTokenValidation(token);
            var userId = Convert.ToInt32(principal.FindFirstValue("UserId"));
            return _userRl.ResetPassword(newPassword, userId);
        }

    }
}
