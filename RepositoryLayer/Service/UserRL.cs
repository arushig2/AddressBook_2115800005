﻿using ModelLayer.Model;
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
      


        public UserRL(AddressBookContext dbContext, HashPassword hashPassword, IConfiguration config, JwtToken jwtToken)
        {
            _dbContext = dbContext;
            _hashPassword = hashPassword;
            _config = config;
            _jwtToken = jwtToken;
            
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

    }
}
