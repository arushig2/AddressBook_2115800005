using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interface;
using ModelLayer.Model;
using AddressBookApplication.RabitMQ.Interface;

namespace AddressBookApplication.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IUserBL _userBL;
        private readonly IPublishSubscribeMQProducer _rabitMQProducer;


        public AuthController(IUserBL userBL, IPublishSubscribeMQProducer rabitMQProducer)
        {
            _userBL = userBL;
            _rabitMQProducer = rabitMQProducer;
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns>Register model</returns>

        [HttpPost("register")]

        public IActionResult Registration(RegistrationModel registerModel)
        {

            var result = _userBL.RegisterUser(registerModel);
            var response = new ResponseModel<UserDto>();

            if (result != null)
            {
                _rabitMQProducer.Publish(result);
                response.Success = true;
                response.Message = "User Registered successfully";
                response.Data = result;
                return Created(string.Empty, response);
            }
            response.Success = false;
            response.Message = "Already Registered";
            return BadRequest(response);
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns>Session Token</returns>
        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {

            var user = _userBL.LoginUser(loginModel);
            var response = new ResponseModel<string>();
            if (user != null)
            {
                _rabitMQProducer.Publish("User Login successful");
                response.Success = true;
                response.Message = "Login successful";
                response.Data = user;

                return Ok(response);
            }
            response.Success = false;
            response.Message = "Login failed";
            return Unauthorized(response);
        }

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="passwordModel"></param>
        /// <returns>Reset Token</returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordModel passwordModel)
        {
            var result = await _userBL.ForgetPassword(passwordModel.email);
            var response = new ResponseModel<string>();
            if (result != null)
            {
                response.Success = true;
                response.Message = $"Reset password link sent successfully to your email address {result}";
                return Ok(response);
            }
            response.Success = false;
            response.Message = $"User is not present with email id ={passwordModel.email}";
            return BadRequest(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="resetModel"></param>
        /// <returns>bool if password is reset</returns>
        [HttpPatch("reset-password")]
        public IActionResult ResetPassword([FromQuery] string token, ResetPasswordModel resetModel)
        {

            var result = _userBL.ResetPassword(resetModel.NewPassword, token);
            var response = new ResponseModel<bool>();
            if (result)
            {
                _rabitMQProducer.Publish("Password reset successful");
                response.Success = true;
                response.Message = "Password reset successful";
                response.Data = result;
                return Ok(response);
            }
            response.Success = false;
            response.Message = "Error occurred resetting password. Please try again.";
            return BadRequest(response);
        }

    }
}
