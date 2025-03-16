using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using RepositoryLayer.Entity;


namespace AddressBookApplication.Controllers
{
    [ApiController]
    [Route("api/addressBook")]
    public class AddressBookController : ControllerBase
    {

        private readonly IAddressBL _addressBL;

        public AddressBookController(IAddressBL addressBL)
        {
            _addressBL = addressBL;
        }


        /// <summary>
        /// Get all Contacts in addressBook
        /// </summary>
        /// <returns>ResponseModel<List<AddressEntryEntity>></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var result = _addressBL.GetAllAddress();
            var response = new ResponseModel<List<AddressEntryEntity>>();

            if (result != null)
            {
                response.Success = true;
                response.Message = "Contact found successfully";
                response.Data = result;

                return Ok(response);
            }

            return NotFound(response);
        }


        /// <summary>
        /// Get Contact by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns><AddressEntryEntity/returns>
        [HttpGet("{id}")]

        public IActionResult GetAddressByID(int id)
        {
            var result = _addressBL.GetAddressByID(id);
            var response = new ResponseModel<AddressEntryEntity>();
            if (result != null)
            {
                response.Success = true;
                response.Message = "Contact found successfully";
                response.Data = result;

                return Ok(response);
            }
            return NotFound(response);
        }

        /// <summary>
        /// Adds New Contact to AddressBook
        /// </summary>
        /// <param name="newContact"></param>
        /// <returns>AddContactModel</returns>
        [HttpPost]

        public IActionResult AddContact(AddContactModel newContact)
        {
            var result = _addressBL.AddContact(newContact);
            var response = new ResponseModel<AddContactModel>();

            if (result != null)
            {
                response.Success = true;
                response.Message = "Contact added successfully";
                response.Data = result;

                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Update Contact in AddressBook
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateContact"></param>
        /// <returns>UpdateContactModel</returns>
        [HttpPut("{id}")]

        public IActionResult UpdateContact(int id, UpdateContactModel updateContact)
        {
            var result = _addressBL.UpdateContact(id, updateContact);
            var response = new ResponseModel<UpdateContactModel>();
            if (result != null)
            {
                response.Success = true;
                response.Message = "Contact updated successfully";
                response.Data = result;
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Delete Contact by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        [HttpDelete("{id}")]

        public IActionResult DeleteContact(int id)
        {
            var result = _addressBL.DeleteContact(id);
            var response = new ResponseModel<bool>();
            if (result)
            {
                response.Success = true;
                response.Message = "Contact deleted successfully";
                response.Data = result;
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}
