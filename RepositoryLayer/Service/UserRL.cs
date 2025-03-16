using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Hashing;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Token;


namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        AddressBookContext _dbContext;
        private readonly HashPassword _hashPassword;
        private readonly IConfiguration _config;
        private readonly JwtToken _jwtToken;
        private readonly EmailService _emailService;
      


        public UserRL(AddressBookContext dbContext, HashPassword hashPassword, IConfiguration config, JwtToken jwtToken, EmailService emailService) 
        {
            _dbContext = dbContext;
            _hashPassword = hashPassword;
            _config = config;
            _jwtToken = jwtToken;
            _emailService = emailService;
        }

        public UserEntity RegisterUser(RegistrationModel registration)
        {
            var result = _dbContext.Users.FirstOrDefault<UserEntity>(e => e.email == registration.email);

            if (result == null)
            {
                var User = new UserEntity
                {
                    username = registration.username,
                    email = registration.email,
                    password = _hashPassword.PasswordHashing(registration.password),
                    role = registration.role
                };

                _dbContext.Users.Add(User);
                _dbContext.SaveChanges();

                return User;
            }
            return null;
        }

        public string LoginUser(LoginModel userLoginDto)
        {
            var validUser = _dbContext.Users.FirstOrDefault(e => e.email == userLoginDto.email);

            if (validUser != null)
            {
                if (_hashPassword.VerifyPassword(userLoginDto.password, validUser.password))
                {
                    var token = _jwtToken.GenerateToken(validUser);
                    return token;
                }

            }
            return null;
        }

        public async Task<string> ForgetPassword(string email)
        {
            var validUser = _dbContext.Users.FirstOrDefault(e => e.email == email);

            if (validUser != null)
            {

                var generatedToken = _jwtToken.GenerateTokenReset(validUser.email, validUser.id);

                var baseUrl = _config["ResetURL:ResetPasswordUrl"];
                var callbackUrl = $"{baseUrl}?token={generatedToken}";

                await _emailService.SendEmailAsync(email, "Reset Password", callbackUrl);

                return "Ok";
            }
            return null;
        }

        public bool ResetPassword(string newPassword, int userId)
        {

            var validUser = _dbContext.Users.FirstOrDefault(e => e.id == userId);

            if (validUser != null)
            {
                validUser.password = _hashPassword.PasswordHashing(newPassword);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
