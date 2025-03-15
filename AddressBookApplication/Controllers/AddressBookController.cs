using Microsoft.AspNetCore.Mvc;

namespace AddressBookApplication.Controllers
{
    [ApiController]
    [Route("api/addressBook")]
    public class AddressBookController : ControllerBase
    {

        [HttpGet]
        public IActionResult Hello()
        {
            return Ok("HelloWorld!");
        }

    }
}
