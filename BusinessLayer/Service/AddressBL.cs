using System;
using BusinessLayer.Interface;
using RepositoryLayer.Interface;
using RepositoryLayer.Entity;
using ModelLayer.Model;

namespace BusinessLayer.Service
{
    public class AddressBL : IAddressBL
    {
        private readonly IAddressRL _addressRL;

        public AddressBL(IAddressRL addressRL)
        {
            _addressRL = addressRL;
        }

        public List<AddressEntryEntity> GetAllAddress()
        {
            return _addressRL.GetAllAddress();
        }

        public AddressEntryEntity GetAddressByID(int id)
        {
            return _addressRL.GetAddressByID(id);
        }

        public AddContactModel AddContact(AddContactModel newContact)
        {
            return _addressRL.AddContact(newContact);
        }

        public UpdateContactModel UpdateContact(int id, UpdateContactModel updateContact)
        {
            return _addressRL.UpdateContact(id, updateContact);
        }

        public bool DeleteContact(int id)
        {
            return _addressRL.DeleteContact(id);
        }
    }
}
