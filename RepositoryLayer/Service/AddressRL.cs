using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class AddressRL : IAddressRL
    {
        AddressBookContext _dbContext;
        public AddressRL(AddressBookContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<AddressEntryEntity> GetAllAddress()
        {
            var result = _dbContext.AddressBooks.ToList();
            return result;
        }

        public AddressEntryEntity GetAddressByID(int id)
        {
            var result = _dbContext.AddressBooks.Where(x => x.id == id).FirstOrDefault();
            return result;
        }

        public AddContactModel AddContact(AddContactModel newContact)
        {
            var result = _dbContext.AddressBooks.FirstOrDefault<AddressEntryEntity>(x => x.email == newContact.email);

            if (result == null)
            {
                AddressEntryEntity addressEntry = new AddressEntryEntity()
                {

                    name = newContact.name,
                    address = newContact.address,
                    phone = newContact.phone,
                    email = newContact.email,

                };

                _dbContext.AddressBooks.Add(addressEntry);
                _dbContext.SaveChanges();
                return newContact;
            }
            else
            {
                return null;

            }
        }

        public UpdateContactModel UpdateContact(int id, UpdateContactModel updateContact)
        {
            var result = _dbContext.AddressBooks.FirstOrDefault(x => x.id == id);

            if (result != null)
            {
                result.name = updateContact.name;
                result.address = updateContact.address;
                result.phone = updateContact.phone;
                result.email = updateContact.email;

                _dbContext.SaveChanges();
                return updateContact;
            }
            else
            {
                return null;
            }
        }

        public bool DeleteContact(int id)
        {
            var result = _dbContext.AddressBooks.FirstOrDefault(x => x.id == id);
            if (result != null)
            {
                _dbContext.AddressBooks.Remove(result);
                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
