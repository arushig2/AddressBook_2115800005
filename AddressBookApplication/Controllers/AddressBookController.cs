using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;


namespace AddressBookApplication.Controllers
{
    [ApiController]
    [Route("api/addressBook")]
    public class AddressBookController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]

        public IActionResult GetAddressByID(int id)
        {
            throw new NotImplementedException();
        }

      
        [HttpPost]

        public IActionResult AddContact()
        {
            throw new NotImplementedException();
        }

       
        [HttpPut("{id}")]

        public IActionResult UpdateContact()
        {
            throw new NotImplementedException();
        }


        [HttpDelete("{id}")]

        public IActionResult DeleteContact(int id)
        {
            throw new NotImplementedException();
        }

    }
}
